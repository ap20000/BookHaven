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
    }
}
