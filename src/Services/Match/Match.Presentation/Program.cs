using Match.Application.Extensions;
using Match.Infrastructure.Extensions;
using Match.Infrastructure.Hubs;
using Match.Presentation.Extensions;
using Match.Presentation.MiddlewareHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureInfrastructure(builder.Configuration);
builder.Services.ConfigureApplication();
builder.Services.ConfigurePresentation(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.MapHub<ChatHub>("/chat");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("MyCorsPolicy");

app.Run();