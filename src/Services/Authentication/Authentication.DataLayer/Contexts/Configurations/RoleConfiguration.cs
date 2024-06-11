using Authentication.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Authentication.DataLayer.Contexts.Configurations;

public class RoleConfiguration: IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.HasData(
            new Role
            {
                Id = "aghvdhad",
                Name = Roles.Admin,
                NormalizedName = Roles.Admin.ToUpper(),
            },
            new Role
            {
                Id = "njnknsk",
                Name = Roles.User,
                NormalizedName = Roles.User.ToUpper()
            },
            new Role
            {
                Id = "wyuewb",
                Name = Roles.Moderator,
                NormalizedName = Roles.Moderator.ToUpper()
            }
        );
    }
}