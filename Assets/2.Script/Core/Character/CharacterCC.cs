using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterCC : MonoBehaviour
{
    public bool IsStun { get; private set; }
    
    private Character _character;
    
    private readonly string _stunAnimationName = "Fainting";
    private readonly string _stunEffectPrefab = "Prefab/Effect/Universal/Fainting";
    private readonly string _brokenPrefabPath = "Prefab/Effect/InGame/Broken";

    private Poolable _stunPoolable;
    private Poolable _brokenPoolable;
    private readonly int _stunCount = 1;
    private int _currentStunCount;
    
    public void Initialize(Character character)
    {
        this._character = character;
    }
    
    public void SetStun()
    {
        if (this._character.Stats.shieldStat.Value > 0)
        this._character.Stats.shieldStat.DefaultValue -= this._character.Stats.shieldStat.Value;
        
        SetAniFainting();
        IsStun = true;

        _stunPoolable = Managers.Resources.Instantiate<Poolable>(_stunEffectPrefab);
        this._character.Attach(_stunPoolable, null);
        Vector3 stunPoolPos = this._character.transform.position;
        stunPoolPos.y += this._character.BoxPosY + this._character.BoxHeight * 0.5f + 0.2f;
        _stunPoolable.transform.position = stunPoolPos;

        _brokenPoolable = Managers.Resources.Instantiate<Poolable>(_brokenPrefabPath);
        this._character.Attach(_brokenPoolable, null);
        Vector3 brokenPoolPos = this._character.transform.position;
        brokenPoolPos.y += this._character.BoxPosY + this._character.BoxHeight * 0.5f + 1f;
        _brokenPoolable.transform.position = brokenPoolPos;

        _currentStunCount = 0;
    }
    public void SetAniFainting()
    {
        _character.SetAnimation(_stunAnimationName, true);
    }
    public bool AddStunCount()
    {
        _currentStunCount++;
        return _currentStunCount > _stunCount;
    }
    public void ClearStun()
    {
        if (_stunPoolable != null)
            Managers.Resources.Destroy(_stunPoolable.gameObject);
        if (_brokenPoolable != null)
            Managers.Resources.Destroy(_brokenPoolable.gameObject);
        IsStun = false;

        _character.SetIdle();
    }
    
    public void Clear()
    {
        if (_stunPoolable != null)
            Managers.Resources.Destroy(_stunPoolable.gameObject);
        if (_brokenPoolable != null)
            Managers.Resources.Destroy(_brokenPoolable.gameObject);
            
        IsStun = false;
    }
}
