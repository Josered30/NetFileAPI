using System.IO;
using NetFileAPI.Enums;

namespace NetFileAPI.Resources.Output
{
    public class FileOutputResource : BaseOutputResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StorageType StorageType { get; set; }
        public string Path { get; set; }
    }
}