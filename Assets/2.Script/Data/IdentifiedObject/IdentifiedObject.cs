using System.Collections;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class IdentifiedObject : ScriptableObject, ICloneable, IEqualityComparer<IdentifiedObject>
{
    [SerializeField]
    protected string codeName;
    [SerializeField]
    private int id = -1;
    [SerializeField]
    public Sprite icon;
    [SerializeField]
    protected string displayName;
    [SerializeField, TextArea(1, 10)]
    public string description;

    public Sprite Icon { get => icon; set => icon = value; }
    public int ID => id;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public virtual string Description => description;
    
    public virtual object Clone() => Instantiate(this);
    
    public bool Equals(IdentifiedObject x, IdentifiedObject y)
    {
        return x != null && y != null && x.CodeName == y.CodeName;
    }
    public int GetHashCode(IdentifiedObject obj)
    {
        return $"{CodeName}".GetHashCode();
    }
}
