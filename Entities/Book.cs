using BookHaven.Entities;

public class Book
{
    public int BookID { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Description { get; set; }
    public DateTime PublicationDate { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Language { get; set; }
    public string Format { get; set; }

    public int? PublisherID { get; set; }
    public Publisher? Publisher { get; set; }

    public int? AdminID { get; set; }
    public Admin? Admin { get; set; }

    public bool IsOnSale { get; set; }
    public DateTime? SaleStartDate { get; set; }
    public DateTime? SaleEndDate { get; set; }

    public ICollection<Review> Reviews { get; set; }
    public ICollection<Bookmark> Bookmarks { get; set; }
    public ICollection<BookAuthor> BookAuthors { get; set; }
    public ICollection<BookGenre> BookGenres { get; set; }
}
