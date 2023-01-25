using BuilderWireCodingChallenge.Models;

namespace BuilderWireCodingChallenge.Services
{
    public class CommonResponseService
    {
        public ServiceResponseModel Response(string className, string methodName, bool success, string message)
        {
            ServiceResponseModel response = new ServiceResponseModel();
            response.ClassName = className;
            response.MethodName = methodName;
            response.Message = message;
            response.Success = success;

            return response;
        }
    }
}