namespace BookHaven.Dtos
{
    public class AddBookDto
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Language { get; set; }
        public string Format { get; set; }

        public int PublisherID { get; set; }
        public string ApplicationUserId { get; set; }

        public bool IsOnSale { get; set; } = false;
        public DateTime? SaleStartDate { get; set; }
        public DateTime? SaleEndDate { get; set; }
    }
}
