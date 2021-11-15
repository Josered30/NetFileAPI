using AutoMapper;
using NetFileAPI.Models;
using NetFileAPI.Resources.Input;
using NetFileAPI.Resources.Output;

namespace NetFileAPI.Mapper
{
    public class FileProfile: Profile
    {
        public FileProfile()
        {
            CreateMap<FileInputResource, FileModel>();
            CreateMap<FileModel, FileOutputResource>();
        }
        
    }
}