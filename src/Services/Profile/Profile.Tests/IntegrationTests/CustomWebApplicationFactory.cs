using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Profile.Infrastructure.Contexts;
using Respawn;
using Shared.Constants;
using Shared.Models;
using StackExchange.Redis;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Encoding = System.Text.Encoding;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Profile.Tests.IntegrationTests;

public class CustomWebApplicationFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    private readonly KafkaContainer _kafkaContainer;
    private readonly IContainer _zookeeperContainer;
    private readonly RedisContainer _redisContainer;
    private readonly IContainer _minioContainer;
    private IConnectionMultiplexer _redisConnection = null!;
    private Respawner _respawner = null!;
    private DbConnection _dbConnection = null!;
    public string JwtToken;
    
    public CustomWebApplicationFactory()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("ProfileDbTest")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithPortBinding(0, true)
            .WithCleanUp(true)
            .Build();
        
        _zookeeperContainer = new ContainerBuilder()
            .WithImage("confluentinc/cp-zookeeper")
            .WithPortBinding(0, true)
            .WithEnvironment("ZOOKEEPER_CLIENT_PORT", "2181")
            .WithCleanUp(true)
            .Build();

        _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka")
            .WithPortBinding(0, true)
            .WithEnvironment("KAFKA_ZOOKEEPER_CONNECT", "localhost:2181")
            .WithEnvironment("KAFKA_ADVERTISED_LISTENERS", "PLAINTEXT://kafka:29092,PLAINTEXT_HOST://localhost:9092")
            .WithEnvironment("KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1")
            .WithCleanUp(true)
            .Build();
        
        _redisContainer = new RedisBuilder()
            .WithImage("redis:latest")
            .WithPortBinding(0, true)
            .WithCleanUp(true)
            .Build();
        
        _minioContainer = new ContainerBuilder()
            .WithImage("minio/minio:latest")
            .WithPortBinding(9000, true) 
            .WithCommand("server", "/data")
            .WithEnvironment("MINIO_ACCESS_KEY", "minioadmin")
            .WithEnvironment("MINIO_SECRET_KEY", "minioadmin")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9000))
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            
            var descriptorType =
                typeof(DbContextOptions<ProfileDbContext>);

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<ProfileDbContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
            });
        });
    }
    
    private string GenerateJwtToken()
    {
        using var scope = Services.CreateScope();
        var jwtOptions = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "user@example.com"),
                new Claim(ClaimTypes.Role, Roles.User) 
            }),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpiresInMinutes),
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    
    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }
    
    public async Task ResetDatabase()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task ResetRedisAsync()
    {
        var db = _redisConnection.GetDatabase();
        await db.ExecuteAsync("FLUSHDB");
    }
    
    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        await _zookeeperContainer.StartAsync();
        await _kafkaContainer.StartAsync();
        await _redisContainer.StartAsync();
        await _minioContainer.StartAsync();
        
        _dbConnection = new NpgsqlConnection(_postgresContainer.GetConnectionString());
        _redisConnection = await ConnectionMultiplexer.ConnectAsync(_redisContainer.GetConnectionString());
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();
        await dbContext.Database.MigrateAsync();
        
        await InitializeRespawnerAsync();
        
        var kafkaPort = _kafkaContainer.GetMappedPublicPort(9092);
        var kafkaAddress = $"localhost:{kafkaPort}";

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        config.GetSection("Kafka:Consumer")["BootstrapServers"] = kafkaAddress;
        config.GetSection("Kafka:Producer")["BootstrapServers"] = kafkaAddress;
        
        var minioPort = _minioContainer.GetMappedPublicPort(9000);
        var minioAddress = $"localhost:{minioPort}";

        config.GetSection("Minio")["Endpoint"] = minioAddress;
        
        JwtToken = GenerateJwtToken();
    }
    
    public new async Task DisposeAsync()
    {
        await _postgresContainer.StopAsync();
        await _zookeeperContainer.StopAsync();
        await _postgresContainer.StopAsync();
        await _redisContainer.StopAsync();
        await _minioContainer.StopAsync();
    }
}