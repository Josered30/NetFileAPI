using AutoMapper;
using NetFileAPI.AWS;
using NetFileAPI.Enums;
using NetFileAPI.Models;
using NetFileAPI.Repositories.Interfaces;
using NetFileAPI.Resources.Output;
using NetFileAPI.Services.Interfaces;

namespace NetFileAPI.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IAwsS3BucketHelper _awsS3BucketHelper;
        private readonly IMapper _mapper;

        public FileService(IFileRepository fileRepository, IUnitOfWork unitOfWork, IConfiguration configuration,
            IMapper mapper,
            IAwsS3BucketHelper awsS3BucketHelper)
        {
            this._fileRepository = fileRepository;
            this._unitOfWork = unitOfWork;
            this._configuration = configuration;
            this._awsS3BucketHelper = awsS3BucketHelper;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<FileOutputResource>> GetFilesAsync()
        {
            var files = await _fileRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FileModel>, IEnumerable<FileOutputResource>>(files);
        }

        public async Task<FileOutputResource> GetFileByIdAsync(int id)
        {
            var file = await _fileRepository.GetByIdAsync(id);
            return _mapper.Map<FileOutputResource>(file);
        }

        public async Task<FileDataOutputResource> GetFileDataByIdAsync(int id)
        {
            var file = await _fileRepository.GetByIdAsync(id);
            FileDataOutputResource fileOutputResource = new FileDataOutputResource();

            if (file.StorageType == StorageType.Cloud)
            {
                try
                {
                    var stream = await _awsS3BucketHelper.GetFile(file.Path);
                    if (stream != null)
                    {
                        fileOutputResource.Stream = stream;
                        fileOutputResource.MimeType = file.MimeType;
                        return fileOutputResource;
                    }

                    fileOutputResource.Success = false;
                    fileOutputResource.Message = "File not found";
                    return fileOutputResource;
                }
                catch (Exception e)
                {
                    fileOutputResource.Success = false;
                    fileOutputResource.Message = "Internal error, not found";
                    return fileOutputResource;
                }
            }
            string path = Path.Combine(_configuration["StoredFilesPath"], file.Path);
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            fileOutputResource.Stream = fileStream;
            fileOutputResource.MimeType = file.MimeType;

            return fileOutputResource;
        }


        public async Task<FileOutputResource> SaveAsync(IFormFile formFile, string name, StorageType storageType)
        {

            String mimeType = Path.GetExtension(formFile.FileName);
            ///String mimeType = formFile.Name.Split("/")[1];
            String path = $"{DateTime.Now.Ticks}{mimeType}";

            if (storageType == StorageType.Cloud)
            {
                try
                {
                    using var ms = new MemoryStream();
                    await formFile.CopyToAsync(ms);
                    await _awsS3BucketHelper.UploadFile(ms, path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new FileOutputResource()
                    {
                        Success = false,
                        Message = "Upload error"
                    };
                }
            }
            else
            {
                using var stream = File.Create(Path.Combine(_configuration["StoredFilesPath"], path));
                await formFile.CopyToAsync(stream);
            }

            FileModel fileModel = new FileModel(path, name, formFile.ContentType, storageType);
            await _fileRepository.AddAsync(fileModel);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<FileOutputResource>(fileModel);
        }

        public async Task<FileOutputResource> UpdateAsync(IFormFile formFile, int id)
        {
            var file = await _fileRepository.GetByIdAsync(id);

            if (file.StorageType == StorageType.Cloud)
            {
                try
                {
                    using var ms = new MemoryStream();
                    await formFile.CopyToAsync(ms);
                    await _awsS3BucketHelper.UploadFile(ms, file.Path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new FileOutputResource()
                    {
                        Success = false,
                        Message = "Upload error"
                    };
                }
            }
            else
            {
                using var stream = File.Create(Path.Combine(_configuration["StoredFilesPath"], file.Path));
                await formFile.CopyToAsync(stream);
                stream.Close();
            }

            _fileRepository.Modify(file);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<FileOutputResource>(file);
        }

        public async Task<FileOutputResource> DeleteAsync(int id)
        {
            var file = await _fileRepository.GetByIdAsync(id);
            if (file.StorageType == StorageType.Cloud)
            {
                try
                {
                    await _awsS3BucketHelper.DeleteFile(file.Path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new FileOutputResource()
                    {
                        Success = false,
                        Message = "Delete error"
                    };
                }
            }
            else
            {
                File.Delete(Path.Combine(_configuration["StoredFilesPath"], file.Path));
            }

            await _fileRepository.DeleteByIdAsync(id);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<FileOutputResource>(file);
        }


        private async Task DeleteAsyncNoSave(int id)
        {
            var file = await _fileRepository.GetByIdAsync(id);
            if (file.StorageType == StorageType.Cloud)
            {
                try
                {
                    await _awsS3BucketHelper.DeleteFile(file.Path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                File.Delete(Path.Combine(_configuration["StoredFilesPath"], file.Path));
            }

            await _fileRepository.DeleteByIdAsync(id);
            return;
        }



        public async Task<FileOutputResource> DeleteAllAsync()
        {
            var list = await GetFilesAsync();
            List<Task> tasks = new List<Task>();

            try
            {
                foreach (var item in list)
                {
                    tasks.Add(Task.Run(() => DeleteAsyncNoSave(item.Id)));
                }
                await Task.WhenAll(tasks);
                await _unitOfWork.CompleteAsync();
                
                return new FileOutputResource()
                {
                    Message = "Deleted",
                    Success = true
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new FileOutputResource()
            {
                Message = "Error",
                Success = false
            };

        }


    }
}