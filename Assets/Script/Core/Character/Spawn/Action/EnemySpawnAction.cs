using System;

public abstract class EnemySpawnAction
{
    protected PlayerController player;
    public Action<Entity> onDead;
    public Action<Entity, Entity, object, BBNumber> onTakeDamage;
    public abstract int Count { get; }
    public abstract void Play(PlayerController player);
    public abstract void Update();
    public abstract void Stop();
    public abstract void Clear();
}
