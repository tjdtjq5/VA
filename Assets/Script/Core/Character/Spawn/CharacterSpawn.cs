using System;

public abstract class CharacterSpawn
{
    public abstract int Count { get; }
    public Action<Entity> onDead;

    public abstract Character Spawn(Character prefab);
    public abstract void OnDead(Entity entity);
    public abstract void Clear();
}
