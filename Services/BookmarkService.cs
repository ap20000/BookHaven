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

        public IEnumerable<Bookmark> GetBookmarks(string userId)
        {
            return _context.Bookmarks.Include(b => b.Book)
                                      .Where(b => b.ApplicationUserId == userId)
                                      .ToList();
        }
    }
}
