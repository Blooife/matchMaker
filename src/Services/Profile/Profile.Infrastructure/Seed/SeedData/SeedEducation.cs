using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Seed.SeedData;

public class SeedEducation : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Education>().HasData(
            new Education()
            {
                Id = 1, Name = "Среднее"
            },new Education()
            {
                Id = 2, Name = "Среднее специальное"
            },new Education()
            {
                Id = 3, Name = "Высшее"
            },new Education()
            {
                Id = 4, Name = "Незаконченное высшее"
            },new Education()
            {
                Id = 5, Name = "Второе высшее"
            });
    }
}