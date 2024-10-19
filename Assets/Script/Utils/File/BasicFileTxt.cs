using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFileTxt : IFileTxt
{
    public virtual string FileName { get => "BasicFileTxt.txt"; }

    public virtual void Add(string key, object value)
    {
        string file = GetFile();

        Dictionary<string, string> data = GetFileData();

        if (data.ContainsKey(key))
        {
            data[key] = CSharpHelper.SerializeObject(value);
        }
        else
        {
            data.Add(key, CSharpHelper.SerializeObject(value));
        }

        string text = CSharpHelper.SerializeObject(data);

        FileHelper.Write(file, text, true);
    }

    public virtual bool Exist()
    {
        return FileHelper.FileExist(GetFile());
    }

    public virtual string GetFile()
    {
        string path = Application.dataPath;

        string file = $"{path}/{"Resources"}/{FileName}";
        return file;
    }

    public virtual Dictionary<string, string> GetFileData()
    {
        TextAsset file = Resources.Load<TextAsset>($"{FileName.Substring(0, FileName.IndexOf('.'))}");

        bool exist = file != null;
        if (!exist)
        {
            UnityHelper.Error_H($"OptionFile GetFileData Exist Error");
            return new Dictionary<string, string>();
        }

        string json = file.text;
        return CSharpHelper.DeserializeObject<Dictionary<string, string>>(json);
    }

    public virtual T Read<T>(string key) where T : new()
    {
        Dictionary<string, string> data = GetFileData();
        if (data.ContainsKey(key))
        {
            string valueData = data[key];
            T value = CSharpHelper.DeserializeObject<T>(valueData);
            return value;
        }
        else
        {
            return default;
        }
    }
    public string Read(string key)
    {
        Dictionary<string, string> data = GetFileData();
        if (data.ContainsKey(key))
        {
            string valueData = CSharpHelper.RemoveSemi(data[key]);
            return valueData;
        }
        else
        {
            return default;
        }
    }

    public virtual void Remove(string key)
    {
        string file = GetFile();

        Dictionary<string, string> data = GetFileData();

        if (data.ContainsKey(key))
        {
            data.Remove(key);
        }

        string text = CSharpHelper.SerializeObject(data);
        FileHelper.Write(file, text, true);
    }

    public List<string> Keys()
    {
        List<string> keys = new List<string>();
        Dictionary<string, string> data = GetFileData();
        foreach (string key in data.Keys)
        {
            keys.Add(key);
        }
        return keys;
    }

  
}
