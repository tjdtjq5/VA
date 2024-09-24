[System.Serializable]
public class AccountLoginRequest
{
    public ProviderType ProviderType {  get; set; }
    public string NetworkIdOrCode { get; set; }
}
[System.Serializable]
public class AccountAutoLoginRequest
{
    public ProviderType ProviderType { get; set; }
    public string JwtToken { get; set; }
    public int AccountId { get; set; }
}
[System.Serializable]
public class AccountLoginResponce
{
    public bool LoginOk { get; set; }
    public string JwtAccessToken { get; set; }
    public int AccountId {  get; set; }
}