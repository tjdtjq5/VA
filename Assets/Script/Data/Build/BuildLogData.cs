using System;
using System.Collections.Generic;

public class BuildLogDatas
{
    public List<BuildLogData> Datas = new List<BuildLogData>();
}
public class BuildLogData
{
    public string Title {  get; set; }
    public int VersionFirst { get; set; }
    public int VersionSecond { get; set; }
    public int VersionThird { get; set; }
    public int BundleCode { get; set; }
    public ServerUrlType ServerUrlType { get; set; }
    public DateTime BuildTime { get; set; }

    public BuildLogData()
    {

    }

    public BuildLogData(string title, string version, int bundleCode, ServerUrlType serverUrlType)
    {
        this.Title = title;
        this.BundleCode = bundleCode;
        this.ServerUrlType = serverUrlType;
        this.BuildTime = DateTime.Now;

        VersionFirst = UnityHelper.GetVersionValue(version, 0);
        VersionSecond = UnityHelper.GetVersionValue(version, 1);
        VersionThird = UnityHelper.GetVersionValue(version, 2);
    }

    public BuildLogData(string title, int versionFirst, int versionSecond, int versionThird, int bundleCode, ServerUrlType serverUrlType)
    {
        this.Title = title;
        this.BundleCode = bundleCode;
        this.ServerUrlType = serverUrlType;
        this.BuildTime = DateTime.Now;

        VersionFirst = versionFirst;
        VersionSecond = versionSecond;
        VersionThird = versionThird;
    }

    public override string ToString()
    {
        string versionCode = UnityHelper.GetVersionCode(VersionFirst, VersionSecond, VersionThird);
        return $"[{BuildTime.ToString()}] : {Title} {versionCode} ({BundleCode})     [{ServerUrlType}]";
    }
}
