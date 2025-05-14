namespace BookHaven.Dtos
{
    public class GetAllBookDto
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
        public bool IsOnSale { get; set; }
        public DateTime? SaleStartDate { get; set; }
        public DateTime? SaleEndDate { get; set; }

        public int PublisherID { get; set; }

        public List<AuthorDto> Authors { get; set; }
        public List<GenreDto> Genres { get; set; }
    }

    public class AuthorDto
    {
        public int AuthorID { get; set; }
        public string Name { get; set; }
    }

    public class GenreDto
    {
        public int GenreID { get; set; }
        public string Name { get; set; }
    }
}
