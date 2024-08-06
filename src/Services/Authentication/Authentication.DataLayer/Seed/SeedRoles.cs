using Authentication.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;

namespace Authentication.DataLayer.Seed;

public static class SeedRoles
{
    public static void Seed(ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(
            new Role
            {
                Id = "335250d4-8cfb-4e24-930f-14b84746fa82",
                Name = Roles.Admin,
                NormalizedName = Roles.Admin.ToUpper(),
            },
            new Role
            {
                Id = "451d9c74-c18f-454d-b64c-b192aec43282",
                Name = Roles.User,
                NormalizedName = Roles.User.ToUpper()
            },
            new Role
            {
                Id = "58122fe2-c131-4ed7-b887-5af4bafdf92a",
                Name = Roles.Moderator,
                NormalizedName = Roles.Moderator.ToUpper()
            }
        );
    }
}