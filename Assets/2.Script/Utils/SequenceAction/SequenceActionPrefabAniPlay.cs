using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sequence/PrefabAniPlay", fileName = "PrefabAniPlay")]
public class SequenceActionPrefabAniPlay : SequenceAction
{
    [SerializeField] private AniPlay prefab;
    
    public override IEnumerator StartSequence(Sequencer context)
    {
        AniPlay aniPlay = Managers.Resources.Instantiate<AniPlay>(prefab);
        aniPlay.Play();

        float time = aniPlay.AniController.GetClipLength("play");
        
        if(isWait)
            yield return new WaitForSeconds(time);
    }
}
