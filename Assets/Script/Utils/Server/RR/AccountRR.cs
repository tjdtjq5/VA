public class AccountLoginRequest
{
    public ProviderType ProviderType;
    public string Token;
}
public class AccountLoginResponce
{
    public bool LoginOk;
    public string JwtAccessToken;
}