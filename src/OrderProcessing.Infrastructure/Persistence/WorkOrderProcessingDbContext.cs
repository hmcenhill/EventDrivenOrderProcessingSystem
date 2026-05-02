using Microsoft.EntityFrameworkCore;

using OrderProcessing.Domain.Models;

namespace OrderProcessing.Infrastructure.Persistence;

public class WorkOrderProcessingDbContext : DbContext
{
    public WorkOrderProcessingDbContext(DbContextOptions<WorkOrderProcessingDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<WorkOrderStateHistory> OrderStateHistory => Set<WorkOrderStateHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.ToTable("work_orders");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ExternalReference)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.CurrentStatus)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(x => x.CreatedUtc)
                .IsRequired();

            entity.Property(x => x.UpdatedUtc)
                .IsRequired();

            entity.HasIndex(x => x.ExternalReference)
                .IsUnique();
        });

        modelBuilder.Entity<WorkOrderStateHistory>(entity =>
        {
            entity.ToTable("work_order_state_history");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(x => x.ChangedUtc)
                .IsRequired();

            entity.Property(x => x.Notes)
                .HasMaxLength(500);

            entity.HasOne(x => x.WorkOrder)
                .WithMany(x => x.StateHistory)
                .HasForeignKey(x => x.WorkOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.WorkOrderId, x.ChangedUtc });
        });
    }
}