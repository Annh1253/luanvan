using System.Net;

namespace ExamService.Response
{
    public record ServiceResponse<T>
    {
        public T? Data { get; set; } = default;

        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public IDictionary<string, string[]> Errors { get; set; } = default!;
    }
}
