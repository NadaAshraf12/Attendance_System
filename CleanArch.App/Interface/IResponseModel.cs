using CleanArch.App.Services;

namespace CleanArch.App.Interface
{
    public interface IResponseModel
    {
        ResponseModel Response(int statusCode, bool isError, string Message, dynamic Data);
    }
}
