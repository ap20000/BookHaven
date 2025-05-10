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
                        Format = b.Format
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
                var book = _context.Books
                    .Include(b => b.Publisher)
                    .FirstOrDefault(b => b.BookID == id);


                if (book == null)
                    return null;

                var result = new GetAllBookDto
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
                    PublisherID = book.PublisherID,
                    PublisherName = book.Publisher?.Name ?? "Unknown"
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get book by id: " + ex.Message);
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
                    Format = b.Format
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

        public class BookmarkService
        {
            private readonly ApplicationDbContext _context;

            public BookmarkService(ApplicationDbContext context)
            {
                _context = context;
            }

            public void AddBookmark(string userId, int bookId)
            {
                var exists = _context.Bookmarks.Any(b => b.ApplicationUserId == userId && b.BookID == bookId);
                if (!exists)
                {
                    var bookmark = new Bookmark
                    {
                        BookID = bookId,
                        ApplicationUserId = userId
                    };
                    _context.Bookmarks.Add(bookmark);
                    _context.SaveChanges();
                }
            }

            public List<Bookmark> GetBookmarks(string userId)
            {
                return _context.Bookmarks
                    .Where(b => b.ApplicationUserId == userId)
                    .Include(b => b.Book)
                    .ToList();
            }

            public void RemoveBookmark(string userId, int bookId)
            {
                var bookmark = _context.Bookmarks
                    .FirstOrDefault(b => b.ApplicationUserId == userId && b.BookID == bookId);
                if (bookmark != null)
                {
                    _context.Bookmarks.Remove(bookmark);
                    _context.SaveChanges();
                }
            }
        }

        public class CartService
        {
            private readonly ApplicationDbContext _context;

            public CartService(ApplicationDbContext context)
            {
                _context = context;
            }

            public void AddToCart(string userId, int bookId, int quantity)
            {
                var item = _context.CartItems
                    .FirstOrDefault(c => c.ApplicationUserId == userId && c.BookID == bookId);

                if (item != null)
                {
                    item.Quantity += quantity;
                }
                else
                {
                    item = new CartItem
                    {
                        BookID = bookId,
                        ApplicationUserId = userId,
                        Quantity = quantity
                    };
                    _context.CartItems.Add(item);
                }

                _context.SaveChanges();
            }

            public List<CartItem> GetCart(string userId)
            {
                return _context.CartItems
                    .Where(c => c.ApplicationUserId == userId)
                    .Include(c => c.Book)
                    .ToList();
            }

            public void RemoveFromCart(string userId, int bookId)
            {
                var item = _context.CartItems
                    .FirstOrDefault(c => c.ApplicationUserId == userId && c.BookID == bookId);

                if (item != null)
                {
                    _context.CartItems.Remove(item);
                    _context.SaveChanges();
                }
            }
        }

    }
}
