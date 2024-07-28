using UnityEditor;

public class ProjectSettingsEditor
{
    public static void IosUrlSchma(string[] schemes)
    {
        PlayerSettings.iOS.iOSUrlSchemes = schemes;
    }
}
