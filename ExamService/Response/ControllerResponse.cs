namespace ExamService.Response
{
    public record ControllerResponse<T>
    {
        public T? Data { get; set; } = default;

        public bool Success { get; set; } = true;

        public string Message { get; set; } = string.Empty;

        public IDictionary<string, string[]> Errors { get; set; } = default!;
    }
}
