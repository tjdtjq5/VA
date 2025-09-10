using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sequence/Wait", fileName = "Wait")]
public class SequenceActionWait : SequenceAction
{
    [SerializeField] private float waitTime;
    
    public override IEnumerator StartSequence(Sequencer context)
    {
        if(isWait)
            yield return new WaitForSeconds(waitTime);
    }
}