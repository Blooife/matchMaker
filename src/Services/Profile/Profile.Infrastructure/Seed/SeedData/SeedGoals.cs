using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Seed.SeedData;

public class SeedGoals : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Goal>().HasData(
            new Goal()
            {
                Id = 1, Name = "Дружба"
            },new Goal()
            {
                Id = 2, Name = "Общение"
            },new Goal()
            {
                Id = 3, Name = "Отношения"
            },new Goal()
            {
                Id = 4, Name = "Семья"
            });
    }
}