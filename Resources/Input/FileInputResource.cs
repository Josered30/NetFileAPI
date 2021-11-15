using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NetFileAPI.Enums;

namespace NetFileAPI.Resources.Input
{
    public class FileInputResource
    {
        public string Name { get; set; }
        public StorageType StorageType { get; set; }
        public IFormFile File { get; set; }
    }
}