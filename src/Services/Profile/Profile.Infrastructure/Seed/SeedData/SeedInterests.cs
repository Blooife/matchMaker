using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Seed.SeedData;

public class SeedInterests : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Interest>().HasData(
            new Interest()
            {
                Id = 1, Name = "Музыка"
            },new Interest()
            {
                Id = 2, Name = "Спорт"
            },new Interest()
            {
                Id = 3, Name = "Хип-хоп"
            },new Interest()
            {
                Id = 4, Name = "Искусство"
            },new Interest()
            {
                Id = 5, Name = "Мода"
            },new Interest()
            {
                Id = 6, Name = "Машины"
            },new Interest()
            {
                Id = 7, Name = "Еда"
            },new Interest()
            {
                Id = 8, Name = "Языки"
            },new Interest()
            {
                Id = 9, Name = "Наука"
            },new Interest()
            {
                Id = 10, Name = "Программирование"
            });
    }
}