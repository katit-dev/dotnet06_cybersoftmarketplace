public class HTTPResponseData<T>
{
    public T DataResponse {get; set;}
    public string Message {get; set;}
    public int statusCode {get; set;}
    public DateTime Timestamp {get; set;}
}

public static class UserResponseMessageDTO
{
    public const string UserRegisteredSuccessfully = "User registered successfully.";
    
}