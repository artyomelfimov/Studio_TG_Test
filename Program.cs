using Microsoft.OpenApi.Models;
using StudioTG_Test.Models;
using StudioTG_Test.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSingleton<IGameService,GameService>();

builder.Services.AddSwaggerGen(opts =>
{
    opts.MapType(typeof(Cell), () => new OpenApiSchema
    {
        Type = "string"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();




