using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        File.WriteAllText(file, text);

#if UNITY_EDITOR
        if (isRefreash)
            AssetDatabase.Refresh();
#endif
    }
    public static IEnumerable<string> ReadLines(string file)
    {
        return File.ReadLines(file);
    }
    public static string ReadAll(string file)
    {
        return File.ReadAllText(file);
    }
    public static bool FileExist(string file)
    {
        return File.Exists(file);
    }
    public static bool DirectoryExist(string directory)
    {
        return Directory.Exists(directory);
    }
    public static void FileDelete(string file)
    {
        File.Delete(file);
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
#if UNITY_EDITOR
        var g = AssetDatabase.FindAssets($"t:Script {type}");

        foreach (var asset in g)
        {
            string path = AssetDatabase.GUIDToAssetPath(asset);
            string[] ps = path.Split('/');
            string result = ps[ps.Length - 1].Replace(".cs", "");

            if (type.Name.Equals(result))
            {
                return path;
            }
        }
#endif
        return $"";
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
