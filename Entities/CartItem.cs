using BookHaven.Entities;

public class CartItem
{
    public int CartItemID { get; set; }

    public int BookID { get; set; }
    public Book Book { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
