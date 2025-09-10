using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sequence/CameraInGame ", fileName = "CameraInGame ")]
public class SequenceActionCameraInGame : SequenceAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        CameraController cc = FindObjectOfType<CameraController>();
        cc.InGameStartAni();
        yield return null;
        // float time = cc.Animator.GetClipLength("InGameStart");
        
        // if(isWait)
        //     yield return new WaitForSeconds(time);
    }
}
