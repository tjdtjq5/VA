using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SequenceAction : ScriptableObject
{
    [SerializeField] protected bool isWait = true;
    public abstract IEnumerator StartSequence(Sequencer context);
    public virtual void Initialize(GameObject obj) { }
}