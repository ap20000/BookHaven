using BookHaven.Dtos;

namespace BookHaven.Services.Interface
{
    public interface IAuthorService
    {
        Task<List<AuthorResponseDto>> GetAllAsync();
        Task<AuthorResponseDto?> GetByIdAsync(int id);
        Task<AuthorResponseDto> CreateAsync(AuthorCreateDto dto);
        Task<bool> UpdateAsync(int id, AuthorCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
