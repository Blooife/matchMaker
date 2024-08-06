using Authentication.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.DataLayer.Contexts.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Ignore(user => user.PhoneNumber);
        builder.Ignore(user => user.PhoneNumberConfirmed);
        builder.Ignore(user => user.TwoFactorEnabled);
        builder.Ignore(user => user.LockoutEnd);
        builder.Ignore(user => user.LockoutEnabled);
        builder.Ignore(user => user.PhoneNumber);
        builder.Property(user => user.Email).IsRequired();
        builder.Property(user => user.PasswordHash).IsRequired();
    }
}