using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.CSharp;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    public Action<Skill> OnSkillPlay;
    
    public readonly Dictionary<SkillTiming, Action> OnSkillTimingEndDics = new();
    
    [SerializeField] List<Skill> overrideSkills = new();
    List<Skill> _skills = new();
    public List<Skill> Skills => _skills;
    
    private Character _character;
    private readonly Dictionary<SkillTiming,  Stack<SkillTimingStack>> _behaviourStack = new();
    private Dictionary<SkillDeckType, int> _skillDeckTypeScore = new();
    private readonly int _firstDeckScore = 100;
    private readonly int _secondDeckScore = 50;
    
    public void Initialize(Character character)
    {
        this._character = character;

        this._character.OnDead += (c) => StartSkillTiming(Skills, SkillTiming.Dead);
        this._character.OnStageStart += () => StartSkillTiming(Skills, SkillTiming.StageStart);
        this._character.OnStageEnd += () => StartSkillTiming(Skills, SkillTiming.StageEnd);
        this._character.OnBattleStart += () => StartSkillTiming(Skills, SkillTiming.BattleStart);
        this._character.OnBattleEnd += () => StartSkillTiming(Skills, SkillTiming.BattleEnd);
        this._character.OnTurnStart += (page) => StartSkillTiming(Skills, SkillTiming.TurnStart, page);
        this._character.OnTurnEnd += (page) => StartSkillTiming(Skills, SkillTiming.TurnEnd, page);
        this._character.OnCharacterActionStart += () => StartSkillTiming(Skills, SkillTiming.CharacterActionStart);
        this._character.OnCharacterActionEnd += () => StartSkillTiming(Skills, SkillTiming.CharacterActionEnd);
        this._character.OnPuzzleAttackStart += (puzzleAttackData) => StartSkillTiming(Skills, SkillTiming.PuzzleAttackStart, puzzleAttackData);
        this._character.OnPuzzleAttackEnd += (puzzleAttackData) => StartSkillTiming(Skills, SkillTiming.PuzzleAttackEnd, puzzleAttackData);
        this._character.OnTakeDamage += (damage, cause) => StartSkillTiming(Skills, SkillTiming.TakeDamage, cause);
        this._character.OnStunSuccess += () => StartSkillTiming(Skills, SkillTiming.StunSuccess);
        this._character.OnSequenceAttack += () => StartSkillTiming(Skills, SkillTiming.SequenceAttack);
        
        this._character.OnSlashAttack += (pageCount) => StartSkillTiming(Skills, SkillTiming.SlashAttack, pageCount);
        this._character.OnLightningAttack += (pageCount) => StartSkillTiming(Skills, SkillTiming.LightningAttack, pageCount);
        this._character.OnHellFireAttack += (pageCount) => StartSkillTiming(Skills, SkillTiming.HellFireAttack, pageCount);
        this._character.OnGasAttack += (pageCount) => StartSkillTiming(Skills, SkillTiming.GasAttack, pageCount);
        this._character.OnIceThornAttack += (pageCount) => StartSkillTiming(Skills, SkillTiming.IceThornAttack, pageCount);
        this._character.OnBurnAttack += (pageCount) => StartSkillTiming(Skills, SkillTiming.BurnAttack, pageCount);
        this._character.OnPoisonAttack += (pageCount) => StartSkillTiming(Skills, SkillTiming.PoisonAttack, pageCount);
        
        this._character.OnPerfect += () => StartSkillTiming(Skills, SkillTiming.Perfect);

        this._character.OnBattleStart += () => StartSkillTiming(Skills, SkillTiming.HpDecrease, this._character.HpPercent);
        this._character.OnHpDecrease += (hpPercent) => StartSkillTiming(Skills, SkillTiming.HpDecrease, hpPercent);
        
        this._character.OnApplyAttack += (CharacterApplyAttack caa) => StartSkillTiming(Skills, SkillTiming.ApplyAttack, caa);

        _skillDeckTypeScore.Clear();
        
        for (int i = 0; i < overrideSkills.Count; i++)
            PushSkill(overrideSkills[i]);

        OnSkillTimingEndDics.Clear();
        int timingLen = CSharpHelper.GetEnumLength<SkillTiming>();
        for (int i = 0; i < timingLen; i++)
        {
            OnSkillTimingEndDics.Add((SkillTiming)i, null);
        }
    }
    private void FixedUpdate()
    {
        if (!_character)
            return;

        if (_character.IsNotDetect)
            return;
        
        for (int i = 0; i < _skills.Count; i++)
        {
            _skills[i].FixedUpdate();
        }
    }
  
    public Skill PushSkill(Skill skill)
    {
        // SkillDeckType Score
        for (int i = 0; i < skill.SkillDeckTypes.Count; i++)
        {
            int score = i == 0 ? _firstDeckScore : _secondDeckScore;

            if (_skillDeckTypeScore.ContainsKey(skill.SkillDeckTypes[i]))
            {
                _skillDeckTypeScore[skill.SkillDeckTypes[i]] += score;
            }
            else
            {
                _skillDeckTypeScore.Add(skill.SkillDeckTypes[i], score);
            }
        }

        // Rmove
        List<Skill> removeSkills = skill.RemoveSkills;
        for (int i = 0; i < removeSkills.Count; i++)
            SkillCloneRemove(removeSkills[i]);
        
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].Equals(skill))
            {
                for (int j = 0; j < _skills[i].Behaviours.Count; j++)
                {
                    if (_skills[i].Behaviours[j].timing == SkillTiming.Instance)
                        _skills[i].StartSkill(_skills[i].Behaviours[j], _character, null);
                }
        
                return _skills[i];
            }
        }
        
        Skill skillClone = (Skill)skill.Clone();
        _skills.Add(skillClone);

        for (int i = 0; i < skillClone.Behaviours.Count; i++)
        {
            if (skillClone.Behaviours[i].timing == SkillTiming.Instance)
                skillClone.StartSkill(skillClone.Behaviours[i], _character, null);
        }
        
        return skillClone;
    }

    private void StartSkillTiming(List<Skill> skills, SkillTiming skillTiming, object cause = null)
    {
        if (!_behaviourStack.ContainsKey(skillTiming))
            _behaviourStack.Add(skillTiming, new Stack<SkillTimingStack>());
        
        for (int i = 0; i < skills.Count; i++)
        {
            for (int j = 0; j < skills[i].Behaviours.Count; j++)
            {
                if (skills[i].Behaviours[j].CheckApply() && skills[i].Behaviours[j].timing == skillTiming)
                {
                    _behaviourStack[skillTiming].Push(new SkillTimingStack() { Skill = skills[i], Behaviour = skills[i].Behaviours[j] , Cause = cause });
                }
            }
        }
        
        SkillTimingBehaviourPop(skillTiming);
    }
    private void SkillTimingBehaviourPop(SkillTiming timing)
    {
        if (_behaviourStack[timing].Count > 0)
        {
            SkillTimingStack stack = _behaviourStack[timing].Pop();
            
            stack.Skill.OnBehaviourEnd.TryAdd_H(stack.Behaviour, () =>
            {
                SkillTimingBehaviourPop(timing);
                
            }, true);

            if (!stack.Skill.StartSkill(stack.Behaviour, _character, stack.Cause))
            {
                SkillTimingBehaviourPop(timing);
            }
        }
        else
        {
            OnSkillTimingEndDics[timing]?.Invoke();
            OnSkillTimingEndDics[timing] = null;
        }
    }
    
    private struct SkillTimingStack
    {
        public Skill Skill;
        public SkillBehaviour Behaviour;
        public object Cause;
    }
    
    private void SkillAllEnd()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            _skills[i].ForceEnd();
        }
    }
    public void SkillCloneRemove(Skill skill)
    {
        Skill removeSkill = _skills.Find(s => s.CodeName.Equals(skill.CodeName));
        if (removeSkill)
        {
            removeSkill.OnRemove?.Invoke(removeSkill);
            _skills.Remove(removeSkill);
        }
    }
    public void SkillRemove(Skill skill)
    {
        skill.OnRemove?.Invoke(skill);
        _skills.Remove(skill);
    }
    public void SkillAllRemove()
    {
        SkillAllEnd();
        _skills.Clear();
    }
    public void Clear()
    {
        SkillAllRemove();
    }

    public List<Skill> GetCheckLearnSkills(List<Skill> skills)
    {
        List<Skill> result = new List<Skill>();
        
        // Player Have Skill Remove
        for (int i = 0; i < _skills.Count; i++)
            skills.RemoveAll(s => s.CodeName.Equals(_skills[i].CodeName));
        
        // Condition (Just Need Must One)
        for (int i = 0; i < skills.Count; i++)
        {
            List<Skill> conditionSkills = skills[i].ConditionSkills;
            bool isOkCondition = conditionSkills.Count <= 0;
            for (int j = 0; j < conditionSkills.Count; j++)
            {
                if (_skills.Exists(s => s.CodeName.Equals(conditionSkills[j].CodeName)))
                {
                    isOkCondition = true;
                    break;
                }
            }
            
            if (isOkCondition)
                result.Add(skills[i]);
        }
        
        // Ignore -> Remove
        for (int i = 0; i < _skills.Count; i++)
        {
            List<Skill> ignoreSkills = _skills[i].RemoveSkills;
            for (int j = 0; j < ignoreSkills.Count; j++)
                result.RemoveAll(s => s.CodeName.Equals(ignoreSkills[j].CodeName));
        }
        
        return result;
    }

    public SkillDeckType GetMainSkillDeckType()
    {
        if (_skillDeckTypeScore.Count <= 0)
            return SkillDeckType.None;

        return _skillDeckTypeScore.OrderByDescending(x => x.Value).First().Key;
    }
}

public enum SkillTiming
{
    None,
    Instance,
    StageStart,
    BattleStart, 
    TurnStart, // Turn Index
    CharacterActionStart, 
    PuzzleAttackStart, // Puzzle Color // Combo
    StageEnd,
    BattleEnd,
    TurnEnd, // Turn Index
    CharacterActionEnd, 
    PuzzleAttackEnd, // Puzzle Color // Combo
    TakeDamage,
    StunSuccess,
    SequenceAttack,
    SlashAttack,
    LightningAttack,
    HellFireAttack,
    GasAttack,
    IceThornAttack,
    BurnAttack,
    PoisonAttack,
    ApplyAttack, // TakeOwner
    Perfect,
    HpDecrease,
    Dead,
}