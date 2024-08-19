using System;
using UnityEngine;

public class BaseLayerBehaviour : StateMachineBehaviour
{
    public Action<Animator, AnimatorStateInfo, int> onStateEnter;
    public Action<Animator, AnimatorStateInfo, int> onStateUpdate;
    public Action<string> onStateExit;
    bool isEnter = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isEnter = true;
        onStateEnter?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onStateUpdate?.Invoke(animator, stateInfo, layerIndex);

        if (isEnter && stateInfo.normalizedTime > 1)
        {
            isEnter = false;
            onStateExit?.Invoke(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }
    }

    public object Clone()
    {
        return new BaseLayerBehaviour()
        {
            onStateEnter = this.onStateEnter,
            onStateUpdate = this.onStateUpdate,
            onStateExit = this.onStateExit,
        };
    }
}
