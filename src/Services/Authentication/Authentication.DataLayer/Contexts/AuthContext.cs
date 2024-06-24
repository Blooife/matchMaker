using System.Reflection;
using Authentication.DataLayer.Models;
using Authentication.DataLayer.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DataLayer.Contexts;

public class AuthContext : IdentityDbContext<User, Role, string>
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options){ }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
        
        SeedRoles.Seed(modelBuilder);
        SeedUsers.Seed(modelBuilder);
    }
}