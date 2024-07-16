using ApiGateway.Ocelot.Extensions;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration, builder.Environment);
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = app.Configuration.GetSection("OcelotOptions:PathToSwaggerGenerator").Value;
});

app.UseRouting();
app.MapControllers();

await app.UseAuthentication().UseOcelot();
app.UseAuthorization();

app.Run();