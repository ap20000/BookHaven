public class Genre
{
    public int GenreID { get; set; }
    public string Name { get; set; }

    public ICollection<BookGenre> BookGenres { get; set; }
}
