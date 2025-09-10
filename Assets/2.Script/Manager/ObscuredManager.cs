using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.Genuine.Android;
using CodeStage.AntiCheat.Genuine.CodeHash;
using Shared.DTOs.ETC;
using UnityEngine;

public class ObscuredManager : MonoBehaviour
{
    public Action OnHashCheckSuccess;
    
    public void Initialize()
    {
        if (Application.platform == RuntimePlatform.Android)
            CodeHashGenerator.HashGenerated += HashGenerat;

        if (Application.platform != RuntimePlatform.WindowsEditor)
        {
            SpeedHackDetector.StartDetection(SpeedHackDetected);
            WallHackDetector.StartDetection(WallHackDetected);
            InjectionDetector.StartDetection(InjectionHackDetected);
            ObscuredCheatingDetector.StartDetection(ObscuredCheatingHackDetected);
        }
    }
    public void HashCheck()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            CodeHashGenerator.Generate();
        }
        else
        {
            OnHashCheckSuccess?.Invoke();
        }
    }
    void HashGenerat(HashGeneratorResult result)
    {
        if (result.Success)
        {
            HashCheckRequest req = new HashCheckRequest()
            {
                Version = Application.version,
                SummaryHash = result.SummaryHash
            };
            Managers.Web.SendPostRequest<bool>("HashCheck/Check", req, res =>
            {
                if (res)
                {
                    OnHashCheckSuccess?.Invoke();
                }
                else
                {
                    UnityHelper.Log_H($"Ban User!");
                }
            });
            UnityHelper.Log_H($"Hash Gen : {result.SummaryHash}");
        }
        else
        {
            UnityHelper.Log_H($"Hash Gen Error Message : {result.ErrorMessage}");
        }
    }
    public bool AndroidAppInstallationSourceValidation()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var source = AppInstallationSourceValidator.GetAppInstallationSource();
            if (source.DetectedSource != AndroidAppSource.AccessError)
            {
                Debug.Log($"Installed from: {source.DetectedSource} (package name: {source.PackageName})");

                if (source.DetectedSource == AndroidAppSource.AccessError || source.DetectedSource == AndroidAppSource.Other)
                    return false;
            
                return true;
            }
            else
            {
                Debug.LogError("Failed to detect the installation source!");
                return false;
            }
        }
        
        return true;
    }

    void SpeedHackDetected()
    {
        UnityHelper.Log_H($"SpeedHackDetected");
    }
    void WallHackDetected()
    {
        UnityHelper.Log_H($"WallHackDetected");
    }
    void InjectionHackDetected(string reason)
    {
        UnityHelper.Log_H($"InjectionHackDetected\nreason : {reason}");
    }
    void ObscuredCheatingHackDetected()
    {
        UnityHelper.Log_H($"ObscuredCheatingHackDetected");
    }
}
