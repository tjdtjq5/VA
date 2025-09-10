using System;
using System.Collections.Generic;
using Shared.Enums;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "IdentifiedObject/Skill")]
public class Skill : IdentifiedObject, IRandom
{
    public Action<Skill> OnRemove;
    public Action<Skill> OnEnd;
    public Dictionary<SkillBehaviour, Action> OnBehaviourEnd = new Dictionary<SkillBehaviour, Action>();

    public bool IsUse { get; set; }
    public bool IsView => isView;
    public bool IsPlus => isPlus;
    public Grade Grade => skillGrade;
    public List<Skill> RemoveSkills => removeSkills;
    public List<Skill> ConditionSkills => conditionSkills;
    public List<SkillBehaviour> Behaviours => behaviours;
    public List<WordTip> WordTips => wordTips;
    public List<SkillDeckType> SkillDeckTypes => skillDeckTypes;

    public int GessoPrice
    {
        get
        {
            switch (Grade)
            {
                case Grade.D:
                    return 200;
                case Grade.C:
                    return 250;
                case Grade.B:
                    return 300;
                case Grade.A:
                    return 350;
                case Grade.S:
                    return 400;
                case Grade.SS:
                    return 450;
                case Grade.SSS:
                    return 500;
                default:
                    return 100;
            }
        }
    }
    
    [SerializeField] bool isView;
    [SerializeField] bool isPlus;
    [SerializeField] Grade skillGrade;
    [SerializeField] List<WordTip> wordTips = new();
    [SerializeField] private List<SkillDeckType> skillDeckTypes = new();
    [SerializeReference] private List<SkillBehaviour> behaviours = new List<SkillBehaviour>();
    
    [SerializeField] private bool isEndRemove;
    [SerializeField] private bool isAllClearApply; // 모든 행동이 끝나면 모든 행동의 적용 카운트를 초기화 / 아니면 행동 적용 카운트만 초기화
    
    [SerializeField] List<Skill> removeSkills = new(); // RemoveSkill : When you acquire that skill, the RemoveSkill is removed 
    [SerializeField] List<Skill> conditionSkills = new(); // ConditionSkill : If the character does not have the ConditionSkill, the skill will not appear.

    private Character _owner;
    private object _cause;
    private int _skillEndCount = 0;


    public bool StartSkill(SkillBehaviour behaviour, Character owner, object cause)
    {
        if (!behaviour.IsUse)
        {
            behaviour.Initialize(this);

            if (!behaviour.IsTimingConditionCheck(owner, cause))
                return false;

            if (!behaviour.IsOperate())
                return false;

            if (!behaviour.IsHpUnder(owner))
                return false;
        }
        
        if (!behaviours.Contains(behaviour))
            return false;

        behaviour.IsUse = true;
        IsUse = true;
        _owner = owner;
        _cause = cause;
        
        owner?.CharacterSkill.OnSkillPlay?.Invoke(this);
        
        behaviour.OnEnd = BehaviourEnd;
        
        behaviour.AddApply();
        behaviour.Start(owner, cause);

        return true;
    }
    void BehaviourEnd(SkillBehaviour behaviour, Character owner, object cause)
    {
        if (OnBehaviourEnd.ContainsKey(behaviour))
        {
            OnBehaviourEnd[behaviour]?.Invoke();
            OnBehaviourEnd[behaviour] = null;
        }
        
        if (!behaviour.CheckApply())
        {
            behaviour.IsUse = false;

            if (isAllClearApply)
            {
                _skillEndCount++;

                if (_skillEndCount < behaviours.Count)
                    return;

                for (int i = 0; i < behaviours.Count; i++)
                    behaviours[i].ClearApply();
            }
            else
            {
                behaviour.ClearApply();
            }

            IsUse = false;
            _skillEndCount = 0;

            if (isEndRemove)
                owner.CharacterSkill.SkillRemove(this);
            
            OnEnd?.Invoke(this);
        }
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < behaviours.Count; i++)
        {
            if (behaviours[i].IsUse)
                behaviours[i].FixedUpdate();
        }
    }
    public void ForceEnd()
    {
        if (_owner)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].EndApply();
                BehaviourEnd(behaviours[i], _owner, _cause);
            }
        }
    }

    public Dictionary<string, string> StringsByKeyword(string preface)
    {
        var stringsByKeyword = new Dictionary<string, string>()
        {
            {"BehavioursCount", behaviours.Count.ToString()}
        };
        
        for (int i = 0; i < behaviours.Count; i++)
            stringsByKeyword.AddRange(behaviours[i].StringsByKeyword($"b{i}."));

        return stringsByKeyword;
    }
    
    public override string Description
    {
        get
        {
            string description = base.Description;

            var stringsByKeyword = StringsByKeyword("");

            description = TextReplacer.Replace(description, stringsByKeyword);
            description = description.Replace("\\n", "\n");

            return description;
        }
    }
    
#if UNITY_EDITOR
    [Button]
    public void LogDescription()
    {
        var stringsByKeyword = StringsByKeyword("");

        foreach (var pair in stringsByKeyword)
        {
            Debug.Log($"Key : {pair.Key}   Value : {pair.Value}");
        }
        
        Debug.Log(Description);
    }
#endif

    public void Load(SkillSaveData saveData)
    {
        this.codeName = saveData.CodeName;
    }
}

[System.Serializable]
public class SkillSaveData
{
    public string CodeName;
    public bool IsUse;

    public SkillSaveData(Skill skill)
    {
        if (skill == null)
        {
            this.CodeName = string.Empty;
            this.IsUse = false;
            return;
        }
        
        this.CodeName = skill.CodeName;
    }
}

public enum SkillDeckType
{
    None,
    Slash,
    Thunder,
    Fire,
    Gas,
    NomalAttack,
    SequenceAttack,
}