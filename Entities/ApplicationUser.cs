using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BookHaven.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string MembershipId { get; set; }
        public string Role { get; set; } // Admin or Member
        public int SuccessfulOrderCount { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
