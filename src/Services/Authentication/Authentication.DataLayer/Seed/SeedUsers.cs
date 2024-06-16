using Authentication.DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DataLayer.Seed;

public static class SeedUsers
{
    public static void Seed(ModelBuilder builder)
    {
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

        var user1 = new User()
        {
            Id = "b2103436-0d48-4b80-8a35-2551a2b47e5b",
            Email = "admin@gmail.com",
        };
        var user2 = new User()
        {
            Id = "cf7ddbd5-3717-489a-84ab-8c4df32780a1",
            Email = "moderator@gmail.com",
        };
        var user3 = new User()
        {
            Id = "7fb23b7f-1dbe-469d-b3c2-51bd1dc24048",
            Email = "user@gmail.com",
        };

        user1.PasswordHash = passwordHasher.HashPassword(user1, "adminPassword");
        user2.PasswordHash = passwordHasher.HashPassword(user2, "moderatorPassword");
        user3.PasswordHash = passwordHasher.HashPassword(user3, "userPassword");

        builder.Entity<User>().HasData(user1, user2, user3);

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>()
            {
                RoleId = "335250d4-8cfb-4e24-930f-14b84746fa82",
                UserId = "b2103436-0d48-4b80-8a35-2551a2b47e5b",
            },
            new IdentityUserRole<string>()
            {
                RoleId = "58122fe2-c131-4ed7-b887-5af4bafdf92a",
                UserId = "cf7ddbd5-3717-489a-84ab-8c4df32780a1",
            },
            new IdentityUserRole<string>()
            {
                RoleId = "451d9c74-c18f-454d-b64c-b192aec43282",
                UserId = "7fb23b7f-1dbe-469d-b3c2-51bd1dc24048",
            }
        );
    }
}