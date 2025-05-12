using BookHaven.Dtos;
using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.EntityFrameworkCore;


namespace BookHaven.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuthorResponseDto>> GetAllAsync()
        {
            return await _context.Authors
                .Select(a => new AuthorResponseDto
                {
                    AuthorID = a.AuthorID,
                    Name = a.Name
                })
                .ToListAsync();
        }

        public async Task<AuthorResponseDto?> GetByIdAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return author == null ? null : new AuthorResponseDto
            {
                AuthorID = author.AuthorID,
                Name = author.Name
            };
        }

        public async Task<AuthorResponseDto> CreateAsync(AuthorCreateDto dto)
        {
            var author = new Author { Name = dto.Name };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return new AuthorResponseDto
            {
                AuthorID = author.AuthorID,
                Name = author.Name
            };
        }

        public async Task<bool> UpdateAsync(int id, AuthorCreateDto dto)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            author.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
