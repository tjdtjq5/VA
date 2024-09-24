using UnityEngine;

public static class PlayerPrefsHelper
{
    public static bool HasKey_H(PlayerPrefsKey key)
    {
        return PlayerPrefs.HasKey(key.ToString());
    }

    public static void Set_H(PlayerPrefsKey key, string data)
    {
        PlayerPrefs.SetString(key.ToString(), data);
    }
    public static string GetString_H(PlayerPrefsKey key)
    {
        if (HasKey_H(key))
            return PlayerPrefs.GetString(key.ToString());
        else
            return "";
    }

    public static void Set_H(PlayerPrefsKey key, int data)
    {
        PlayerPrefs.SetInt(key.ToString(), data);
    }
    public static int GetInt_H(PlayerPrefsKey key)
    {
        if (HasKey_H(key))
            return PlayerPrefs.GetInt(key.ToString());
        else
            return -1;
    }

    public static void Set_H(PlayerPrefsKey key, float data)
    {
        PlayerPrefs.SetFloat(key.ToString(), data);
    }
    public static float GetFloat_H(PlayerPrefsKey key)
    {
        if (HasKey_H(key))
            return PlayerPrefs.GetFloat(key.ToString());
        else
            return -1;
    }
}
public enum PlayerPrefsKey
{
    auto_login_jwt_token,
    auto_login_provider,
    auto_login_account_id,
}