using System;

public abstract class CharacterSpawn
{
    public abstract int Count { get; }

    public abstract Character Spawn(Character prefab);
    public abstract void Clear();
}
