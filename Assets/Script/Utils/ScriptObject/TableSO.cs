using System;
using UnityEngine;

public class TableSO : ScriptableObject, ICloneable
{
    [SerializeField]
    private Sprite icon;
    public string codeName;
    [SerializeField]
    private string displayName;
    [SerializeField]
    private string description;

    public Sprite Icon => icon;
    public string DisplayName => displayName;
    public virtual string Description => description;

    public virtual object Clone() => Instantiate(this);
    protected virtual object TableDataObject => default;
}
