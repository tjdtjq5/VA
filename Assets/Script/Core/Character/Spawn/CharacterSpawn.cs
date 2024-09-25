using System;
using UnityEngine;

public abstract class CharacterSpawn
{
    public abstract int Count { get; }

    public abstract Character Spawn(Character prefab, Vector3 pos);
    public abstract void Clear();
    public abstract void Clear(Character character);
}
