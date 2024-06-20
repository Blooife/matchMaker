using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Profile.Domain.Models;
using Profile.Infrastructure.Seed;

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
    public DbSet<Image> Images { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserProfile>()
            .HasMany(userProfile => userProfile.UserEducations)
            .WithOne(userEducation => userEducation.Profile)
            .HasForeignKey(userEducation => userEducation.ProfileId);

        modelBuilder.Entity<Education>()
            .HasMany(education => education.UserEducations)
            .WithOne(userEducation => userEducation.Education)
            .HasForeignKey(userEducation => userEducation.EducationId);

        modelBuilder.Entity<UserEducation>()
            .HasKey(userEducation => new { userEducation.ProfileId, userEducation.EducationId });
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyAllSeeds(Assembly.GetExecutingAssembly());
        
        
    }
}