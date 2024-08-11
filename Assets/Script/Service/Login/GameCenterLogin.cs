using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterLogin : MonoBehaviour, ILoginService
{
    public void Initialize()
    {
    }

    public void Login(Action callback)
    {
        if (Social.localUser.authenticated == true)
        {
            Debug.Log("Success to true");
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Success to authenticate");
                }
                else
                {
                    Debug.Log("Faile to login");
                }
            });
        }
    }
}
