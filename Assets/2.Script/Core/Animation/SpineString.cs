using System;
using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SpineString : MonoBehaviour
{
    public Action<string, string, SpineStringPositionType> OnEffect;
    public Action<string> OnSound;
    
    private readonly string _stringEvent = "string";
    private readonly char _stringSeparator = '/';
    private readonly char _folderSeparator = '|';
    private readonly char _andSeparator = '&';
    private readonly string _effectString = "effect";
    private readonly string _soundString = "sound";
    private readonly string _cameraShakeString = "cameraShake";
    private readonly string _effectPath = "Prefab/Effect/{0}";

    
    public void Initialize(SkeletonAnimation sa)
    {
        if (sa != null && sa.AnimationState != null) {
            sa.AnimationState.Event -= EventListener; // 기존 이벤트 제거
            sa.AnimationState.Event += EventListener; // 새로운 이벤트 등록
        } else {
            UnityHelper.Error_H($"Failed to register event listener - sa or AnimationState is null");
        }
    }

    public void Initialize(SkeletonGraphic sa)
    {
        if (sa != null && sa.AnimationState != null) {
            sa.AnimationState.Event -= EventListener; // 기존 이벤트 제거
            sa.AnimationState.Event += EventListener; // 새로운 이벤트 등록
        } else {
            UnityHelper.Error_H($"Failed to register event listener - sa or AnimationState is null");
        }
    }
    
    void EventListener(TrackEntry trackEntry, Spine.Event e)
    {
        string[] stringDatas = e.String.Split(_andSeparator);
        for (int i = 0; i < stringDatas.Length; i++)
            StringAction(stringDatas[i]);
    }
    
    void StringAction(string str)
    {
        if (string.IsNullOrEmpty(str))
            return;

        
        string[] datas = str.Split(_stringSeparator);

        if (datas[0].Contains(_effectString))
        {
            if (datas.Length == 4)
            {
                EffectAction(
                    CSharpHelper.Format_H(_effectPath, datas[1])
                    ,datas[2]
                    ,CSharpHelper.EnumParse<SpineStringPositionType>(datas[3], true));
            }
            else
                UnityHelper.Error_H($"SpineAniController StringAction Error\nstr : {str}\ngameobject : {gameObject.name}");
        }
        else if (datas[0].Contains(_soundString))
        {
            if (datas.Length == 2)
            {
                SoundAction(datas[1]);
            }
            else
                UnityHelper.Error_H($"SpineAniController StringAction Error\nstr : {str}\ngameobject : {gameObject.name}");
        }
        else if (datas[0].Contains(_cameraShakeString))
        {
            if (datas.Length == 2)
            {
                CameraShakeAction(datas[1]);
            }
            else
                UnityHelper.Error_H($"SpineAniController StringAction Error\nstr : {str}\ngameobject : {gameObject.name}");
        }
        else
        {
            UnityHelper.Error_H($"SpineAniController StringAction Error\nstr : {str}\ngameobject : {gameObject.name}");
        }
    }
    void EffectAction(string prefabPath, string boneName, SpineStringPositionType positionStr)
    {
        prefabPath = prefabPath.Replace(_folderSeparator, '/');
        OnEffect?.Invoke(prefabPath, boneName, positionStr);
    }
    void SoundAction(string clipName)
    {
        clipName = clipName.Replace(_folderSeparator, '/');
        Managers.Sound.Play(GetClipName(clipName));
        OnSound?.Invoke(clipName);
    }
    void CameraShakeAction(string index)
    {
        int indexInt = int.Parse(index);
        Managers.Observer.CameraController.Shake(indexInt);
    }

    string GetClipName(string clipName)
    {
        string result = clipName;
        switch (clipName)
        {
            case "at":
                result = $"{clipName}_{(int)UnityHelper.Random_H(1, 4)}";
                break;
        }
        return result;
    }

    public void Clear()
    {
        OnEffect = null;
        OnSound = null;
    }
}

public enum SpineStringPositionType
{
    n,
    nr,
    p,
    pr,
}