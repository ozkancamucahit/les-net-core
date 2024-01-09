
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace CommandsService.AsyncDataServices
{
	public sealed class MessageBusSubscriber : BackgroundService
	{
		private readonly IConfiguration configuration;
		private readonly IEventProcesor eventProcesor;
		private IConnection connection;
		private IModel channel;
		private string queueName;

		#region CTOR
		public MessageBusSubscriber(IConfiguration configuration, IEventProcesor eventProcesor)
        {
			this.configuration = configuration;
			this.eventProcesor = eventProcesor;
			InitializeRabbitMQ();
		}
        #endregion

		private void InitializeRabbitMQ()
		{
			var factory = new ConnectionFactory()
			{
				HostName = configuration["RabbitMQHost"],
				Port = int.Parse(configuration["RabbitMQPort"]),
			};

			connection = factory.CreateConnection();
			channel = connection.CreateModel();
			channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
			queueName = channel.QueueDeclare().QueueName;
			channel.QueueBind(queue:queueName,
				exchange: "trigger",
				routingKey: ""
				);

            Console.WriteLine(" ==> LISTENING ON THE MESSAGE BUS...");
			connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        }

		private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
		{
            Console.WriteLine(" ==> CONNECTION SHUTDOWN.");
        }

		public override void Dispose()
		{
			if (channel.IsOpen)
			{
				channel.Close();
				connection.Close();
			}
			base.Dispose();
		}


		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(channel);

			consumer.Received += (ModeuleHandle, ea) =>
			{
                Console.WriteLine(" ==> EVENT RECEIVED.");

				var body = ea.Body;
				var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
				eventProcesor.ProcessEvent(notificationMessage);
            };


			channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
			return Task.CompletedTask;

		}
	}
}
