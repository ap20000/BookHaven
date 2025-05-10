using BookHaven.Dtos;
using BookHaven.Entities;
using BookHaven.Services.Interface;

namespace BookHaven.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly ApplicationDbContext _context;

        public PublisherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddPublisher(AddPublisherDto addPublisherDto)
        {
            try
            {
                var publisher = new Publisher
                {
                 Name = addPublisherDto.Name,  
                };
                _context.Publishers.Add(publisher);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding publisher" + ex.Message);
            }

        }
    }
}
