using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class SearchResponse<T> : IResponse<T>
    {
        public string ErrorMessage { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public SearchResponse(string errorMessage, T data, int statusCode, bool isSuccess)
        {
            ErrorMessage = errorMessage;
            Data = data;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
        }

        public override bool Equals(object? obj)
        {
            if(!(obj == null))
            {
                if(obj is SearchResponse<T>)
                {
                    SearchResponse<T> response = (SearchResponse<T>)obj;
                    return ErrorMessage.Equals(response.ErrorMessage) && (Data != null) ? Data.Equals(response.Data) : true
                        && StatusCode == response.StatusCode && IsSuccess == response.IsSuccess;
                }
            }
            return false;
        }
    }
}
