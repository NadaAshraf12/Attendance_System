using CleanArch.App.Interface;

namespace CleanArch.App.Services
{
    public class ResponseModel : IResponseModel
    {
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
        public dynamic Result { get; set; }

        ResponseModel IResponseModel.Response(int statusCode, bool isError, string message, dynamic data)
        {
            return new ResponseModel
            {
                Timestamp = DateTime.UtcNow,
                StatusCode = statusCode,
                IsError = isError,
                Message = message,
                Result = data
            };
        }

        // ✅ Add helper methods
        public static ResponseModel Success(string message, dynamic data = null) =>
            new ResponseModel { Timestamp = DateTime.UtcNow, StatusCode = 200, IsError = false, Message = message, Result = data };

        public static ResponseModel Fail(string message, int statusCode = 400) =>
            new ResponseModel { Timestamp = DateTime.UtcNow, StatusCode = statusCode, IsError = true, Message = message };


    }
}
