namespace VIKTALEA_Backend.Shared
{
    public record ApiError(string error, string? detail = null);
    public record ApiResponse<T>(bool success, string TraceId, T? data = default, ApiError? error = null, Pagination? pagination = null)
    {
        public static ApiResponse<T> Success(T data, string traceId, Pagination pagination) => new(true, traceId, data, null, pagination);
        public static ApiResponse<T> Fail(string error, string? detail, string traceId) => new(false, traceId, default, new ApiError(error, detail), null);
    }
}
