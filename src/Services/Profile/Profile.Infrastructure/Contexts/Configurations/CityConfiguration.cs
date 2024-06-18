using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Contexts.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(city => city.Id);
        builder.Property(city => city.Id).IsRequired().ValueGeneratedOnAdd();
    }
}