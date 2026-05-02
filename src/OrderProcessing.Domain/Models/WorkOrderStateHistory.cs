namespace OrderProcessing.Domain.Models;

public class WorkOrderStateHistory
{
    public Guid Id { get; set; }
    public Guid WorkOrderId { get; set; }
    public required WorkOrderStatus Status { get; set; }
    public DateTime ChangedUtc { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
    public required string CompletedBy { get; set; }

    public required WorkOrder WorkOrder { get; set; }
}