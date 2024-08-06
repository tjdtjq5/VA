using Best.HTTP;
using Newtonsoft.Json;
using OPS.Obfuscator.Attribute;
using System;
using System.IO;
using System.Text;
using UnityEngine;

[DoNotObfuscateClass]
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
            request.SetHeader(_jwtTokenHeaderKey, JwtToken);

        if (AccountId > 0)
            request.SetHeader(_accountIdHeaderKey, AccountId.ToString());
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
                UnityHelper.LogError_H($"Server sent an error: {response.StatusCode}-{response.DataAsText}\nRequest Url : {request.Uri}");


                ErrorResponse errorResponse = CSharpHelper.DeserializeObject<ErrorResponse>(response.DataAsText);
                if (errorResponse == null)
                {
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
                    ErrorResponseMessage(errorMsgType);
            }
        }
        catch (AsyncHTTPException e)
        {
            Debug.LogError($"Request finished with error! Error: {e.Message}");
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
