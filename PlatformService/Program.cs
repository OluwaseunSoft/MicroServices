using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Profiles;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment environment = builder.Environment;
// Add services to the container.

System.Console.WriteLine("--> Using SqlServer DB");
builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
builder.Services.AddTransient<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddControllers();
// Auto Mapper Configurations

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
System.Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    System.Console.WriteLine("--> Using InMem DB");
builder.Services.AddDbContext<AppDbContext>(opt => 
    opt.UseInMemoryDatabase("InMem"));
    app.UseSwagger();
    app.UseSwaggerUI();    
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();
