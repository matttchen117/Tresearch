﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class PrivateResponse<T> : IResponse<T>
    {

        public string ErrorMessage { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }



        public PrivateResponse(string errorMessage, T? data, int statusCode, bool isSuccess)
        {

            ErrorMessage = errorMessage;
            Data = data;
            StatusCode = statusCode;
            IsSuccess = isSuccess;

        }

    }

}