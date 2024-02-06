namespace HotelChief.API.Middleware.Exceptions
{
    using System.Net;

    internal class ExceptionResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public ExceptionResponse(HttpStatusCode statusCode, string description)
        {
            StatusCode = statusCode;
            StatusMessage = description;
        }
    }
}