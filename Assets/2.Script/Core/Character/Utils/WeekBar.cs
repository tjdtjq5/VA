using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekBar : MonoBehaviour
{
    public Action OnAllWeekCrash;
    public PuzzleType CurrentWeek => _weekDatas[_weekIndex];
    public bool IsAllWeekCrash => _weekIndex >= _weekDatas.Length;
    
    [SerializeField] private Transform _contents;

    private PuzzleType[] _weekDatas;
    private int _weekIndex;
    private List<Week> _weeks = new();

    private readonly string _weekPrefab = "Prefab/Character/Week";

    public void Initialize(int weekCount)
    {
        this._weekDatas = new PuzzleType[weekCount];
        this._weekIndex = 0;

        for (int i = 0; i < _weekDatas.Length; i++)
        {
            _weekDatas[i] = GetRandomWeek();
        }
        
        for (int i = 0; i < _weeks.Count; i++)
        {
            _weeks[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _weekDatas.Length; i++)
        {
            Week week = null;
            
            if (i < _weeks.Count)
            {
                week = _weeks[i];
                week.gameObject.SetActive(true);
            }
            else
            {
                week = Managers.Resources.Instantiate<Week>(_weekPrefab, _contents);
                _weeks.Add(week);
            }
            
            week.UISet(_weekDatas[i]);
        }
    }
    public void Conquer(Character takeCharacter, PuzzleType conquerType)
    {
        if (_weekDatas.Length <= 0 || _weekIndex >= _weekDatas.Length)
            return;

        if (_weekDatas[_weekIndex] == conquerType)
        {
            _weeks[_weekIndex].Crash();
            _weekIndex++;

            if (_weekIndex >= _weekDatas.Length)
            {
                OnAllWeekCrash?.Invoke();
                takeCharacter?.OnStunSuccess?.Invoke();
            }
        }
    }
    public void Conquer(Character takeCharacter)
    {
        if (_weekDatas.Length <= 0 || _weekIndex >= _weekDatas.Length)
            return;

        _weeks[_weekIndex].Crash();
        _weekIndex++;

        if (_weekIndex >= _weekDatas.Length)
        {
            OnAllWeekCrash?.Invoke();
            takeCharacter?.OnStunSuccess?.Invoke();
        }
    }
    private PuzzleType GetRandomWeek()
    {
        int r = (int)UnityHelper.Random_H(1, 5);
        return (PuzzleType)r;
    }
    public void ChangeWeek(int weekIndex, PuzzleType puzzleType)
    {
        if (_weekDatas.Length <= 0 || _weekIndex >= _weekDatas.Length)
            return;

        if(_weeks[weekIndex].IsCrash)
            return;

        _weekDatas[weekIndex] = puzzleType;
        _weeks[weekIndex].UISet(puzzleType);
    }
}
