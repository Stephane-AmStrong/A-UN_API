using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models.ExtendedModels
{
    public class UniversityExtended : IEntity
    {
        public UniversityExtended()
        {

        }

        public UniversityExtended(University university)
        {
            Id = university.Id;
            Name = university.Name;
            ImgLink = university.ImgLink;
            Price = university.Price;
            AppUserId = university.AppUserId;
            Birthday = university.Birthday;
            CreateAt = university.CreateAt;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImgLink { get; set; }
        public long Price { get; set; }
        public string AppUserId { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreateAt { get; set; }

        

        public IEnumerable<Branch> Branches { get; set; }
    }
}
