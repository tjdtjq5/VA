using CodeStage.AntiCheat.Storage;
using UnityEngine;

public static class PlayerPrefsHelper
{
    public static bool HasKey_H(PlayerPrefsKey key)
    {
        return ObscuredPrefs.HasKey(key.ToString());
    }

    public static void Set_H(PlayerPrefsKey key, string data)
    {
        ObscuredPrefs.SetString(key.ToString(), data);
    }
    public static string GetString_H(PlayerPrefsKey key)
    {
        if (HasKey_H(key))
            return ObscuredPrefs.GetString(key.ToString());
        else
            return "";
    }

    public static void Set_H(PlayerPrefsKey key, int data)
    {
        ObscuredPrefs.SetInt(key.ToString(), data);
    }
    public static int GetInt_H(PlayerPrefsKey key)
    {
        if (HasKey_H(key))
            return ObscuredPrefs.GetInt(key.ToString());
        else
            return -1;
    }

    public static void Set_H(PlayerPrefsKey key, float data)
    {
        ObscuredPrefs.SetFloat(key.ToString(), data);
    }
    public static float GetFloat_H(PlayerPrefsKey key)
    {
        if (HasKey_H(key))
            return ObscuredPrefs.GetFloat(key.ToString());
        else
            return -1;
    }
    
    public static void Set_H(PlayerPrefsKey key, bool data)
    {
        ObscuredPrefs.SetInt(key.ToString(), data ? 1 : 0);
    }
    public static bool GetBool_H(PlayerPrefsKey key)
    {
        if (HasKey_H(key))
            return GetInt_H(key) == 1 ? true : false;
        else
            return false;
    }
}
public enum PlayerPrefsKey
{
    auto_login_jwt_token,
    auto_login_provider,
    auto_login_account_id,
    ingame_manager_data,
    
    tutorial_forkroad,
    tutorial_puzzle,
    tutorial_weapon,
    tutorial_week,
    tutorial_stun,
    tutorial_wordTip,

    table_data,
    table_updated_at,
}