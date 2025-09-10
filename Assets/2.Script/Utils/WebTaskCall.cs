using System.Text;
using System.Threading.Tasks;
using Shared.CSharp;
using UnityEngine;
using UnityEngine.Networking;

public class WebTaskCall
{
    public static async Task<T> Get<T>(string url) where T : new()
    {
        System.UriBuilder builder = new System.UriBuilder(url);
        var request = UnityWebRequest.Get(builder.Uri);
        request.SetRequestHeader("Content-Type", "json/application");
        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        var jsonResonse = request.downloadHandler.text;
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {request.error}");
        }

        if (typeof(T) == typeof(string))
        {
            return (T)(object)(jsonResonse);
        }
        else
        {
            var result = CSharpHelper.DeserializeObject<T>(jsonResonse);
            return result;
        }
    }
    public static async Task<T> Get<T>(bool isMyServer, string url) where T : new()
    {
        string sendUrl = isMyServer ? $"{GameOptionManager.GetCurrentServerUrl}/{url}" : url;
        return await Get<T>(sendUrl);
    }
    public static async Task<string> Get(string url)
    {
        System.UriBuilder builder = new System.UriBuilder(url);
        var request = UnityWebRequest.Get(builder.Uri);
        request.SetRequestHeader("Content-Type", "json/application");
        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        var jsonResonse = request.downloadHandler.text;
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Failed: {request.error}");
        }

        return jsonResonse;
    }
    public static async Task<string> Get(bool isMyServer, string url)
    {
        string sendUrl = isMyServer ? $"{GameOptionManager.GetCurrentServerUrl}/{url}" : url;
        return await Get(sendUrl);
    }

    public static async Task<T> Post<T>(string url, object obj) where T : new()
    {
        WWWForm form = new WWWForm();

        byte[] jsonBytes = null;
        if (obj != null)
        {
            string jsonStr = CSharpHelper.SerializeObject(obj);
            jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
        }

        using (var uwr = UnityWebRequest.Post(url, form))
        {
            uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            var operation = uwr.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            var jsonResonse = uwr.downloadHandler.text;
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                var errorResponse = CSharpHelper.DeserializeObject<ErrorResponse>(jsonResonse);
                UnityHelper.Error_H($"{jsonResonse}\nFailed: {uwr.error}\nurl : {url}\nsendData : {CSharpHelper.SerializeObject(obj)}");
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)(jsonResonse);
            }
            else
            {
                var result = CSharpHelper.DeserializeObject<T>(jsonResonse);
                return result;
            }
        }
    }
    public static async Task<T> Post<T>(bool isMyServer, string url, object obj) where T : new()
    {
        string sendUrl = isMyServer ? $"{GameOptionManager.GetCurrentServerUrl}/{url}" : url;
        return await Post<T>(sendUrl, obj);
    }
    public static async Task<string> Post(string url, object obj)
    {
        WWWForm form = new WWWForm();

        byte[] jsonBytes = null;
        if (obj != null)
        {
            string jsonStr = CSharpHelper.SerializeObject(obj);
            jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
        }

        using (var uwr = UnityWebRequest.Post(url, form))
        {
            uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            var operation = uwr.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            var jsonResonse = uwr.downloadHandler.text;
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                var errorResponse = CSharpHelper.DeserializeObject<ErrorResponse>(jsonResonse);
                UnityHelper.Error_H($"{jsonResonse}\nFailed: {uwr.error}\nurl : {url}\nsendData : {CSharpHelper.SerializeObject(obj)}");
            }

            return jsonResonse;
        }
    }
    public static async Task<string> Post(bool isMyServer, string url, object obj)
    {
        string sendUrl = isMyServer ? $"{GameOptionManager.GetCurrentServerUrl}/{url}" : url;
        return await Post(sendUrl, obj);
    }
}
