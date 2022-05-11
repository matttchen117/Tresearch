using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeContentResponse<T> : IResponse<T>, IEquatable<NodeContentResponse<T>>
    {
        public string ErrorMessage { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }

        public NodeContentResponse(string errorMessage, T? data, int statusCode, bool isSuccess)
        {
            ErrorMessage = errorMessage;
            Data = data;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
        }

        public bool Equals(NodeContentResponse<T>? obj)
        {
            if (obj != null && Data != null && obj.Data != null && Data.GetType().Equals(obj.GetType()))
            {
                return ErrorMessage.Equals(obj.ErrorMessage) && (Data != null) ? Data.Equals(obj.Data) : true
                    && StatusCode == obj.StatusCode && IsSuccess == obj.IsSuccess;
            }
            return false;
        }
    }
}
