namespace NetFileAPI.Resources.Output
{
    public abstract class BaseOutputResponse
    {
        protected BaseOutputResponse()
        {
            Success = true;
            Message = string.Empty;
        }

        protected BaseOutputResponse(string message)
        {
            Message = message;
            Success = true;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}