using System.Linq;

public class CharacterSO : TableSO
{
    public PlayerController prefab;

    public CharacterTableData TableData => (CharacterTableData)TableDataObject;
    protected override object TableDataObject => Managers.Table.CharacterTable.Gets().Where(c => c.characterCode.Equals(codeName)).FirstOrDefault();
}
