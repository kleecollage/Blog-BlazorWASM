using System.Net;

namespace ApiBlogApp.Models;

public class ApiResponse
{
    // THIS CTOR IS REPLACED WITH = []
    // public ApiResponse() { ErrorMessages = new List<string>(); }
    
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> ErrorMessages { get; set; } = [];
    public object Result { get; set; }
}