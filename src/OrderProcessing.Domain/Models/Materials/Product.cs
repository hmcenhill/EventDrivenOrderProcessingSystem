namespace OrderProcessing.Domain.Models.Materials;

public class Product
{
    public string ItemId { get; }
    public string ItemName { get; }

    public Product(string ItemId, string ItemName)
    {
        this.ItemId = ItemId;
        this.ItemName = ItemName;
    }

}