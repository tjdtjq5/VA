using System.Linq;

public class CharacterSO : TableSO
{
    public PlayerController prefab;

    CharacterTableData tableData;
    public CharacterTableData TableData
    {
        get
        {
            if(tableData == null)
                tableData = (CharacterTableData)TableDataObject;

            return tableData;
        }
    }
    protected override object TableDataObject => Managers.Table.CharacterTable.Gets().Where(c => c.characterCode.Equals(codeName)).FirstOrDefault();
}
