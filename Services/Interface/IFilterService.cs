using BookHaven.Dtos;

namespace BookHaven.Services.Interface
{
    public interface IFilterService
    {
        Task<IEnumerable<GetAllBookDto>> GetFilteredBooksAsync(BookFilterDto filterDto);

    }
}
