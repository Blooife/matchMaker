using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Domain.Models;

namespace Profile.Infrastructure.Contexts.Configurations;

public class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.HasKey(goal => goal.Id);
        builder.Property(goal => goal.Id).IsRequired().ValueGeneratedOnAdd();
        builder.HasIndex(goal => goal.Name).IsUnique();
    }
}