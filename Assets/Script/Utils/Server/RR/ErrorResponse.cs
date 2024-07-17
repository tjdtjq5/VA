public class ErrorResponse
{
    public string Title { get; set; }
    public string Status { get; set; }
    public string Detail { get; set; }

    public HttpResponceMessageType MessageType 
    { 
        get 
        {
            HttpResponceMessageType result = CSharpHelper.Parse<HttpResponceMessageType>(Detail, false);
            return result;
        } 
    }
}
