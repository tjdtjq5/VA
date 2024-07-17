//using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookLogin : MonoBehaviour
{
    const string appId = "810697856028827";
    const string secretCode = "f6f931bdbcfd3490db14c31e6fa0b2aa";

    List<string> permission = new List<string>() { "email" };

    private void Start()
    {
        Init();
    }

    void Init()
    {
        //FB.Init(appId, secretCode);
    }

    [ContextMenu("OnLogin")]
    public void OnLogin()
    {
        //FB.LogInWithPublishPermissions(permission, (result) => 
        //{
        //    Debug.Log($"Access FB !");

        //    string token = result.AccessToken.TokenString;

        //    Dictionary<string, IComparable> reqDics = new Dictionary<string, IComparable>()
        //    {
        //        { "Token", token }
        //    };

        //    Managers.Web.SendPostRequest<FacebookWebResponce>("account/login/facebook", reqDics, (res => 
        //    {
        //        Debug.Log(res.JwtAccessToken);
        //        Managers.Web.JwtToken = res.JwtAccessToken;
        //    }));
        //});
    }
}