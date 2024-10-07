using System;
using System.Collections.Generic;

public abstract class PlayerDataC<T>
{
    protected List<T> datas = new List<T>();
    public List<T> Gets() => datas;
    public void Sets(List<T> datas) => this.datas = datas;  
    public abstract void DbGets(Action<List<T>> result);
    public abstract List<Stat> GetStats();
}
