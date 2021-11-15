using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetFileAPI.Enums;

namespace NetFileAPI.Models
{
    [Table("file_models")]
    public class FileModel
    {
        [Column("id")] public int Id { get; set; }
        [Column("path")] public string Path { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("mime_type")] public string MimeType { get; set; }
        [Column("storage_type")] public StorageType StorageType { get; set; }

        public FileModel(string url, string name, string mimeType, StorageType storageType)
        {
            this.Path = url;
            this.Name = name;
            this.StorageType = storageType;
            this.MimeType = mimeType;
        }

        public FileModel()
        {
            this.Path = "";
            this.Name = "";
            this.StorageType = StorageType.Local;
            this.Id = 0;
            this.MimeType = "";
        }
    }
}