using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StateTransition<EntityType>
{
    public const int kNullCommand = int.MinValue;

    private Func<State<EntityType>, bool> transitionCondition;

    public bool CanTrainsitionToSelf { get; private set; }
    public State<EntityType> FromState { get; private set; }
    public State<EntityType> ToState { get; private set; }
    public int TransitionCommand { get; private set; }
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