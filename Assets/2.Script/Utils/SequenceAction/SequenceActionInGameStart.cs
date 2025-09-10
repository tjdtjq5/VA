using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sequence/InGameStart ", fileName = "InGameStart ")]
public class SequenceActionInGameStart : SequenceAction
{
    InGameManager _inGameManager;
    
    public override void Initialize(GameObject obj)
    {
        base.Initialize(obj);
        
        _inGameManager = obj.GetComponent<InGameManager>();
    }

    public override IEnumerator StartSequence(Sequencer context)
    {
        // InGameStart
        InGameStart inGameStart = Managers.Resources.Instantiate<InGameStart>("Prefab/UI/Main/InGame/InGameStart");
        inGameStart.Initialize(_inGameManager.DungeonTree);
        inGameStart.Play();

        float time = inGameStart.GetPlayAniLength();

        if (isWait)
            yield return new WaitForSeconds(time);
    }
}
