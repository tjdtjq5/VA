using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialContents : MonoBehaviour
{

    protected int Index => _index;

    private int _index;

    public virtual void Initialize() { }

    public virtual void Set(int index)
    {
        _index = index;
    }

    public void Left()
    {
        if (_index > 0)
        {
            _index--;
            Set(_index);
        }
    }

    public void Right()
    {
        _index++;
        Set(_index);
    }

    public virtual void Clear() { }
}
