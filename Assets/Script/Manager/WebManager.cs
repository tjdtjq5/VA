using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager
{
    public string JwtToken { get; set; }

    const string jwtTokenHeaderKey = "JwtToken";

    public void SendPostRequest<T>(string url, object obj, Action<T> res)
	{
        SendPostRequest(true, url, obj, res);
	}
    public void SendGetRequest<T>(string url, Action<T> res)
    {
        SendGetRequest(true, url, res);
    }
    public void SendPostRequest<T>(bool isMyServer, string url, object obj, Action<T> res)
    {
        Managers.Instance.StartCoroutine(CoSendWebRequest(isMyServer, WebRequestMethod.Post, url, obj, res));
    }
    public void SendGetRequest<T>(bool isMyServer, string url, Action<T> res)
    {
        Managers.Instance.StartCoroutine(CoSendWebRequest(isMyServer, WebRequestMethod.Get, url, null, res));
    }
    IEnumerator CoSendWebRequest<T>(bool isMyServer, WebRequestMethod method, string url, object obj, Action<T> res)
    {
        string sendUrl = isMyServer ? $"{GameOptionManager.GetServerUrl}/{url}" : url;

        switch (method)
        {
            case WebRequestMethod.Get:
                using (var uwr = UnityWebRequest.Get(sendUrl))
                {
                    if (!string.IsNullOrEmpty(JwtToken))
                        uwr.SetRequestHeader(jwtTokenHeaderKey, JwtToken);

                    yield return uwr.SendWebRequest();

                    WebResult(uwr, sendUrl, res);
                }
                break;
            case WebRequestMethod.Post:

                WWWForm form = new WWWForm();

                byte[] jsonBytes = null;
                if (obj != null)
                {
                    string jsonStr = CSharpHelper.SerializeObject(obj);
                    jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
                }

                using (var uwr = UnityWebRequest.Post(sendUrl, form))
                {
                    uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
                    uwr.downloadHandler = new DownloadHandlerBuffer();
                    uwr.SetRequestHeader("Content-Type", "application/json");

                    if (!string.IsNullOrEmpty(JwtToken))
                    {
                        uwr.SetRequestHeader(jwtTokenHeaderKey, JwtToken);
                    }

                    yield return uwr.SendWebRequest();

                    WebResult(uwr, sendUrl, res);
                }
                break;
        }
    }
 
    void WebResult<T>(UnityWebRequest req, string sendUrl ,Action<T> res)
    {
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"{req.error}     sendUrl : {sendUrl}    result : {req.downloadHandler.text}");
        }
        else
        {
            try
            {
                if (typeof(T) == typeof(string))
                {
                    res.Invoke((T)(object)(req.downloadHandler.text));
                }
                else
                {
                    T resObj = JsonConvert.DeserializeObject<T>(req.downloadHandler.text);
                    res.Invoke(resObj);
                }
            }
            catch
            {
                Debug.LogError($"Failed Deserialize : {req.downloadHandler.text}\nsendUrl : {sendUrl}");
            }
        }
    }
    void SetHeader(UnityWebRequest uwr)
    {
        if (!string.IsNullOrEmpty(JwtToken))
            uwr.SetRequestHeader(jwtTokenHeaderKey, JwtToken);
    }
}
public enum WebRequestMethod
{
	Get,
	Post,
}
