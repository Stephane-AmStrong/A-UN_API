using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IImageRepository
    {
        public string PictureName { set; }

        public Task<string> UploadImage(IFormFile picture);

        public Task DeleteImage(string filePath);
    }
}
