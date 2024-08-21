using System;
using UnityEngine;

public abstract class PlayerSpawnAction
{
    public Action<Entity> onDead;
    public Action<Entity, Entity, object, BBNumber> onTakeDamage;
    public abstract PlayerController Player { get; }
    public abstract void Play(Vector3 pos);
    public abstract void Stop();
    public abstract void Clear();
}
