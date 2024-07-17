using UnityEditor;

public class EditorMessageUtils
{
    public static bool DialogMessage(string title, string message)
    {
        return EditorUtility.DisplayDialog(title, message, "OK");
    }
    public static bool DialogMessageYesNo(string title, string message)
    {
        return EditorUtility.DisplayDialog(title, message, "OK", "Cancle");
    }
}
