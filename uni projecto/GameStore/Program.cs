using GameStore.Models;
using GameStore.Repositories;
using GameStore.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using GameStore.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
var builder = WebApplication.CreateBuilder(args);





// Register FluentValidation
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddValidatorsFromAssemblyContaining<OrderRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GameDiscountValidator>();

var app = builder.Build();
app.UseAuthorization();
app.MapControllers();
app.Run();

builder.Services.AddSingleton<GameStoreService>();

var builder = WebApplication.CreateBuilder(args);
// Configure MongoDB
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

// Register repositories
builder.Services.AddSingleton<GameRepository>();
builder.Services.AddSingleton<OrderRepository>();

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
