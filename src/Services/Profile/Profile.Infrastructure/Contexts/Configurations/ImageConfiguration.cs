using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Contexts.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();
    }
}