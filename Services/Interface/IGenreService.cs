using BookHaven.Dtos;

namespace BookHaven.Services.Interface
{
    public interface IGenreService
    {
        Task<List<GenreResponseDto>> GetAllAsync();
        Task<GenreResponseDto?> GetByIdAsync(int id);
        Task<GenreResponseDto> CreateAsync(GenreCreateDto dto);
        Task<bool> UpdateAsync(int id, GenreCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
