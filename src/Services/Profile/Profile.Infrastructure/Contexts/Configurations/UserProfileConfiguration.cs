using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Contexts.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.Property(userProfile => userProfile.Id).ValueGeneratedOnAdd();
        builder.HasMany(p => p.Interests).WithMany(interest => interest.Profiles);
        builder.HasMany(p => p.Images).WithOne(i => i.Profile).HasForeignKey(i => i.ProfileId);
    }
}