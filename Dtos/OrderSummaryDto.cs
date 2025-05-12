namespace BookHaven.Dtos
{
    public class OrderSummaryDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string PickupStatus { get; set; }
        public bool DiscountApplied { get; set; }
        public int DiscountPercent { get; set; }
        public string ClaimCode { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
    }

}
