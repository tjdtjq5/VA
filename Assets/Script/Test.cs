using EasyButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        GoogleLogin.Initialize();
        AppleLogin.Initialize();
    }

    [Button]
    public void GoogleLoginFunc()
    {
        GoogleLogin.SignIn();
    }
    [Button]
    public void GoogleLogoutFunc()
    {
        GoogleLogin.SignOut();
    }
    [Button]
    public void GoogleTokenFunc()
    {
        GoogleLogin.GetAccessToken();
    }
    [Button]
    public void AppleLoginFunc()
    {
        AppleLogin.SignIn();
    }
    [Button]
    public void AppleLogoutFunc()
    {
        AppleLogin.SignOut();
    }
    [Button]
    public void AppleTokenFunc()
    {
        AppleLogin.GetAccessToken();
    }
    [Button]
    public void TT()
    {
        TableSOMakePacket.ChangeUpdate("a");
    }
    [Button]
    public void MasterGets()
    {
        Managers.Table.DbGets(); 
    }
    [Button]
    public void CharacterGets()
    {
        UnityHelper.SerializeL(Managers.PlayerData.Item.Gets());
    }
    [Button]
    public void SimpleFormatTest_Update()
    {
        PlayerDataCPacket.Create("Character");
    }

    [Button]
    public void SimpleFormatTest_Exist()
    {
        UnityHelper.Log_H(PlayerDataCPacket.Exist("Character"));
    }
 
    [Button]
    public void SimpleFormatTest_Remove()
    {
        PlayerDataCPacket.Remove("Character");
    }
}