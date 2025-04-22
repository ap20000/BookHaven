public class Order
{
    public int OrderID { get; set; }
    public int MemberID { get; set; }
    public Member Member { get; set; }

    public DateTime OrderDate { get; set; }
    public string PickupStatus { get; set; } // e.g., Pending, Claimed
    public string ClaimCode { get; set; }
    public bool DiscountApplied { get; set; }
    public int DiscountPercent { get; set; }
    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
}
