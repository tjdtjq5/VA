using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleSignIn.Apple.Scripts;
using Assets.SimpleSignIn.Google.Scripts;
using UnityEngine;
using JWT = Assets.SimpleSignIn.Google.Scripts.JWT;

public class LoginTest : MonoBehaviour
{
    public GoogleAuth GoogleAuth;
    public AppleAuth AppleAuth;
    
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        GoogleAuth = new GoogleAuth();
        GoogleAuth.DebugLog = false;
        GoogleAuth.TryResume(OnSignIn, OnGetTokenResponse);
        
        AppleAuth = new AppleAuth();
        AppleAuth.TryResume(OnSignIn, OnGetTokenResponse);
    }
    
    public void GoogleSignIn()
    {
        this.GoogleAuth.SignIn(OnSignIn, caching: true);
    }

    public void GoogleSignOut()
    {
        this.GoogleAuth.SignOut(revokeAccessToken: true);
    }
    
    public void GoogleToken()
    {
        this.GoogleAuth.GetTokenResponse(OnGetTokenResponse);
    }
    
    public void AppleSignIn()
    {
        AppleAuth.SignIn(OnSignIn, caching: true);
    }
    
    public void AppleSignOut()
    {
        AppleAuth.SignOut(revokeAccessToken: true);
        UnityHelper.Log_H("Not signed in");
    }
    
    public void AppleToken()
    {
        AppleAuth.GetTokenResponse(OnGetTokenResponse);
    }

    private void OnSignIn(bool success, string error, Assets.SimpleSignIn.Google.Scripts.UserInfo userInfo)
    {
        UnityHelper.Log_H(userInfo.sub);
    }
    private void OnSignIn(bool success, string error, Assets.SimpleSignIn.Apple.Scripts.UserInfo userInfo)
    {
        UnityHelper.Log_H(success ? $"Hello, {userInfo.Name}!" : error);
    }
    private void OnGetTokenResponse(bool success, string error, Assets.SimpleSignIn.Google.Scripts.TokenResponse tokenResponse)
    {
        UnityHelper.Log_H(success ? $"Access token: {tokenResponse.AccessToken}" : error);

        if (!success) return;

        var jwt = new JWT(tokenResponse.IdToken);

        UnityHelper.Log_H($"JSON Web Token (JWT) Payload: {jwt.Payload}");

        jwt.ValidateSignature(this.GoogleAuth.ClientId, OnValidateSignature);
    }
    private void OnGetTokenResponse(bool success, string error, Assets.SimpleSignIn.Apple.Scripts.TokenResponse tokenResponse)
    {
        UnityHelper.Log_H(success ? $"Access token: {tokenResponse.AccessToken}" : error);

        if (!success) return;

        var jwt = new JWT(tokenResponse.IdToken);

        UnityHelper.Log_H($"JSON Web Token (JWT) Payload: {jwt.Payload}");

        jwt.ValidateSignature(this.AppleAuth.ClientId, OnValidateSignature);
    }
    private void OnValidateSignature(bool success, string error)
    {
        UnityHelper.Log_H(success ? "JWT signature validated" : error);
    }
}
