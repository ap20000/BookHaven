using System.Collections.Generic;
using System.Threading.Tasks;
using BookHaven.DTOs;

namespace BookHaven.Services.Interface
{
    public interface IAnnouncementService
    {
        Task<int> CreateAnnouncementAsync(AnnouncementRequestDto request);
        Task<List<AnnouncementResponseDto>> GetActiveAnnouncementsAsync();
        Task<bool> DeleteAnnouncementAsync(int announcementId);
    }
}
