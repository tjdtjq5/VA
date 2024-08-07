public record SseMessageResponse
{
    public string Id { get; set; } = null!;
    public SseEvent Event { get; set; }
    public string Data { get; set; } = null!;
}
public record SseClientId
{
    public string ClientId { get; set; } = null!;
}