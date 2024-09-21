using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Table<T>
{
    protected List<T> datas = new List<T>();
    public List<T> Gets() => datas;
    public void Push(List<T> pushDatas) => datas = pushDatas;
    public abstract void DbGets(Action<List<T>> result);
}
