namespace BookHaven.DTOs
{
    public class OrderItemDto
    {
        public int BookID { get; set; }
        public int Quantity { get; set; }
    }

    public class PlaceOrderRequest
    {
        public List<OrderItemDto> Items { get; set; }
        public bool ApplyDiscount { get; set; }
    }
}
