using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "IODatabase")]
public class IODatabase : ScriptableObject
{
    [SerializeField]
    private List<IdentifiedObject> datas = new();

    public IReadOnlyList<IdentifiedObject> Datas => datas;
    public int Count => datas.Count;

    public IdentifiedObject this[int index] => datas[index];

    private void SetID(IdentifiedObject target, int id)
    {
        // IdentifiedObject�� id ���� ã�ƿ�
        // BindingFlags.NonPublic = �����ڰ� public�� �ƴϿ�����, BindingFlags.Instance = static type�� �ƴϿ�����
        var field = typeof(IdentifiedObject).GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
        // ������ id ���� ������ ���� target�� id ���� ���� ������
        field.SetValue(target, id);
        // Serialize ����(���⼭�� id ����)�� code�󿡼� �������� ��� EditorUtility.SetDirty�� ���ؼ� Serailize ������ �����Ǿ����� Unity�� �˷������
        // �׷��� ������ ������ ���� �ݿ����� �ʰ� ���� ������ ���ư�
        // ���⼭�� ���� �����Ǿ��ٰ� Unity�� �˷��ٻ�, ������ ���� ����ɷ��� Editor Code���� ApplyModifiedProperties �Լ� Ȥ��
        // ������Ʈ ��ü�� �����ϴ� AssetDatabase.SaveAssets �Լ��� ȣ��Ǿ����
        // ���⼭�� ���߿� �ٸ� ������ AssetDatabase.SaveAssets�� ȣ�� �� ���̱� ���� �ۼ����� ����.
#if UNITY_EDITOR
    EditorUtility.SetDirty(target);
#endif
    }

    // index ������ IdentifiedObjects�� id�� �缳����
    private void ReorderDatas()
    {
        var field = typeof(IdentifiedObject).GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < datas.Count; i++)
        {
            field.SetValue(datas[i], i);
#if UNITY_EDITOR
        EditorUtility.SetDirty(datas[i]);
#endif
        }
    }

    public void Add(IdentifiedObject newData)
    {
        datas.Add(newData);
        SetID(newData, datas.Count - 1);
    }

    public void Remove(IdentifiedObject data)
    {
        datas.Remove(data);
        ReorderDatas();
    }

    public IdentifiedObject GetDataByID(int id) => datas[id];
    public T GetDataByID<T>(int id) where T : IdentifiedObject => GetDataByID(id) as T;

    public IdentifiedObject GetDataCodeName(string codeName) => datas.Find(item => item.CodeName == codeName);
    public T GetDataCodeName<T>(string codeName) where T : IdentifiedObject => GetDataCodeName(codeName) as T;

    public bool Contains(IdentifiedObject item) => datas.Contains(item);

    // Data�� CodeName�� �������� ������������ ������
    public void SortByCodeName()
    {
        datas.Sort((x, y) => x.CodeName.CompareTo(y.CodeName));
        ReorderDatas();
    }
}
