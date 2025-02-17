using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(byte[] fileData, string fileName);
        Task<byte[]> GetImageAsync(string filePath);
        Task DeleteImageAsync(string filePath);
    }
}
