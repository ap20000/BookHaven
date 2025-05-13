using BookHaven.Dtos;
using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookHaven.Services
{
    public class FilterService : IFilterService
    {
        private readonly ApplicationDbContext _context;

        public FilterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllBookDto>> GetFilteredBooksAsync(BookFilterDto filter)
        {
            var query = _context.Books
                .Include(b => b.BookAuthors)
                .Include(b => b.BookGenres)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(b => b.Title.Contains(filter.Title));

            if (!string.IsNullOrWhiteSpace(filter.ISBN))
                query = query.Where(b => b.ISBN.Contains(filter.ISBN));

            if (!string.IsNullOrWhiteSpace(filter.Description))
                query = query.Where(b => b.Description.Contains(filter.Description));

            if (!string.IsNullOrWhiteSpace(filter.Language))
                query = query.Where(b => b.Language == filter.Language);

            if (filter.AuthorIDs?.Any() == true)
                query = query.Where(b => b.BookAuthors.Any(a => filter.AuthorIDs.Contains(a.AuthorID)));

            if (filter.GenreIDs?.Any() == true)
                query = query.Where(b => b.BookGenres.Any(g => filter.GenreIDs.Contains(g.GenreID)));

            // Sorting
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                bool isDesc = filter.SortDirection?.ToLower() == "desc";
                query = filter.SortBy.ToLower() switch
                {
                    "title" => isDesc ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
                    "price" => isDesc ? query.OrderByDescending(b => b.Price) : query.OrderBy(b => b.Price),
                    "date" => isDesc ? query.OrderByDescending(b => b.PublicationDate) : query.OrderBy(b => b.PublicationDate),
                    _ => query
                };
            }

            var result = await query.Select(b => new GetAllBookDto
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
                SaleEndDate = b.SaleEndDate,
                PublisherID = b.PublisherID,
                Authors = b.BookAuthors.Select(a => new AuthorDto
                {
                    AuthorID = a.AuthorID,
                }).ToList(),
                Genres = b.BookGenres.Select(g => new GenreDto
                {
                    GenreID = g.GenreID,
                }).ToList()
            }).ToListAsync();

            return result;
        }
    }
}

