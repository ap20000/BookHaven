using BookHaven.Dtos;
using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookHaven.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly ApplicationDbContext _context;

        public BookmarkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddBookmark(string userId, int bookId)
        {
            var existingBookmark = _context.Bookmarks
                .FirstOrDefault(b => b.ApplicationUserId == userId && b.BookID == bookId);

            if (existingBookmark != null)
                throw new Exception("Book already bookmarked.");

            var bookmark = new Bookmark
            {
                ApplicationUserId = userId,
                BookID = bookId
            };

            _context.Bookmarks.Add(bookmark);
            _context.SaveChanges();
        }

      

        public IEnumerable<BookmarkDto> GetBookmarks(string userId)
        {
            return _context.Bookmarks
                .Where(b => b.ApplicationUserId == userId)
                .Include(b => b.Book)
                .Select(b => new BookmarkDto
                {
                    BookmarkID = b.BookmarkID,
                    BookID = b.BookID,
                    BookTitle = b.Book.Title,
                    Description = b.Book.Description
                })
                .ToList();
        }


        public void RemoveBookmark(string userId, int bookId)
        {
            var bookmark = _context.Bookmarks
                .FirstOrDefault(b => b.ApplicationUserId == userId && b.BookID == bookId);

            if (bookmark == null)
                throw new Exception("Bookmark not found.");

            _context.Bookmarks.Remove(bookmark);
            _context.SaveChanges();
        }


    }
}
