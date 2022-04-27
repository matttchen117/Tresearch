using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    /// <summary>
    ///     SearchResponse: Object that represents the response information of Search related methods
    /// </summary>
    /// <typeparam name="T">The type of the Data that the response will store</typeparam>
    public class SearchResponse<T> : IResponse<T>
    {
        public string ErrorMessage { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }

        /// <summary>
        ///     public SearchResponse():
        ///         Constructor for SearchResponse objects
        /// </summary>
        /// <param name="errorMessage">The error message of the response</param>
        /// <param name="data">The data of type T of the response</param>
        /// <param name="statusCode">The status code of the response</param>
        /// <param name="isSuccess">Whether or not the corresponding operation was successful</param>
        public SearchResponse(string errorMessage, T? data, int statusCode, bool isSuccess)
        {
            ErrorMessage = errorMessage;
            Data = data;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
        }

        /// <summary>
        ///     Equals:
        ///         overriden equals method to compare SearchResponse's (mainly for testing purposes)
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>Whether the objects are equal</returns>
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
