using Authentication.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.DataLayer.Contexts.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Ignore(e => e.PhoneNumber);
        builder.Ignore(e => e.PhoneNumberConfirmed);
        builder.Ignore(e => e.TwoFactorEnabled);
        builder.Ignore(e => e.LockoutEnd);
        builder.Ignore(e => e.LockoutEnabled);
        builder.Ignore(e => e.PhoneNumber);
        builder.Property(e => e.Email).IsRequired();
        builder.Property(e => e.PasswordHash).IsRequired();
    }
}