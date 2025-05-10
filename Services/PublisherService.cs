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

        public IEnumerable<PublisherDto> GetAllPublishers()
        {
            return _context.Publishers
                .Select(p => new PublisherDto
                {
                    PublisherID = p.PublisherID,
                    Name = p.Name
                })
                .ToList();
        }

        public bool DeletePublisher(int id)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher == null) return false;

            _context.Publishers.Remove(publisher);
            _context.SaveChanges();
            return true;
        }
    }
}
