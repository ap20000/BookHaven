using BookHaven.Dtos;
using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookHaven.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GenreResponseDto>> GetAllAsync()
        {
            return await _context.Genres
                .Select(g => new GenreResponseDto
                {
                    GenreID = g.GenreID,
                    Name = g.Name
                })
                .ToListAsync();
        }

        public async Task<GenreResponseDto?> GetByIdAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            return genre == null ? null : new GenreResponseDto
            {
                GenreID = genre.GenreID,
                Name = genre.Name
            };
        }

        public async Task<GenreResponseDto> CreateAsync(GenreCreateDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return new GenreResponseDto
            {
                GenreID = genre.GenreID,
                Name = genre.Name
            };
        }

        public async Task<bool> UpdateAsync(int id, GenreCreateDto dto)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return false;

            genre.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return false;

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
