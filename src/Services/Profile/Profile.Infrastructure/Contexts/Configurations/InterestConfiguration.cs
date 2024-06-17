using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Contexts.Configurations;

public class InterestConfiguration : IEntityTypeConfiguration<Interest>
{
    public void Configure(EntityTypeBuilder<Interest> builder)
    {
        builder.HasKey(interest => interest.Id);
        builder.Property(interest => interest.Id).IsRequired().ValueGeneratedOnAdd();
        builder.HasIndex(interest=> interest.Name).IsUnique();
    }
}