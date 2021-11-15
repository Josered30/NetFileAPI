using System.IO;

namespace NetFileAPI.Resources.Output
{
    public class FileDataOutputResource: BaseOutputResponse
    {
        public Stream Stream { get; set; }
        public string MimeType { get; set; }
        
    }
}