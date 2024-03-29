using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


if (builder.Environment.IsDevelopment())
{
    Console.WriteLine(" ==> USING INMEM.");

    builder.Services.AddDbContext<AppDbContext>(
    opt => opt.UseInMemoryDatabase("InMem")
);
}
else if (builder.Environment.IsProduction())
{
    Console.WriteLine(" ==> USING SQLSERVERDB.");

	builder.Services.AddDbContext<AppDbContext>(
		opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"))
	);
 }



builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services
    .AddSingleton<IMessageBusClient, MessageBusClient>()
    .AddGrpc();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/Platforms.proto", async (context) => {
    await context.Response.WriteAsync(File.ReadAllText("Protos/Platforms.proto"));
});


PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
