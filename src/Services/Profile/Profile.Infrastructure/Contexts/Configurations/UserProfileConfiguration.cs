using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Contexts.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(userProfile => userProfile.Id);
        builder.Property(userProfile => userProfile.Id).IsRequired().ValueGeneratedOnAdd();
        builder.HasIndex(userProfile => userProfile.UserId).IsUnique();
        
        builder.HasOne(userProfile => userProfile.Preference)
            .WithOne(preference => preference.Profile)
            .HasForeignKey<Preference>(preference => preference.ProfileId);

        builder.HasMany(p => p.Interests).WithMany(interest => interest.Profiles);
        builder.HasMany(p => p.Images).WithOne(i => i.Profile).HasForeignKey(i => i.ProfileId);
    }
}