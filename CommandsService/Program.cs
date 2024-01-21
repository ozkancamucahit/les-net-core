



using CommandsService.AsyncDataServices;
using CommandsService.Data;
using CommandsService.EventProcessing;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddHostedService<MessageBusSubscriber>()
    .AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMEM"))
    .AddScoped<ICommandRepo, CommandRepo>()
    .AddSingleton<IEventProcesor, EventProcesor>()
    .AddScoped<IPlatformDataClient, PlatformDataClient>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDB.PrepPopulation(app);

app.Run();
