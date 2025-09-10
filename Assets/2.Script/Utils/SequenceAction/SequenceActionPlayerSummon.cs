using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sequence/PlayerSummon", fileName = "PlayerSummon")]
public class SequenceActionPlayerSummon : SequenceAction
{
    public override IEnumerator StartSequence(Sequencer context)
    {
        Character player = Managers.Observer.Player;

        Vector3 originPos = player.transform.position;
        Vector3 summonPos = originPos + new Vector3(-17f, 0, 0);

        player.transform.position = summonPos;
        player.CharacterMove.SetMove(originPos);

        yield return new WaitUntil(() => player.CharacterMove.IsMoving == false);
        player.SetIdle();
    }
}
