using System.Collections.Generic;
using System.Net;

namespace Application.Wrappers
{
    public class RequestResult<T>
    {
        public int StatusCode { get; set; } = (int) HttpStatusCode.OK;
        public bool IsSuccess { get; set; }
        public T Body { get; set; }
        public string Error { get; set; }

        public static RequestResult<T> Success(T value) => new RequestResult<T> {IsSuccess = true, Body = value};
        public static RequestResult<T> Failutre(int statusCode, string error) => new RequestResult<T> {StatusCode = statusCode, IsSuccess = false, Error = error};
    }
}