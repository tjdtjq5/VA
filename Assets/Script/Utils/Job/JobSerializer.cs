﻿using System;
using System.Collections.Generic;

public class JobSerializer
{
    JobTimer _timer = new JobTimer();
    Queue<IJob> _jobQueue = new Queue<IJob>();
    object _lock = new object();
    bool _flush = false;

    public int Count
    {
        get
        {
            return _jobQueue.Count;
        }
    }

    public IJob PushAfter(int tickAfter, Action action) { return PushAfter(tickAfter, new Job(action)); }
    public IJob PushAfter<T1>(int tickAfter, Action<T1> action, T1 t1) { return PushAfter(tickAfter, new Job<T1>(action, t1)); }
    public IJob PushAfter<T1, T2>(int tickAfter, Action<T1, T2> action, T1 t1, T2 t2) { return PushAfter(tickAfter, new Job<T1, T2>(action, t1, t2)); }
    public IJob PushAfter<T1, T2, T3>(int tickAfter, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) { return PushAfter(tickAfter, new Job<T1, T2, T3>(action, t1, t2, t3)); }
    public IJob PushAfter<T1, T2, T3, T4>(int tickAfter, Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4) { return PushAfter(tickAfter, new Job<T1, T2, T3, T4>(action, t1, t2, t3, t4)); }
    public IJob PushAfter<T1, T2, T3, T4, T5>(int tickAfter, Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) { return PushAfter(tickAfter, new Job<T1, T2, T3, T4, T5>(action, t1, t2, t3, t4, t5)); }

    public IJob PushAfter(int tickAfter, IJob job)
    {
        _timer.Push(job, tickAfter);
        return job;
    }

    public void Push(Action action) { Push(new Job(action)); }
    public void Push<T1>(Action<T1> action, T1 t1) { Push(new Job<T1>(action, t1)); }
    public void Push<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2) { Push(new Job<T1, T2>(action, t1, t2)); }
    public void Push<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3) { Push(new Job<T1, T2, T3>(action, t1, t2, t3)); }
    public void Push<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4) { Push(new Job<T1, T2, T3, T4>(action, t1, t2, t3, t4)); }
    public void Push<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) { Push(new Job<T1, T2, T3, T4, T5>(action, t1, t2, t3, t4, t5)); }

    public void Push(IJob job)
    {
        lock (_lock)
        {
            _jobQueue.Enqueue(job);
        }
    }

    public void Flush()
    {
        _timer.Flush();

        while (true)
        {
            IJob job = Pop();
            if (job == null)
                return;

            job.Execute();
        }
    }

    public IJob Pop()
    {
        lock (_lock)
        {
            if (_jobQueue.Count == 0)
            {
                _flush = false;
                return null;
            }
            return _jobQueue.Dequeue();
        }
    }
}
