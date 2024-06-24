using Microsoft.EntityFrameworkCore;

namespace Profile.Infrastructure.Seed;

public interface IDataSeeder
{
    void Seed(ModelBuilder modelBuilder);
}
