using System;
using System.Collections.Generic;
using Shared.CSharp;
using Shared.DTOs.Table;

public class TableManager
{
    public void Initialize()
    {
    }
    public void DbGets(Action callback = null)
    {
        callback?.Invoke();
        return;
        
        // checksum을 확인하고 업데이트 해야함
        // 업데이트 할 때 캐시 삭제
        // 캐시 삭제 후 다시 로드

        TableGetsRequest request = new TableGetsRequest();
        request.TableUpdateAt = GetLastTableUpdatedAt();
        

        Managers.Web.SendPostRequest<TableGetsResponse>("table/gets", request, (_result) =>
        {
            SaveTableData(_result.Datas, _result.UpdateAt);

            if (callback != null)
                callback.Invoke();
        });
    }

    /// <summary>
    /// 로컬에 저장된 테이블 최종 업데이트 날짜 가져오기
    /// </summary>
    public DateTime GetLastTableUpdatedAt()
    {
        string saved = PlayerPrefsHelper.GetString_H(PlayerPrefsKey.table_updated_at);
        return string.IsNullOrEmpty(saved) ? DateTime.MinValue : DateTime.Parse(saved);
    }

    /// <summary>
    /// 서버에서 받아온 테이블 데이터 저장
    /// </summary>
    public void SaveTableData(List<TableGetsData<object>> datas, DateTime updatedAt)
    {
        string json = datas.SerializeObject_H();

        List<TableGetsData<object>> saveDatas = GetTableDatas(json);
        List<TableGetsData<object>> loadDatas = LoadTableDataList();

        if (loadDatas == null)
            loadDatas = new List<TableGetsData<object>>();

        for (int i = 0; i < saveDatas.Count; i++)
        {
            TableGetsData<object> saveData = saveDatas[i];
            TableGetsData<object> loadData = loadDatas.Find(data => data.TableName == saveData.TableName);

            if (loadData == null)
            {
                loadDatas.Add(saveData);
            }
            else
            {
                loadData = saveData;
            }
        }

        string loadJson = loadDatas.SerializeObject_H();

        PlayerPrefsHelper.Set_H(PlayerPrefsKey.table_data, loadJson);
        PlayerPrefsHelper.Set_H(PlayerPrefsKey.table_updated_at, updatedAt.ToString_H());
    }

    /// <summary>
    /// 로컬에서 테이블 데이터 가져오기 (게임 실행 중 사용)
    /// </summary>
    public string LoadTableData()
    {
        return PlayerPrefsHelper.GetString_H(PlayerPrefsKey.table_data);
    }

    public List<TableGetsData<object>> LoadTableDataList()
    {
        string json = LoadTableData();
        return GetTableDatas(json);
    }

    public List<TableGetsData<object>> GetTableDatas(string json)
    {
        return CSharpHelper.DeserializeObject<List<TableGetsData<object>>>(json);
    }

    public T GetTableData<T>()
    {
        List<TableGetsData<object>> loadDatas = LoadTableDataList();

        if (loadDatas == null)
            loadDatas = new List<TableGetsData<object>>();

        string typeName = typeof(T).IsGenericType ? typeof(T).GetGenericArguments()[0].Name : typeof(T).Name;
        TableGetsData<object> loadData = loadDatas.Find(data => data.TableName == typeName);

        if (loadData == null)
            return default(T);

        string json = CSharpHelper.SerializeObject(loadData.TableDatas);

        return CSharpHelper.DeserializeObject<T>(json);
    }
}