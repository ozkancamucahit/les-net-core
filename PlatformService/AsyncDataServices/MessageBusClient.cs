using PlatformService.DTOS;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
	public sealed class MessageBusClient : IMessageBusClient
	{
		private readonly IConfiguration _configuration;
		private readonly IConnection _connection;
		private readonly IModel _channel;

		#region CTOR
		public MessageBusClient(IConfiguration configuration)
        {
			this._configuration = configuration;

			var factory = new ConnectionFactory { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };

			try
			{
				_connection = factory.CreateConnection();
				_channel = _connection.CreateModel();

				_channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
				_connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine(" ==> CONNECTED TO MESSAGE BUS");
            }
			catch (Exception ex)
			{
                Console.WriteLine(" ==> COULD NOT CONNECT TO MESSAGE BUS :" + ex.Message);
            }
		}
        #endregion

        public void PublishNewPlatform(PlatformPublishedDTO platformPublishedDTO)
		{
			string message = JsonSerializer.Serialize(platformPublishedDTO);

			if(_connection.IsOpen)
			{
                Console.WriteLine(" ==> RABBITMQ CONNECTION OPEN; SENDING MESSAGE");
				SendMessage(message);
            }
			else
			{
                Console.WriteLine(" ==> RABBITMQ CONNECTION CLOSED; NOT SENDING MESSAGE");
			}
		}

		private void SendMessage(string message)
		{
			var body = Encoding.UTF8.GetBytes(message);
			_channel.BasicPublish(exchange: "trigger",
				routingKey: "",
				basicProperties: null,
				body: body);

            Console.WriteLine(" ==> SENT MESSAGE :" + message);
        }

		public void Dispose()
		{
            Console.WriteLine(" ==> MESSAGE BUS DISPOSED");
			if (_channel.IsOpen)
			{
				_channel.Close();
				_connection.Close();
			}
        }

		private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
		{
            Console.WriteLine(" ==> RABBITMQ SHUTTING DOWN");
        }

	}
}
