using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager
{
    public string JwtToken { get; set; }
    public int AccountId { get; set; }

    const string _jwtTokenHeaderKey = "jwttoken";
    const string _accountIdHeaderKey = "accountId";

    JobSerializer _jobSerializer = new JobSerializer();
    bool _isWorking;

    public void SendPostRequest<T>(string url, object obj, Action<T> res, params ErrorResponseJob[] errorJob)
	{
        if (_isWorking)
            _jobSerializer.Push(Post, true, url, obj, res, errorJob);
        else
        {
            Post(true, url, obj, res);
            _isWorking = true;
        }
    }
    public void SendGetRequest<T>(string url, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        if (_isWorking)
            _jobSerializer.Push(Get, true, url, res, errorJob);
        else
        {
            Get(true, url, res);
            _isWorking = true;
        }
    }
    public void SendPostRequest<T>(bool isMyServer, string url, object obj, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        if (_isWorking)
            _jobSerializer.Push(Post, isMyServer, url, obj, res, errorJob);
        else
        {
            Post(isMyServer, url, obj, res);
            _isWorking = true;
        }
    }
    public void SendGetRequest<T>(bool isMyServer, string url, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        if (_isWorking)
            _jobSerializer.Push(Get, isMyServer, url, res, errorJob);
        else
        {
            Get(isMyServer, url, res);
            _isWorking = true;
        }
    }

    void Get<T>(bool isMyServer, string url, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        Managers.Instance.StartCoroutine(CoSendWebRequest(isMyServer, WebRequestMethod.Get, url, null, res));
    }
    void Post<T>(bool isMyServer, string url, object obj, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        Managers.Instance.StartCoroutine(CoSendWebRequest(isMyServer, WebRequestMethod.Post, url, obj, res));
    }

    IEnumerator CoSendWebRequest<T>(bool isMyServer, WebRequestMethod method, string url, object obj, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        string sendUrl = isMyServer ? $"{GameOptionManager.GetCurrentServerUrl}/{url}" : url;

        switch (method)
        {
            case WebRequestMethod.Get:
                using (var uwr = UnityWebRequest.Get(sendUrl))
                {
                    uwr.downloadHandler = new DownloadHandlerBuffer();
                    uwr.SetRequestHeader("Content-Type", "application/json");

                    if (!string.IsNullOrEmpty(JwtToken))
                    {
                        uwr.SetRequestHeader(_jwtTokenHeaderKey, JwtToken);
                    }

                    if (AccountId > 0)
                    {
                        uwr.SetRequestHeader(_accountIdHeaderKey, AccountId.ToString());
                    }

                    yield return uwr.SendWebRequest();

                    WebResult(uwr, sendUrl, res, errorJob);
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
                        uwr.SetRequestHeader(_jwtTokenHeaderKey, JwtToken);
                    }

                    if (AccountId > 0)
                    {
                        uwr.SetRequestHeader(_accountIdHeaderKey, AccountId.ToString());
                    }

                    yield return uwr.SendWebRequest();

                    WebResult(uwr, sendUrl, res, errorJob);
                }
                break;
        }
    }
    public async Task<T> PostFormUrlEncoded<T>(string url, IEnumerable<KeyValuePair<string, string>> postData) where T : new()
    {
        using (var httpClient = new HttpClient())
        {
            using (var content = new FormUrlEncodedContent(postData))
            {
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                string json = await response.Content.ReadAsStringAsync();
                T result = CSharpHelper.DeserializeObject<T>(json);
                return result;
            }
        }
    }

    void WebResult<T>(UnityWebRequest req, string sendUrl ,Action<T> res, params ErrorResponseJob[] errorJob)
    {
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"{req.result}     sendUrl : {sendUrl}    result : {req.downloadHandler.text}");

            var errorResponse = CSharpHelper.DeserializeObject<ErrorResponse>(req.downloadHandler.text);

            if (errorResponse != null) 
            {
                HttpResponceMessageType errorMsgType = errorResponse.MessageType;

                bool isJopWork = false;
                for (int i = 0; i < errorJob.Length; i++)
                {
                    if (errorJob[i]._messageType == errorMsgType)
                    {
                        if (errorJob[i]._job != null)
                        {
                            isJopWork = true;
                            errorJob[i]._job.Execute();
                            break;
                        }
                    }
                }

                if (!isJopWork)
                    ErrorResponseMessage(errorMsgType);
            }
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

        if (_jobSerializer.Count <= 0)
            _isWorking = false;
        else
            _jobSerializer.Pop().Execute();
    }
    void SetHeader(UnityWebRequest uwr)
    {
        if (!string.IsNullOrEmpty(JwtToken))
            uwr.SetRequestHeader(_jwtTokenHeaderKey, JwtToken);
    }

    void ErrorResponseMessage(HttpResponceMessageType type)
    {

    }
}
public enum WebRequestMethod
{
	Get,
	Post,
}
