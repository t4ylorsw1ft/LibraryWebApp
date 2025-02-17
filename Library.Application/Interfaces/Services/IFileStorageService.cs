using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(byte[] fileData, string fileName);
        Task<byte[]> GetFileAsync(string filePath);
        Task DeleteFileAsync(string filePath);
    }
}
