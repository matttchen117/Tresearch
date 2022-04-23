using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class TagResponse<T> : IResponse<T>
    {
        public string ErrorMessage { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }

        public TagResponse(string errorMessage, T data, int statusCode, bool isSuccess)
        {
            ErrorMessage = errorMessage;
            Data = data;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
        }
    }
}
