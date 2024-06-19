using Match.Infrastructure.Context;
using Match.Infrastructure.Extensions;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration["MatchDatabase:ConnectionString"]!;

builder.Services.Configure<MatchDbSettings>(builder.Configuration.GetSection("MatchDatabase"));
builder.Services.ConfigureInfrastructure(builder.Configuration);

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

builder.Services.AddScoped<IMongoDbContext, MatchDbContext>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();