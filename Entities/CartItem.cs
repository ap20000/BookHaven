namespace BookHaven.Entities
{
    public class CartItem
    {
        public int CartItemID { get; set; }

        public int BookID { get; set; }
        public Book Book { get; set; }

        public int MemberID { get; set; }  // or UserID if you're not separating Member/Admin
        public Member Member { get; set; }

        public int Quantity { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
