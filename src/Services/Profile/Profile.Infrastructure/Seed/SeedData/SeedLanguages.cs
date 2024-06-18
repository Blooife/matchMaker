using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Seed.SeedData;

public class SeedLanguages : IDataSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>().HasData(
            new Language()
            {
                Id = 1, Name = "Русский"
            },new Language()
            {
                Id = 2, Name = "Английский"
            },new Language()
            {
                Id = 3, Name = "Польский"
            },new Language()
            {
                Id = 4, Name = "Французский"
            },new Language()
            {
                Id = 5, Name = "Немецкий"
            },new Language()
            {
                Id = 6, Name = "Белорусский"
            },new Language()
            {
                Id = 7, Name = "Испанский"
            });
        
    }
}