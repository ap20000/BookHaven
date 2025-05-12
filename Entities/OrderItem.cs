public class OrderItem
{
    public int OrderItemID { get; set; }
    public int OrderID { get; set; }
    public Order Order { get; set; }
    public int BookID { get; set; }
    public Book Book { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
}
