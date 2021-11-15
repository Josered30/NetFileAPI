using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace NetFileAPI.AWS
{
    public interface IAwsS3BucketHelper
    {
        Task<bool> UploadFile(Stream inputStream, string fileName);
        Task<ListVersionsResponse> FilesList();
        Task<Stream?> GetFile(string key);
        Task<bool> DeleteFile(string key);
    }

    public class AwsS3BucketHelper : IAwsS3BucketHelper
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly ServiceConfiguration _serviceConfiguration;

        public AwsS3BucketHelper(IAmazonS3 amazonS3, IOptions<ServiceConfiguration> serviceConfiguration)
        {
            this._amazonS3 = amazonS3;
            this._serviceConfiguration = serviceConfiguration.Value;
        }

        public async Task<bool> UploadFile(Stream inputStream, string fileName)
        {

            Console.WriteLine(_amazonS3.Config.RegionEndpoint);

            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = inputStream,
                    BucketName = _serviceConfiguration.AwsS3.BucketName,
                    Key = fileName
                };

                PutObjectResponse putObjectResponse = await _amazonS3.PutObjectAsync(request);
                return putObjectResponse.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ListVersionsResponse> FilesList()
        {
            return await _amazonS3.ListVersionsAsync(_serviceConfiguration.AwsS3.BucketName);
        }

        public async Task<Stream?> GetFile(string key)
        {
            GetObjectResponse getObjectResponse =
                await _amazonS3.GetObjectAsync(_serviceConfiguration.AwsS3.BucketName, key);
            return getObjectResponse.HttpStatusCode == System.Net.HttpStatusCode.OK
                ? getObjectResponse.ResponseStream
                : null;
        }

        public async Task<bool> DeleteFile(string key)
        {
            try
            {
                DeleteObjectResponse deleteObjectResponse =
                    await _amazonS3.DeleteObjectAsync(_serviceConfiguration.AwsS3.BucketName, key);
                return deleteObjectResponse.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}