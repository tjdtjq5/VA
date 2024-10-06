using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


#if UNITY_EDITOR
using System.Windows.Forms;
#endif
using UnityEditor;

public class FileHelper
{
    static string assetFolderName = "Assets";
    static SecretOptionFile secretFileTxt = new SecretOptionFile();

    public static string GetFile(string[] folders, string fileName)
    {
        string path = assetFolderName;

        for (int i = 0; i < folders.Length; i++)
        {
            string checkPath = $"{path}/{folders[i]}";
            if (!DirectoryExist(checkPath))
            {
                Directory.CreateDirectory($"{checkPath}");
            }

            path += $"/{folders[i]}";
        }

        string file = $"{UnityEngine.Application.dataPath.Replace(assetFolderName, path)}/{fileName}";

        return file;
    }
    public static void Write(string file, string text, bool isRefreash)
    {
        File.WriteAllText(file, text, Encoding.UTF8);

#if UNITY_EDITOR
        if (isRefreash)
            AssetDatabase.Refresh();
#endif
    }
    public static IEnumerable<string> ReadLines(string file)
    {
        return File.ReadLines(file, Encoding.UTF8);
    }
    public static string ReadAll(string file)
    {
        return File.ReadAllText(file, Encoding.UTF8);
    }
    public static bool FileExist(string file)
    {
        return File.Exists(file);
    }
    public static bool ScriptExist(string type)
    {
        string path = GetScriptPath(type);
        return !string.IsNullOrEmpty(path);
    }
    public static bool DirectoryExist(string directory)
    {
        return Directory.Exists(directory);
    }
    public static void FileDelete(string file, bool isRefreash)
    {
        File.Delete(file);

#if UNITY_EDITOR
        if (isRefreash)
            AssetDatabase.Refresh();
#endif
    }
    public static string SelectFilePath(string fileSrc)
    {
#if UNITY_EDITOR
        FileInfo fileName = null;
        OpenFileDialog fileDialog = new OpenFileDialog()
        {
            InitialDirectory = fileSrc,
            Filter = "All Files | *.*",

            CheckFileExists = true,
            CheckPathExists = true,
        };

        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            fileName = new FileInfo(fileDialog.FileName);
        }

        if (fileName == null)
        {
            return "";
        }
        else
        {
            return fileName.FullName;
        }
#endif
        return "";
    }
    public static string GetCurrentDirectory()
    {
        return Directory.GetCurrentDirectory();
    }
    public static string GetScriptPath(Type type)
    {
        return GetScriptPath(type.Name);
    }
    public static string GetScriptPath(string type)
    {
#if UNITY_EDITOR
        var g = AssetDatabase.FindAssets($"t:Script {type}");

        foreach (var asset in g)
        {
            string path = AssetDatabase.GUIDToAssetPath(asset);
            string[] ps = path.Split('/');
            string result = ps[ps.Length - 1].Replace(".cs", "");

            if (type.Equals(result))
            {
                return path;
            }
        }
#endif
        return $"";
    }
    public static bool IsPathData(string path)
    {
        // 경로 길이 체크
        if (path.Length < 3)
            return false;

        // 드라이브 문자열 체크
        Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
        if (driveCheck.IsMatch(path.Substring(0, 3)) == false)
            return false;

        // 경로 이름에 사용할 수 없는 문자가 있는지 체크
        string invalidPathChars = new string(Path.GetInvalidPathChars());
        invalidPathChars += @":/?*" + "\"";

        Regex regexInvalidPath = new Regex("[" + Regex.Escape(invalidPathChars) + "]");
        if (regexInvalidPath.IsMatch(path.Substring(3, path.Length - 3)))
            return false;

        return true;
    }

    public static void ProcessStart(string file)
    {
        Process.Start(file);
    }

    public static void RunCmd(string exePath, string directory, string arguments = null)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo(exePath, arguments);
        startInfo.Arguments = " -c \"" + arguments + " \"";
        startInfo.WorkingDirectory = directory;
        startInfo.CreateNoWindow = true;
        startInfo.RedirectStandardError = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardInput = false;
        startInfo.UseShellExecute = false;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.ErrorDialog = false;
        startInfo.Environment["LANG"] = "en_US.UTF-8"; //REQUIRED FOR POD INSTALL

        using (Process p = new Process { StartInfo = startInfo, EnableRaisingEvents = true })
        {
            p.ErrorDataReceived += (sender, e) => {
                if (!System.String.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.Log("Error: " + e.Data);
                }
            };

            p.OutputDataReceived += (sender, e) => {
                if (!System.String.IsNullOrEmpty(e.Data))
                {
                    UnityEngine.Debug.Log("Output: " + e.Data);
                }
            };

            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.WaitForExit();
        }
    }
}
