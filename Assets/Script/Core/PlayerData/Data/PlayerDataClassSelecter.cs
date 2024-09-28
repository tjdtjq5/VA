using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataClassSelecter : ScriptableObject, ICloneable
{
    [SerializeReference, SubclassSelector]
    public IPlayerData playerDataClass;

    public virtual object Clone() => Instantiate(this);
}
