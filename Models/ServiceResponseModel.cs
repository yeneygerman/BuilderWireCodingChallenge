namespace BuilderWireCodingChallenge.Models
{
    public class ServiceResponseModel
    {
        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}