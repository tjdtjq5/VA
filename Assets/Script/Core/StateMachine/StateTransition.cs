using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// EntityType�� State�� �����ϴ� Entity�� Type
// StateMachine�� EntityType�� ��ġ�ؾ���
public class StateTransition<EntityType>
{
    // Transition Command�� ������ ��Ÿ��
    public const int kNullCommand = int.MinValue;

    // Transition�� ���� ���� �Լ�, ���ڴ� ���� State, ������� ���� ���� ����(bool)
    private Func<State<EntityType>, bool> transitionCondition;

    // ���� State���� �ٽ� ���� State�� ���̰� ���������� ���� ����
    public bool CanTrainsitionToSelf { get; private set; }
    // ���� State
    public State<EntityType> FromState { get; private set; }
    // ������ State
    public State<EntityType> ToState { get; private set; }
    // ���� ��ɾ�
    public int TransitionCommand { get; private set; }
    // ���� ���� ����(Condition ���� ���� ����)
    public bool IsTransferable => transitionCondition == null || transitionCondition.Invoke(FromState);

    public StateTransition(State<EntityType> fromState,
        State<EntityType> toState,
        int transitionCommand,
        Func<State<EntityType>, bool> transitionCondition,
        bool canTrainsitionToSelf)
    {
        UnityHelper.Assert_H(transitionCommand != kNullCommand || transitionCondition != null,
            "StateTransition - TransitionCommand�� TransitionCondition�� �� �� null�� �� �� �����ϴ�.");

        FromState = fromState;
        ToState = toState;
        TransitionCommand = transitionCommand;
        this.transitionCondition = transitionCondition;
        CanTrainsitionToSelf = canTrainsitionToSelf;
    }
}