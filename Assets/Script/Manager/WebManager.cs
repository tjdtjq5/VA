using Best.HTTP;
using System;
using System.IO;
using System.Text;
using UnityEngine;

#if !UNITY_WEBGL || UNITY_EDITOR
using Best.TLSSecurity;
#endif

public class WebManager
{
    public string JwtToken { get; set; }
    public int AccountId { get; set; }

    JobSerializer _jobSerializer = new JobSerializer();
    bool _isWorking;

    public void Initialize()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        TLSSecurity.Setup();
#endif
    }

    public void SendPostRequest<T>(string url, object obj, Action<T> res, params ErrorResponseJob[] errorJob)
	{
        if (_isWorking)
            _jobSerializer.Push(Post, true, url, obj, res, errorJob);
        else
        {
            Post(true, url, obj, res, errorJob);
            _isWorking = true;
        }
    }
    public void SendGetRequest<T>(string url, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        if (_isWorking)
            _jobSerializer.Push(Get, true, url, res, errorJob);
        else
        {
            Get(true, url, res, errorJob);
            _isWorking = true;
        }
    }
    public void SendPostRequest<T>(bool isMyServer, string url, object obj, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        if (_isWorking)
            _jobSerializer.Push(Post, isMyServer, url, obj, res, errorJob);
        else
        {
            Post(isMyServer, url, obj, res, errorJob);
            _isWorking = true;
        }
    }
    public void SendGetRequest<T>(bool isMyServer, string url, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        if (_isWorking)
            _jobSerializer.Push(Get, isMyServer, url, res, errorJob);
        else
        {
            Get(isMyServer, url, res, errorJob);
            _isWorking = true;
        }
    }

    void Get<T>(bool isMyServer, string url, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        var request = HTTPRequest.CreatePost($"{GetUrl(isMyServer, url)}");

        AddHeader(request);

        Send(request, res, errorJob);
    }
    void Post<T>(bool isMyServer, string url, object obj, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        var request = HTTPRequest.CreatePost($"{GetUrl(isMyServer, url)}");

        request.SetHeader("content-type", "application/json");

        AddHeader(request);

        if (obj != null)
        {
            byte[] jsonBytes = null;
            string jsonStr = CSharpHelper.SerializeObject(obj);
            jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
            request.UploadSettings.UploadStream = new MemoryStream(jsonBytes);
        }

        Send(request, res, errorJob);
    }

    void AddHeader(HTTPRequest request)
    {
        if (!string.IsNullOrEmpty(JwtToken))
            request.SetHeader(HttpHeaderKey.JwtToken.ToString(), JwtToken);

        if (AccountId > 0)
            request.SetHeader(HttpHeaderKey.AccountId.ToString(), AccountId.ToString());
    }
    async void Send<T>(HTTPRequest request, Action<T> res, params ErrorResponseJob[] errorJob)
    {
        try
        {
            var response = await request.GetHTTPResponseAsync();

            if (response.IsSuccess)
            {
                if (typeof(T) == typeof(string))
                {
                    res.Invoke((T)(object)(response.DataAsText));
                }
                else
                {
                    T resObj = CSharpHelper.DeserializeObject<T>(response.DataAsText);
                    res.Invoke(resObj);
                }
            }
            else
            {
                ErrorResponse errorResponse = CSharpHelper.DeserializeObject<ErrorResponse>(response.DataAsText);
                if (errorResponse == null)
                {
                    UnityHelper.Error_H($"ErrorResponse DeserializeObject Error");
                    return;
                }

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
                {
                    UnityHelper.Error_H($"Server sent an error: {response.StatusCode}-{response.DataAsText}\nRequest Url : {request.Uri}");
                    ErrorResponseMessage(errorMsgType);
                }

            }
        }
        catch (AsyncHTTPException e)
        {
            // UnityHelper.LogError_H($"Request finished with error! Error: {e.Message}");
        }
        finally 
        {
            if (_jobSerializer.Count <= 0)
                _isWorking = false;
            else
                _jobSerializer.Pop().Execute();
        }
    }

    void ErrorResponseMessage(HttpResponceMessageType type)
    {
    }

    private string GetUrl(bool isMyServer, string url)
    {
        return isMyServer ? $"{GameOptionManager.GetCurrentServerUrl}/{url}" : url;
    }
}
public enum WebRequestMethod
{
	Get,
	Post,
}
