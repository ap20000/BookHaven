using BookHaven.DTOs;
using BookHaven.Entities;
using BookHaven.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookHaven.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAnnouncementAsync(AnnouncementRequestDto request)
        {
            var announcement = new Announcement
            {
                Title = request.Title,
                Message = request.Message,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ApplicationUserId = request.ApplicationUserId
            };

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return announcement.AnnouncementID;
        }

        public async Task<List<AnnouncementResponseDto>> GetActiveAnnouncementsAsync()
        {
            var activeAnnouncements = await _context.Announcements
                .ToListAsync();

            return activeAnnouncements.Select(a => new AnnouncementResponseDto
            {
                AnnouncementID = a.AnnouncementID,
                Title = a.Title,
                Message = a.Message,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                ApplicationUserId = a.ApplicationUserId 
            }).ToList();
        }

        public async Task<bool> DeleteAnnouncementAsync(int announcementId)
        {
            var announcement = await _context.Announcements.FindAsync(announcementId);
            if (announcement == null) return false;

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
