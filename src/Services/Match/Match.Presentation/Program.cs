using Match.Application.Extensions;
using Match.Infrastructure.Extensions;
using Match.Infrastructure.Hubs;
using Match.Presentation.MiddlewareHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.ConfigureApplication();

builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.MapHub<ChatHub>("/chat");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();