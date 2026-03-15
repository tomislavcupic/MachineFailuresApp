using MachineFailures.Domain;
using Microsoft.EntityFrameworkCore;

namespace MachineFailures.Infrastructure;

public class MachineFailureDbContext : DbContext
{
    public MachineFailureDbContext(DbContextOptions<MachineFailureDbContext> options) : base(options) {}
    public DbSet<Machine> Machines { get; set; }
    public DbSet<Failure> Failures { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
            entity.Property(m => m.CategoryId).IsRequired();

            entity.HasIndex(m => m.Name).IsUnique();

            entity.HasOne<Category>()
                .WithMany()
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Failure>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.MachineId).IsRequired();
            entity.Property(f => f.Name).IsRequired().HasMaxLength(50);
            entity.Property(f => f.Description).IsRequired();
            entity.Property(f => f.StartOfFailure).IsRequired();
            entity.Property(f => f.EndOfFailure).IsRequired(false);

            entity.HasOne<Machine>()
                .WithMany()
                .HasForeignKey(f => f.MachineId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}