using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Contexts.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(userProfile => userProfile.Profile).WithOne(user => user.User)
            .HasForeignKey<UserProfile>(userProfile => userProfile.UserId);
    }
}