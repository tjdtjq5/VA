using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    public Action OnSequencerEnd; 
    public List<SequenceAction> SequencesActions = new();

    private void Awake()
    {
        for (int i = 0; i < SequencesActions.Count; i++)
            SequencesActions[i].Initialize(this.gameObject);
    }

    public void Excute()
    {
        StartCoroutine(ExcuteSequenceCoroutine());
    }

    private IEnumerator ExcuteSequenceCoroutine()
    {
        for (int i = 0; i < SequencesActions.Count; i++)
        {
            yield return StartCoroutine(SequencesActions[i].StartSequence(this));
        }
        
        OnSequencerEnd?.Invoke();
    }
}
