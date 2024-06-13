using System.Reflection;
using Authentication.DataLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.DataLayer.Contexts;

public class AuthContext : IdentityDbContext<User, Role, string>
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options){ }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}