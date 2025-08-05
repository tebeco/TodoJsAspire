using Microsoft.AspNetCore.Http.HttpResults;

namespace TodoJsAspire.ApiService;

public static class TypedResultsExtensions
{
    public static Results<Ok<T>, NotFound> ToOkOrNotFound<T>(this IResultExtensions _, T? t)
        => t is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(t);
}