using BookHaven.Entities;

public class Order
{
    public int OrderID { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public DateTime OrderDate { get; set; }
    public string PickupStatus { get; set; }
    public string ClaimCode { get; set; }
    public bool DiscountApplied { get; set; }
    public int DiscountPercent { get; set; }
    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}
