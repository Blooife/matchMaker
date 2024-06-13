using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Contexts;

public class ProfileDbContext : DbContext
{
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }
    
    public DbSet<UserProfile> Profiles { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Preference> Preferences { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>()
            .HasMany(up => up.UserEducations)
            .WithOne(ue => ue.Profile)
            .HasForeignKey(ue => ue.ProfileId);

        modelBuilder.Entity<Education>()
            .HasMany(e => e.UserEducations)
            .WithOne(ue => ue.Education)
            .HasForeignKey(ue => ue.EducationId);

        modelBuilder.Entity<UserEducation>()
            .HasKey(ue => new { ue.ProfileId, ue.EducationId });
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
        
    }
}