using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Seed.SeedData;

public class SeedCities : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            // Россия
            new City()
            {
                Id = 1, Name = "Москва", CountryId = 1
            },
            new City()
            {
                Id = 2, Name = "Санкт-Петербург", CountryId = 1
            },
            new City()
            {
                Id = 3, Name = "Новосибирск", CountryId = 1
            },
            // Великобритания
            new City()
            {
                Id = 4, Name = "Лондон", CountryId = 2
            },
            new City()
            {
                Id = 5, Name = "Манчестер", CountryId = 2
            },
            new City()
            {
                Id = 6, Name = "Бирмингем", CountryId = 2
            },
            // США
            new City()
            {
                Id = 7, Name = "Нью-Йорк", CountryId = 3
            },
            new City()
            {
                Id = 8, Name = "Лос-Анджелес", CountryId = 3
            },
            new City()
            {
                Id = 9, Name = "Чикаго", CountryId = 3
            },
            // Канада
            new City()
            {
                Id = 10, Name = "Торонто", CountryId = 4
            },
            new City()
            {
                Id = 11, Name = "Ванкувер", CountryId = 4
            },
            new City()
            {
                Id = 12, Name = "Монреаль", CountryId = 4
            },
            // Польша
            new City()
            {
                Id = 13, Name = "Варшава", CountryId = 5
            },
            new City()
            {
                Id = 14, Name = "Краков", CountryId = 5
            },
            new City()
            {
                Id = 15, Name = "Вроцлав", CountryId = 5
            },
            // Франция
            new City()
            {
                Id = 16, Name = "Париж", CountryId = 6
            },
            new City()
            {
                Id = 17, Name = "Марсель", CountryId = 6
            },
            new City()
            {
                Id = 18, Name = "Лион", CountryId = 6
            },
            // Германия
            new City()
            {
                Id = 19, Name = "Берлин", CountryId = 7
            },
            new City()
            {
                Id = 20, Name = "Гамбург", CountryId = 7
            },
            new City()
            {
                Id = 21, Name = "Мюнхен", CountryId = 7
            },
            // Беларусь
            new City()
            {
                Id = 22, Name = "Минск", CountryId = 8
            },
            new City()
            {
                Id = 23, Name = "Гомель", CountryId = 8
            },
            new City()
            {
                Id = 24, Name = "Могилёв", CountryId = 8
            },
            // Испания
            new City()
            {
                Id = 25, Name = "Мадрид", CountryId = 9
            },
            new City()
            {
                Id = 26, Name = "Барселона", CountryId = 9
            },
            new City()
            {
                Id = 27, Name = "Валенсия", CountryId = 9
            });
    }
}
