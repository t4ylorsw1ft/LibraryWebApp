using Library.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileStorageService _fileStorageService;

        public FileService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<string> UploadImageAsync(byte[] fileData, string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            if (extension != ".jpg" && extension != ".png")
                throw new ArgumentException("Недопустимый формат изображения");
            Console.WriteLine("Service");

            return await _fileStorageService.SaveFileAsync(fileData, fileName);
        }

        public async Task<byte[]> GetImageAsync(string filePath)
        {
            return await _fileStorageService.GetFileAsync(filePath);
        }

        public async Task DeleteImageAsync(string filePath)
        {
            await _fileStorageService.DeleteFileAsync(filePath);
        }
    }
}
