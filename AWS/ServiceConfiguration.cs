namespace NetFileAPI.AWS
{
    public class ServiceConfiguration
    {
        public AwsS3Configuration AwsS3 { get; set; }
    }
    public class AwsS3Configuration
    {
        public string BucketName { get; set; }
    }
}