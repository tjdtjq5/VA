public abstract class EnemySpawnAction
{
    public abstract int Count { get; }
    public abstract void Play();
    public abstract void Update();
    public abstract void Stop();
    public abstract void Clear();
}
