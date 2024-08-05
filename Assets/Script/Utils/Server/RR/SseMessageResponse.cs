public record SseMessageResponse
{
    public string Id { get; set; } = null!;
    public string Event { get; set; } = null!;
    public string Data { get; set; } = null!;
}
public record SseClientId
{
    public string ClientId { get; set; } = null!;
}