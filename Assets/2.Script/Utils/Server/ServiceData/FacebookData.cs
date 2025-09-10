using System;
using System.Collections.Generic;

[Serializable]
public class FacebookResponceJsonData
{
    public FacebookTokenData data { get; set; }
}
[Serializable]

public class FacebookTokenData
{
    public string app_id;
    public string type;
    public string application;
    public long data_access_expires_at;
    public long expires_at;
    public bool is_valid;
    public long issued_at;
    public List<string> scopes;
    public string user_id;
    public FacebookErrorData error;
}
[Serializable]
public class FacebookErrorData
{
    public int code;
    public string message;
}
