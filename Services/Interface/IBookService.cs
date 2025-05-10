using BookHaven.Dtos;
using BookHaven.Entities;

namespace BookHaven.Services.Interface
{
    public interface IBookService
    {
        void AddBook(AddBookDto addBookDto);
        List<GetAllBookDto> GetAllBooks();

        GetAllBookDto GetById(int id);

        void DeleteBook(int id);

        void UpdateBook(int id, UpdateBookDto bookDto);

        PaginatedResult<GetAllBookDto> BrowseCatalog(string? search, string? sortBy, int pageNumber, int pageSize);




    }
}
