using System;

[Serializable] // ItemTable
public class ItemTableData
{
	public string itemCode { get; set; }
	public int itemType { get; set; }
	public string tipName { get; set; }
}

[Serializable] // SkillTable
public class SkillTableData
{
	public string skillCode { get; set; }
	public int skillType { get; set; }
	public string tipName { get; set; }
}