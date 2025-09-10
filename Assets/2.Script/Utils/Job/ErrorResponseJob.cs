using Shared.Define;

public class ErrorResponseJob
{
    public HttpResponceMessageType _messageType { get; set; }
    public Job _job;

    public ErrorResponseJob(HttpResponceMessageType type)
    {
        this._messageType = type;
        this._job = null;
    }
    public ErrorResponseJob(HttpResponceMessageType type, Job job)
    {
        this._messageType = type;
        this._job = job;
    }
}
