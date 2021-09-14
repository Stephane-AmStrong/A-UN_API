using Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string baselocation = "/pictures/";
        private string fullFilePath;
        private string relativeFilePath;

        public string Location
        {
            get { return fullFilePath; }
            //set { folderName = value; }
        }

        private string fileName;
        public string PictureName { set => fileName = value; }



        public ImageRepository(IWebHostEnvironment webHostEnvironment, string folderName)
        {
            _webHostEnvironment = webHostEnvironment;
            this.fullFilePath = _webHostEnvironment.WebRootPath + baselocation + folderName;

            relativeFilePath = baselocation + folderName;
        }



        public async Task DeleteImage(string filePath)
        {
            try
            {
                await Task.Run(()=> File.Delete(filePath));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UploadImage(IFormFile picture)
        {
            try
            {
                if (!System.IO.Directory.Exists(fullFilePath)) Directory.CreateDirectory(fullFilePath);


                if (picture.Length > 0)
                {
                    string extention = picture.FileName.Split('.').Last();

                    var fullPath = $"{this.fullFilePath}/{fileName}.{extention}";
                    var relativePath = $"{this.relativeFilePath}/{fileName}.{extention}";
                    using (var stream = File.Create(fullPath))
                    {
                        await picture.CopyToAsync(stream);
                        await stream.FlushAsync();
                        return relativePath;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}
