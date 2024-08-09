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
        // ���ο� ���·� ���� �� ����
        onStateEnter?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ó���� ������ �������� ������ �� ������ ������ ����
        onStateUpdate?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���°� ���� ���·� �ٲ�� ������ ����
        onStateExit?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // MonoBehaviour.OnAnimatorMove ���Ŀ� ����
        onStateMove?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // MonoBehaviour.OnAnimatorIK ���Ŀ� ����
        onStateIK?.Invoke(animator, stateInfo, layerIndex);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        // ��ũ��Ʈ�� ������ ���� ���� ��ȯ�� ������ ����
        onStateMachineEnter?.Invoke(animator, stateMachinePathHash);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        // ��ũ��Ʈ�� ������ ���� ��迡�� �������ö� ����
        onStateMachineExit?.Invoke(animator, stateMachinePathHash);
    }
}
