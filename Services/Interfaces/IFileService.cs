using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetFileAPI.Enums;
using NetFileAPI.Models;
using NetFileAPI.Resources.Output;

namespace NetFileAPI.Services.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<FileOutputResource>> GetFilesAsync();
        Task<FileDataOutputResource> GetFileDataByIdAsync(int id);
        Task<FileOutputResource> GetFileByIdAsync(int id);
        Task<FileOutputResource> SaveAsync(IFormFile formFile, string name, StorageType storageType);
        Task<FileOutputResource> UpdateAsync(IFormFile formFile, int id);
        Task<FileOutputResource> DeleteAsync(int id);

        Task<FileOutputResource> DeleteAllAsync();
    }
}