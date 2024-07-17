using System.Collections.Generic;

public interface IFileTxt
{
    public string FileName { get; }


    string GetFile();
    Dictionary<string, string> GetFileData();

    public bool Exist();
    public T Read<T>(string key);
    public void Add(string key, object value);
    public void Remove(string key);
    public List<string> Keys();
}
