public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } // "Member", "Staff", "Admin"
    public string MembershipId { get; set; }
    public int SuccessfulOrderCount { get; set; }

    public ICollection<Order> Orders { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    public ICollection<Whitelist> Whitelist { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
