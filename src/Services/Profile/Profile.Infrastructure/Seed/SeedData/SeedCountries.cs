using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Seed.SeedData;

public class SeedCountries : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>().HasData(
            new Country()
            {
                Id = 1, Name = "Россия"
            },new Country()
            {
                Id = 2, Name = "Великобритания"
            },new Country()
            {
                Id = 3, Name = "США"
            },new Country()
            {
                Id = 4, Name = "Канада"
            },new Country()
            {
                Id = 5, Name = "Польша"
            },new Country()
            {
                Id = 6, Name = "Франция"
            },new Country()
            {
                Id = 7, Name = "Германия"
            },new Country()
            {
                Id = 8, Name = "Беларусь"
            },new Country()
            {
                Id = 9, Name = "Испания"
            });
    }
}