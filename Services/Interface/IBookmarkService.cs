using BookHaven.Dtos;
using BookHaven.Entities;

namespace BookHaven.Services.Interface
{
    public interface IBookmarkService
    {
        // Adds a book to the user's bookmarks
        void AddBookmark(string userId, int bookId);
        void RemoveBookmark(string userId, int bookId);

        // Retrieves all bookmarked books for a user
        IEnumerable<BookmarkDto> GetBookmarks(string userId);
    }
}
