namespace OrderProcessing.Domain.Models;

public class WorkOrder
{
    public Guid Id { get; set; }
    public string ExternalReference { get; set; } = null!;
    public required WorkOrderStatus CurrentStatus { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; }

    public ICollection<WorkOrderStateHistory> StateHistory { get; set; } = [];
}
