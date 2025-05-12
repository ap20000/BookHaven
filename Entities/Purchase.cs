using System;
using BookHaven.Entities;

public class Purchase
{
    public int PurchaseID { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public int BookID { get; set; }
    public Book Book { get; set; }

    public DateTime PurchaseDate { get; set; }
}
