using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
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
	builder.Services.AddDbContext<AppDbContext>(
	opt => opt.UseInMemoryDatabase("InMem")
);
}
else if (builder.Environment.IsProduction())
{
	builder.Services.AddDbContext<AppDbContext>(
		opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"))
	);
}



builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine(" ==> USING INMEM.");

    app.UseSwagger();
    app.UseSwaggerUI();
}
else if(app.Environment.IsProduction())
{
    Console.WriteLine(" ==> USING SQLSERVERDB.");
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
PrepDb.PrepPopulation(app);

app.Run();
