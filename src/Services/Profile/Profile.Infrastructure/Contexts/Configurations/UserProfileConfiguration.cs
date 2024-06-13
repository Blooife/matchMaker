using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Contexts.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.HasOne(u => u.Preference)
            .WithOne(p => p.Profile)
            .HasForeignKey<Preference>(p => p.ProfileId);

        builder.HasMany(p => p.Interests).WithMany(i => i.Profiles);
    }
}