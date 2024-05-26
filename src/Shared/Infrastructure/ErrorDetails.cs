using System.Net;
using System.Text.Json;

namespace Svk.Shared.Infrastructure;

//TODO copyd from https://github.com/HOGENT-Web/csharp-ch-8-example-2/blob/solution/src/Shared/Infrastructure/ErrorDetails.cs

/// <summary>
/// Error returned by the <see cref="ExceptionMiddleware"/> when an <see cref="Exception"/> is thrown.
/// This class is used to not show the entire stacktrace to the client.
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// The HTTP statuscode
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// The custom message of the error, most of the time the one from the exception.
    /// </summary>
    public string? Message { get; set; }

    public ErrorDetails()
    {
    }

    public ErrorDetails(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        StatusCode = (int)statusCode;
        Message = message;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}