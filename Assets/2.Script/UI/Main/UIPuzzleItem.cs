using System;
using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using UnityEngine;

public class UIPuzzleItem : UIFrame
{
    protected override void Initialize()
    {
        base.Initialize();
    }

    public void Initialize(UIPuzzle puzzle)
    {
        this._puzzle = puzzle;
    }

    [SerializeField] private Transform contents;
    
    private readonly string _lightningItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemLightning";
    private readonly string _hellFireItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemHellFire";
    private readonly string _gasItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemGas";
    private readonly string _slashItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemSlash";
    private readonly string _iceThornItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemIceThorn";
    private readonly string _bloodItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemBloodBlade";
    private readonly string _missileItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemMissile";
    private readonly string _prismItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemPrism";
    private readonly string _waveItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemWave";
    private readonly string _windItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemWind";
    private readonly string _shootingStarItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemShootingStar";
    private readonly string _hpRecoveryItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemHPRecovery";
    private readonly string _atkItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemAtk_20";
    private readonly string _shieldItemPath = "Prefab/InGame/PuzzleItem/PuzzleItemShield";
    
    private UIPuzzle _puzzle;
    private Dictionary<Vector2Int, PuzzleItem> _puzzleItems = new Dictionary<Vector2Int, PuzzleItem>();

    public void RandomLightningSpawn(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _lightningItemPath);
    }
    public void RandomHellFire(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _hellFireItemPath);
    }
    public void RandomGas(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _gasItemPath);
    }
    public void RandomSlash(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _slashItemPath);
    }
    public void RandomIceThorn(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _iceThornItemPath);
    }
    public void RandomBloodBlade(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _bloodItemPath);
    }
    public void RandomMissile(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _missileItemPath);
    }
    public void RandomPrism(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _prismItemPath);
    }
    public void RandomWave(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _waveItemPath);
    }
    public void RandomWind(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _windItemPath);
    }
    public void RandomShootingStar(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _shootingStarItemPath);
    }
    public void RandomHpRecovery(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _hpRecoveryItemPath);
    }
    public void RandomAtk(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _atkItemPath);
    }
    public void RandomShield(List<Vector2Int> points)
    {
        InstanceRandomItem(points, _shieldItemPath);
    }
    public void RandomItem(List<Vector2Int> points, PuzzleItem puzzleItem)
    {
        InstanceRandomItem(points, puzzleItem);
    }

    private void InstanceRandomItem(List<Vector2Int> points, string itemPath)
    {
        InstanceRandomItem(points, Managers.Resources.Load<PuzzleItem>(itemPath));
    }
    
    private void InstanceRandomItem(List<Vector2Int> points, PuzzleItem item)
    {
        List<Vector2Int> keys = new List<Vector2Int>(_puzzleItems.Keys);
        
        for (int i = 0; i < keys.Count; i++)
        {
            points.Remove(keys[i]);
        }

        if (points.Count <= 0)
            return;
        
        int rIndex = (int)UnityHelper.Random_H(0, points.Count);
        Vector2Int point = points[rIndex];
        PuzzleItem puzzleItem =  Managers.Resources.Instantiate<PuzzleItem>(item, contents);
        puzzleItem.transform.localScale = Vector3.one;
        puzzleItem.Open();

        _puzzleItems.TryAdd_H(point, puzzleItem, true);
        
        _puzzleItems[point].transform.position = _puzzle.GetPuzzlePosition(point);
        _puzzle.GetPuzzle(point).ItemSwitch(true);

        _puzzle.GetPuzzle(point).OnUISet += PuzzleUISet;
    }

    public void PointUp(List<Vector2Int> points)
    {
        foreach (var puzzleItem in _puzzleItems)
        {
            if (points.Contains(puzzleItem.Key) && puzzleItem.Value != null)
            {
                puzzleItem.Value.Use();
                _puzzle.GetPuzzle(puzzleItem.Key).ItemSwitch(false);
                _puzzle.GetPuzzle(puzzleItem.Key).OnUISet -= PuzzleUISet;
            }
        }

        for (int i = 0; i < points.Count; i++)
        {
            if (_puzzleItems.ContainsKey(points[i]))
                _puzzleItems.Remove(points[i]);
        }
    }

    public int ItemCount(List<Vector2Int> points)
    {
        int count = 0;
        foreach (var puzzleItem in _puzzleItems)
        {
            if (points.Contains(puzzleItem.Key) && puzzleItem.Value != null)
            {
                count++;
            }
        }

        return count;
    }

    private void PuzzleUISet(PuzzleData puzzleData)
    {
        switch (puzzleData.puzzleType)
        {
            case PuzzleType.None:
                _puzzleItems[puzzleData.number].gameObject.SetActive(false);
                break;
            default:
                _puzzleItems[puzzleData.number].gameObject.SetActive(!puzzleData.isBlock);
                break;
        }
    }

    public void Clear()
    {
        foreach (var puzzleItem in _puzzleItems)
        {
            _puzzle.GetPuzzle(puzzleItem.Key).OnUISet -= PuzzleUISet;
            puzzleItem.Value.Clear();
        }
        
        _puzzleItems.Clear();
    }
}