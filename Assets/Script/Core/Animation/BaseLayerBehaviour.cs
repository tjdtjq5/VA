using System;
using UnityEngine;

public class BaseLayerBehaviour : StateMachineBehaviour
{
    public Action<Animator, AnimatorStateInfo, int> onStateEnter;
    public Action<Animator, AnimatorStateInfo, int> onStateUpdate;
    public Action<Animator, AnimatorStateInfo, int> onStateExit;
    public Action<Animator, AnimatorStateInfo, int> onStateMove;
    public Action<Animator, AnimatorStateInfo, int> onStateIK;
    public Action<Animator, int> onStateMachineEnter;
    public Action<Animator, int> onStateMachineExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 새로운 상태로 변할 때 실행
        onStateEnter?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 처음과 마지막 프레임을 제외한 각 프레임 단위로 실행
        onStateUpdate?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 상태가 다음 상태로 바뀌기 직전에 실행
        onStateExit?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // MonoBehaviour.OnAnimatorMove 직후에 실행
        onStateMove?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // MonoBehaviour.OnAnimatorIK 직후에 실행
        onStateIK?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        // 스크립트가 부착된 상태 기계로 전환이 왔을때 실행
        onStateMachineEnter?.Invoke(animator, stateMachinePathHash);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        // 스크립트가 부착된 상태 기계에서 빠져나올때 실행
        onStateMachineExit?.Invoke(animator, stateMachinePathHash);
    }
}
