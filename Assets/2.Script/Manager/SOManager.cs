using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Enums;
using Unity.VisualScripting;
using UnityEngine;

public class SOManager
{
    private Dictionary<string, Stat> _statCache = new Dictionary<string, Stat>();
    private Dictionary<string, Buff> _buffCache = new Dictionary<string, Buff>();
    private Dictionary<string, Skill> _skillCache = new Dictionary<string, Skill>();
    private Dictionary<string, Item> _itemCache = new Dictionary<string, Item>();
    private Dictionary<string, Equip> _equipCache = new Dictionary<string, Equip>();
    private List<Buff> _buffs;
    private List<Skill> _skills;

    public Stat GetStat(string code)
    {
        if (!_statCache.TryGetValue(code, out Stat stat))
        {
            string path = DefinePath.StatSOResourcesPath(code);
            stat = Managers.Resources.Load<Stat>(path);
            _statCache[code] = stat;
        }
        return (Stat)stat.Clone();
    }

    public Buff GetBuff(string code)
    {
        try
        {
            if (!_buffCache.TryGetValue(code, out Buff buff))
            {
                if (_buffs == null)
                {
                    string path = DefinePath.BuffSOResourcesPath();
                    _buffs = Resources.LoadAll<Buff>(path).ToList();
                }
                buff = _buffs.Find(s => s.CodeName.Equals(code));
                _buffCache[code] = buff;
            }
            return (Buff)buff.Clone();
        }
        catch (Exception e)
        {
            UnityHelper.Error_H($"Error loading buff resource: {e.Message}\nCode : '{code}'");
            throw;
        }
    }

    public Skill GetSkill(string code)
    {
        try
        {
            if (!_skillCache.TryGetValue(code, out Skill skill))
            {
                if (_skills == null)
                {
                    string path = DefinePath.SkillSOResourcesPath();
                    _skills = Resources.LoadAll<Skill>(path).ToList();
                }
                skill = _skills.Find(s => s.CodeName.Equals(code));
                _skillCache[code] = skill;
            }
            return (Skill)skill.Clone();
        }
        catch (Exception e)
        {
            UnityHelper.Error_H($"Error loading skill resource: {e.Message}\nCode : '{code}'");
            throw;
        }
    }

    public Item GetItem(string code)
    {
        if (!_itemCache.TryGetValue(code, out Item item))
        {
            string path = DefinePath.ItemSOResourcesPath(code);
            item = Managers.Resources.Load<Item>(path);
            _itemCache[code] = item;
        }
        return item;
    }

    public Equip GetEquip(string code)
    {
        if (!_equipCache.TryGetValue(code, out Equip equip))
        {
            string path = DefinePath.EquipSOResourcesPath(code);
            equip = Managers.Resources.Load<Equip>(path);
            _equipCache[code] = equip;
        }
        return equip;
    }
}

public class NodeTreeManager
{
    private Dictionary<string, DungeonTree> _dungeonTreeCache = new Dictionary<string, DungeonTree>();
    private Dictionary<string, ResearchTree> _researchTreeCache = new Dictionary<string, ResearchTree>();

    public DungeonTree GetDungeonTree(string code)
    {
        if (!_dungeonTreeCache.TryGetValue(code, out DungeonTree tree))
        {
            string path = DefinePath.NodeResourcesPath("Dungeon", code);
            tree = Managers.Resources.Load<DungeonTree>(path);
            _dungeonTreeCache[code] = tree;
        }
        return (DungeonTree)tree.Clone();
    }

    public ResearchTree GetResearchTree(string code)
    {
        if (!_researchTreeCache.TryGetValue(code, out ResearchTree tree))
        {
            string path = DefinePath.NodeResourcesPath("Research", code);
            tree = Managers.Resources.Load<ResearchTree>(path);
            _researchTreeCache[code] = tree;
        }
        return tree;
    }
}
