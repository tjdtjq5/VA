[System.Serializable]
public class AccountLoginRequest
{
    public ProviderType ProviderType {  get; set; }
    public string Token { get; set; }
}
public class AccountLoginResponce
{
    public bool LoginOk { get; set; }
    public string JwtAccessToken { get; set; }
}