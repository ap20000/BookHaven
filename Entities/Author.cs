public class Author
{
    public int AuthorID { get; set; }
    public string Name { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; }
}
