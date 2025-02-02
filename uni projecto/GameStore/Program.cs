using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog with MongoDB Sink
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/app-log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MongoDB("mongodb://localhost:27017/GameStoreLogs", collectionName: "logs") // MongoDB connection
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(); // Add Serilog to the app

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Serilog request logging
app.UseSerilogRequestLogging();

app.UseAuthorization();
app.MapControllers();
app.Run();
