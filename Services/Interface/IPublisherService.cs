using BookHaven.Dtos;
using System.Collections.Generic;

namespace BookHaven.Services.Interface
{
    public interface IPublisherService
    {
        void AddPublisher(AddPublisherDto addPublisherDto);
        IEnumerable<PublisherDto> GetAllPublishers();
        bool DeletePublisher(int id);

    }
}
