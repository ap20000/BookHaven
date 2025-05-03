using BookHaven.Entities;

public class User
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // "Member", "Staff", "Admin"
    public string MembershipId { get; set; }
    public int SuccessfulOrderCount { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
