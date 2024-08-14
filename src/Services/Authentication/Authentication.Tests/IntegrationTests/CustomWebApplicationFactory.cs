using System.Data.Common;
using Authentication.DataLayer.Contexts;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;

namespace Authentication.Tests.IntegrationTests;

public class CustomWebApplicationFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    private readonly KafkaContainer _kafkaContainer;
    private readonly IContainer _zookeeperContainer;
    private Respawner _respawner = null!;
    private DbConnection _dbConnection = null!;
    
    public CustomWebApplicationFactory()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("AuthDbTest")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithPortBinding(5432, false)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .WithCleanUp(true)
            .Build();
        
        _zookeeperContainer = new ContainerBuilder()
            .WithImage("confluentinc/cp-zookeeper")
            .WithPortBinding(2181, false)
            .WithEnvironment("ZOOKEEPER_CLIENT_PORT", "2181")
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(2181))
            .Build();

        _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka")
            .WithPortBinding(9092, false)
            .WithEnvironment("KAFKA_ZOOKEEPER_CONNECT", "localhost:2181")
            .WithEnvironment("KAFKA_ADVERTISED_LISTENERS", "PLAINTEXT://kafka:29092,PLAINTEXT_HOST://localhost:9092")
            .WithEnvironment("KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1")
            .WithCleanUp(true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9092))
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddHangfire(config => 
                config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(_postgresContainer.GetConnectionString())));
            
            var descriptorType =
                typeof(DbContextOptions<AuthContext>);

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<AuthContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));
        });
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

    
    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        await _zookeeperContainer.StartAsync();
        await _kafkaContainer.StartAsync();
        _dbConnection = new NpgsqlConnection(_postgresContainer.GetConnectionString());
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
        await dbContext.Database.MigrateAsync();
        await InitializeRespawnerAsync();
    }
    
    new public async Task DisposeAsync()
    {
        await _postgresContainer.StopAsync();
        await _zookeeperContainer.StopAsync();
        await _postgresContainer.StopAsync();
    }
}