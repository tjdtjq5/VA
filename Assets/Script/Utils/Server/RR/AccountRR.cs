[System.Serializable]
public class AccountLoginRequest
{
    public ProviderType ProviderType {  get; set; }
    public string NetworkId { get; set; }
}
[System.Serializable]
public class AccountLoginResponce
{
    public bool LoginOk { get; set; }
    public string JwtAccessToken { get; set; }
}