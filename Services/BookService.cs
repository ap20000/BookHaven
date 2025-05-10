using BookHaven.Dtos;
using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookHaven.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddBook(AddBookDto addBookDto)
        {
            try
            {
                var book = new Book
                {
                    Title = addBookDto.Title,
                    ISBN = addBookDto.ISBN,
                    Description = addBookDto.Description,
                    PublicationDate = addBookDto.PublicationDate,
                    Price = addBookDto.Price,
                    Stock = addBookDto.Stock,
                    Language = addBookDto.Language,
                    Format = addBookDto.Format,
                    PublisherID = addBookDto.PublisherID,
                    ApplicationUserId = addBookDto.ApplicationUserId,
                    IsOnSale = addBookDto.IsOnSale,
                    SaleStartDate = addBookDto.SaleStartDate,
                    SaleEndDate = addBookDto.SaleEndDate,

                };
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding book" + ex.Message);
            }

        }

        public List<GetAllBookDto> GetAllBooks()
        {
            try
            {

                var book = _context.Books;
                var result = new List<GetAllBookDto>();
                foreach (var b in book)
                {
                    result.Add(new GetAllBookDto
                    {
                        BookID = b.BookID,
                        Title = b.Title,
                        ISBN = b.ISBN,
                        Description = b.Description,
                        PublicationDate = b.PublicationDate,
                        Price = b.Price,
                        Stock = b.Stock,
                        Language = b.Language,
                        Format = b.Format,
                       

                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all users" + ex.Message);
            }
        }

        public GetAllBookDto GetById(int id)
        {
            try
            {
                var book = _context.Books.FirstOrDefault(u => u.BookID == id);
                if (book == null)
                    throw new Exception("Book not found");

                var result = new GetAllBookDto()
                {
                    BookID = book.BookID,
                    Title = book.Title,
                    ISBN = book.ISBN,
                    Description = book.Description,
                    PublicationDate = book.PublicationDate,
                    Price = book.Price,
                    Stock = book.Stock,
                    Language = book.Language,
                    Format = book.Format,
                    IsOnSale = book.IsOnSale,
                    SaleStartDate = book.SaleStartDate,
                    SaleEndDate = book.SaleEndDate
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get book by id" + ex.Message);
            }
        }

        public void DeleteBook(int id)
        {
            try
            {
                var book = _context.Books.FirstOrDefault(u => u.BookID == id);
                if (book == null)
                    throw new Exception("Book not found");

                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting book:" + ex.Message);
            }
        }

        public void UpdateBook(int id, UpdateBookDto bookDto)
        {
            try
            {
                var book = _context.Books.FirstOrDefault(u => u.BookID == id);
                if (book == null)
                    throw new Exception("Book not found");

                book.Title = bookDto.Title;
                book.ISBN = bookDto.ISBN;
                book.Description = bookDto.Description;
                book.PublicationDate = bookDto.PublicationDate;
                book.Price = bookDto.Price;
                book.Stock = bookDto.Stock;
                book.Language = bookDto.Language;
                book.Format = bookDto.Format;
                book.PublisherID = bookDto.PublisherID;
                book.IsOnSale = bookDto.IsOnSale ?? book.IsOnSale;
                book.SaleStartDate = bookDto.SaleStartDate;
                book.SaleEndDate = bookDto.SaleEndDate;


                _context.Books.Update(book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update book by id" + ex.Message);

            }
        }
        public PaginatedResult<GetAllBookDto> BrowseCatalog(string? search, string? sortBy, int pageNumber, int pageSize)
        {
            var query = _context.Books.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Description.Contains(search));
            }

            // Sort
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "price_asc" => query.OrderBy(b => b.Price),
                    "price_desc" => query.OrderByDescending(b => b.Price),
                    "title" => query.OrderBy(b => b.Title),
                    "date" => query.OrderByDescending(b => b.PublicationDate),
                    _ => query
                };
            }

            var totalItems = query.Count();

            var books = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new GetAllBookDto
                {
                    BookID = b.BookID,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    Description = b.Description,
                    PublicationDate = b.PublicationDate,
                    Price = b.Price,
                    Stock = b.Stock,
                    Language = b.Language,
                    Format = b.Format,
                     IsOnSale = b.IsOnSale,
                    SaleStartDate = b.SaleStartDate,
                    SaleEndDate = b.SaleEndDate
                })
                .ToList();

            return new PaginatedResult<GetAllBookDto>
            {
                Items = books,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalItems
            };
        }

        public IQueryable<Book> FilterBooksByCategory(string category)
        {
            var now = DateTime.UtcNow;
            var threeMonthsAgo = now.AddMonths(-3);

            var query = _context.Books.AsQueryable();

            switch (category.ToLower())
            {
                case "all":
                    break;

                case "new releases":
                    query = query.Where(b =>
                        b.PublicationDate >= threeMonthsAgo &&
                        b.PublicationDate <= now);
                    break;

                case "coming soon":
                    query = query.Where(b => b.PublicationDate > now);
                    break;

                case "deals":
                    query = query.Where(b =>
                        b.IsOnSale &&
                        b.SaleStartDate <= now &&
                        (b.SaleEndDate == null || b.SaleEndDate >= now));
                    break;

                default:
                    // Return no results for unsupported categories
                    query = query.Where(b => false);
                    break;
            }

            return query;
        }

        public PaginatedResult<GetAllBookDto> GetBooksByCategory(string category, int pageNumber, int pageSize)
        {
            var query = FilterBooksByCategory(category);

            var totalItems = query.Count();

            var books = query
                .OrderByDescending(b => b.PublicationDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new GetAllBookDto
                {
                    BookID = b.BookID,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    Description = b.Description,
                    PublicationDate = b.PublicationDate,
                    Price = b.Price,
                    Stock = b.Stock,
                    Language = b.Language,
                    Format = b.Format,
                    IsOnSale = b.IsOnSale,
                    SaleStartDate = b.SaleStartDate,
                    SaleEndDate = b.SaleEndDate

                })
                .ToList();

            return new PaginatedResult<GetAllBookDto>
            {
                Items = books,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalItems
            };
        }
    }
}
