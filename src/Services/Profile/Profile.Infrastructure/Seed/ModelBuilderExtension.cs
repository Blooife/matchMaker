using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Profile.Infrastructure.Seed;

public static class ModelBuilderExtensions
{
    public static void ApplyAllSeeds(this ModelBuilder modelBuilder, Assembly assembly)
    {
        var seeders = assembly.GetTypes()
            .Where(t => typeof(IDataSeeder).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IDataSeeder>();

        foreach (var seeder in seeders)
        {
            seeder.Seed(modelBuilder);
        }
    }
}
