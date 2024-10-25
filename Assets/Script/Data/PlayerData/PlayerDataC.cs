using System;
using System.Collections.Generic;

public abstract class PlayerDataC<T>
{
    protected List<T> datas = new List<T>();
    public List<T> Gets() => datas;
    public abstract T Get(object key);
    public void Sets(List<T> _datas)
    {
        if(_datas == null)
            datas.Clear();
        else
            this.datas = _datas;
    }
    public abstract void DbGets(Action<List<T>> result);
    public abstract List<Stat> GetStats();
    public virtual void InitialData() { }
}
