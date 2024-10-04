using System;

[Serializable] // ItemTable
public class ItemTableData
{
	public string itemCode { get; set; }
	public int itemType { get; set; }
	public string tipName { get; set; }
}

[Serializable] // CharacterTable
public class CharacterTableData
{
	public string characterCode { get; set; }
	public int tribeType { get; set; }
	public int grade { get; set; }
	public string tipName { get; set; }
}

[Serializable] // FormulaTable
public class FormulaTableData
{
	public string formulaCode { get; set; }
	public string fM { get; set; }
	public string tipName { get; set; }
}

[Serializable] // GameDefineTable
public class GameDefineTableData
{
	public string gameDefineCode { get; set; }
	public string value { get; set; }
	public string tipName { get; set; }
}