using Faces.Shared.Common.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Orders.Core.Extensions;
using Orders.Core.Hubs;
using Orders.Core.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR()
    .AddJsonProtocol(opt =>
    {
        opt.PayloadSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddCoreStartup(builder.Configuration);
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration["OrdersContextConnection"])
    .AddRabbitMQ("amqp://rabbitmq://rabbitmq:5672/", sslOption: null)
    .AddSignalRHub("http://orders-api/orderhub", "SignalR Health Check", Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed((host) => true)
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseMiddleware<GlobalExceptionHandler>();

app.MapHealthChecks("/orders/healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.UseRouting();

app.UseAuthorization();

app.MapHub<OrderHub>("/orderhub");

app.UseCors("CorsPolicy");

using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
scope.ServiceProvider.GetService<OrdersContext>()!.MigrateDB();

app.Run();