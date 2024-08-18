using System;

public abstract class EnemySpawnAction
{
    public Action<Entity> onDead;
    public Action<Entity, Entity, object, BBNumber> onTakeDamage;
    public abstract int Count { get; }
    public abstract void Play();
    public abstract void Update();
    public abstract void Stop();
    public abstract void Clear();
}
