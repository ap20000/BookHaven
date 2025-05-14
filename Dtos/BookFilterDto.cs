namespace BookHaven.Dtos
{
    public class BookFilterDto
    {
        public string Title { get; set; }
        public string? ISBN { get; set; }
        public string? Description { get; set; }
        public string? Language { get; set; }
        public List<int>? AuthorIDs { get; set; }
        public List<int>? GenreIDs { get; set; }

        public string? SortBy { get; set; } 
        public string? SortDirection { get; set; }
    }
}
