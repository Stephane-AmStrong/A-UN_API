using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Extensions
{
    public static class UniversityExtensions
    {
        public static void Map(this University dbUniversity, University university)
        {
            dbUniversity.Id = university.Id;
            dbUniversity.Name = university.Name;
            dbUniversity.ImgLink = university.ImgLink;
            dbUniversity.Price = university.Price;
            dbUniversity.AppUserId = university.AppUserId;
            dbUniversity.Birthday = university.Birthday;
            dbUniversity.CreateAt = university.CreateAt;
        }
    }
}
