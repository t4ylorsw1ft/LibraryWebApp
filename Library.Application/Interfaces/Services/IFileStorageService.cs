using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(byte[] fileData, string fileName, CancellationToken cancellationToken);
        Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken);
        bool DeleteFile(string filePath);
    }
}
