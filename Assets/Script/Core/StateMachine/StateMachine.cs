using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// EntityType�� StateMachine�� �����ϴ� Entity�� Type
public class StateMachine<EntityType>
{
    // State�� ���̵Ǿ����� �˸��� Event
    public delegate void StateChangedHandler(StateMachine<EntityType> stateMachine,
        State<EntityType> newState,
        State<EntityType> prevState,
        int layer);

    private class StateData
    {
        // State�� ����Ǵ� Layer
        public int Layer { get; private set; }
        // State�� ��� ����
        public int Priority { get; private set; }
        public State<EntityType> State { get; private set; }
        public List<StateTransition<EntityType>> Transitions { get; private set; } = new();

        public StateData(int layer, int priority, State<EntityType> state)
            => (Layer, Priority, State) = (layer, priority, state);
    }

    private readonly Dictionary<int, Dictionary<Type, StateData>> stateDatasByLayer = new();
    private readonly Dictionary<int, List<StateTransition<EntityType>>> anyTransitionsByLayer = new();

    private readonly Dictionary<int, StateData> currentStateDatasByLayer = new();

    private readonly SortedSet<int> layers = new();

    public EntityType Owner { get; private set; }

    public event StateChangedHandler onStateChanged;

    public void Setup(EntityType owner)
    {
        UnityHelper.Assert_H(owner != null, $"StateMachine<{typeof(EntityType).Name}>::Setup - owner�� null�� �� �� �����ϴ�.");

        Owner = owner;

        AddStates();
        MakeTransitions();
        SetupLayers();
    }

    public void SetupLayers()
    {
        foreach ((int layer, var statDatasByType) in stateDatasByLayer)
        {
            currentStateDatasByLayer[layer] = null;

            var firstStateData = statDatasByType.Values.First(x => x.Priority == 0);
            ChangeState(firstStateData);
        }
    }

    private void ChangeState(StateData newStateData)
    {
        var prevState = currentStateDatasByLayer[newStateData.Layer];

        prevState?.State.Exit();
        currentStateDatasByLayer[newStateData.Layer] = newStateData;
        newStateData.State.Enter();

        onStateChanged?.Invoke(this, newStateData.State, prevState.State, newStateData.Layer);
    }

    private void ChangeState(State<EntityType> newState, int layer)
    {
        var newStateData = stateDatasByLayer[layer][newState.GetType()];
        ChangeState(newStateData);
    }

    private bool TryTransition(IReadOnlyList<StateTransition<EntityType>> transtions, int layer)
    {
        foreach (var transition in transtions)
        {
            if (transition.TransitionCommand != StateTransition<EntityType>.kNullCommand || !transition.IsTransferable)
                continue;

            if (!transition.CanTrainsitionToSelf && currentStateDatasByLayer[layer].State == transition.ToState)
                continue;

            ChangeState(transition.ToState, layer);
            return true;
        }
        return false;
    }

    public void Update()
    {
        foreach (var layer in layers)
        {
            var currentStateData = currentStateDatasByLayer[layer];

            bool hasAnyTransitions = anyTransitionsByLayer.TryGetValue(layer, out var anyTransitions);

            if ((hasAnyTransitions && TryTransition(anyTransitions, layer)) ||
                TryTransition(currentStateData.Transitions, layer))
                continue;

            currentStateData.State.Update();
        }
    }

    public void AddState<T>(int layer = 0) where T : State<EntityType>
    {
        layers.Add(layer);

        var newState = Activator.CreateInstance<T>();
        newState.Setup(this, Owner, layer);

        if (!stateDatasByLayer.ContainsKey(layer))
        {
            stateDatasByLayer[layer] = new();
            anyTransitionsByLayer[layer] = new();
        }

        UnityHelper.Assert_H(!stateDatasByLayer[layer].ContainsKey(typeof(T)),
            $"StateMachine::AddState<{typeof(T).Name}> - �̹� ���°� �����մϴ�.");

        var stateDatasByType = stateDatasByLayer[layer];
        stateDatasByType[typeof(T)] = new StateData(layer, stateDatasByType.Count, newState);
    }

    public void MakeTransition<FromStateType, ToStateType>(int transitionCommand,
        Func<State<EntityType>, bool> transitionCondition, int layer = 0)
        where FromStateType : State<EntityType>
        where ToStateType : State<EntityType>
    {
        var stateDatas = stateDatasByLayer[layer];
        var fromStateData = stateDatas[typeof(FromStateType)];
        var toStateData = stateDatas[typeof(ToStateType)];

        var newTransition = new StateTransition<EntityType>(fromStateData.State, toStateData.State,
            transitionCommand, transitionCondition, true);
        fromStateData.Transitions.Add(newTransition);
    }

    public void MakeTransition<FromStateType, ToStateType>(Enum transitionCommand,
        Func<State<EntityType>, bool> transitionCondition, int layer = 0)
        where FromStateType : State<EntityType>
        where ToStateType : State<EntityType>
        => MakeTransition<FromStateType, ToStateType>(Convert.ToInt32(transitionCommand), transitionCondition, layer);

    public void MakeTransition<FromStateType, ToStateType>(Func<State<EntityType>, bool> transitionCondition, int layer = 0)
        where FromStateType : State<EntityType>
        where ToStateType : State<EntityType>
        => MakeTransition<FromStateType, ToStateType>(StateTransition<EntityType>.kNullCommand, transitionCondition, layer);

    public void MakeTransition<FromStateType, ToStateType>(int transitionCommand, int layer = 0)
        where FromStateType : State<EntityType>
        where ToStateType : State<EntityType>
        => MakeTransition<FromStateType, ToStateType>(transitionCommand, null, layer);

    public void MakeTransition<FromStateType, ToStateType>(Enum transitionCommand, int layer = 0)
        where FromStateType : State<EntityType>
        where ToStateType : State<EntityType>
        => MakeTransition<FromStateType, ToStateType>(transitionCommand, null, layer);

    public void MakeAnyTransition<ToStateType>(int transitionCommand,
        Func<State<EntityType>, bool> transitionCondition, int layer = 0, bool canTransitonToSelf = false)
        where ToStateType : State<EntityType>
    {
        var stateDatasByType = stateDatasByLayer[layer];
        // StateDatas���� ToStateType�� State�� ���� StateData�� ã�ƿ�
        var state = stateDatasByType[typeof(ToStateType)].State;
        // Transition ����, �������� ���Ǹ� ������ ������ ���̹Ƿ� FromState�� �������� ����
        var newTransition = new StateTransition<EntityType>(null, state, transitionCommand, transitionCondition, canTransitonToSelf);
        // Layer�� AnyTransition���� �߰�
        anyTransitionsByLayer[layer].Add(newTransition);
    }

    // MakeAnyTransition �Լ��� Enum Command ����
    // Enum������ ���� Command�� Int�� ��ȯ�Ͽ� ���� �Լ��� ȣ����
    public void MakeAnyTransition<ToStateType>(Enum transitionCommand,
        Func<State<EntityType>, bool> transitionCondition, int layer = 0, bool canTransitonToSelf = false)
        where ToStateType : State<EntityType>
        => MakeAnyTransition<ToStateType>(Convert.ToInt32(transitionCommand), transitionCondition, layer, canTransitonToSelf);

    // MakeAnyTransition �Լ��� Command ���ڰ� ���� ����
    // NullCommand�� �־ �ֻ���� MakeTransition �Լ��� ȣ����
    public void MakeAnyTransition<ToStateType>(Func<State<EntityType>, bool> transitionCondition,
        int layer = 0, bool canTransitonToSelf = false)
        where ToStateType : State<EntityType>
        => MakeAnyTransition<ToStateType>(StateTransition<EntityType>.kNullCommand, transitionCondition, layer, canTransitonToSelf);

    // MakeAnyTransiiton�� Condition ���ڰ� ���� ����
    // Condition���� null�� �־ �ֻ���� MakeTransition �Լ��� ȣ���� 
    public void MakeAnyTransition<ToStateType>(int transitionCommand, int layer = 0, bool canTransitonToSelf = false)
    where ToStateType : State<EntityType>
        => MakeAnyTransition<ToStateType>(transitionCommand, null, layer, canTransitonToSelf);

    // �� �Լ��� Enum ����(Command ���ڰ� Enum���̰� Condition ���ڰ� ����)
    // ���� ���ǵ� Enum���� MakeAnyTransition �Լ��� ȣ����
    public void MakeAnyTransition<ToStateType>(Enum transitionCommand, int layer = 0, bool canTransitonToSelf = false)
        where ToStateType : State<EntityType>
        => MakeAnyTransition<ToStateType>(transitionCommand, null, layer, canTransitonToSelf);

    // Command�� �޾Ƽ� Transition�� �����ϴ� �Լ�
    public bool ExecuteCommand(int transitionCommand, int layer)
    {
        // AnyTransition���� Command�� ��ġ�ϰ�, ���� ������ �����ϴ� Transiton�� ã�ƿ�
        var transition = anyTransitionsByLayer[layer].Find(x =>
        x.TransitionCommand == transitionCommand && x.IsTransferable);

        // AnyTransition���� Transtion�� �� ã�ƿԴٸ� ���� �������� CurrentStateData�� Transitions����
        // Command�� ��ġ�ϰ�, ���� ������ �����ϴ� Transition�� ã�ƿ�
        transition ??= currentStateDatasByLayer[layer].Transitions.Find(x =>
        x.TransitionCommand == transitionCommand && x.IsTransferable);

        // ������ Transtion�� ã�ƿ��� ���ߴٸ� ���� ������ ����
        if (transition == null)
            return false;

        // ������ Transiton�� ã�ƿԴٸ� �ش� Transition�� ToState�� ����
        ChangeState(transition.ToState, layer);
        return true;
    }

    // ExecuteCommand�� Enum Command ����
    public bool ExecuteCommand(Enum transitionCommand, int layer)
        => ExecuteCommand(Convert.ToInt32(transitionCommand), layer);

    // ��� Layer�� ������� ExecuteCommand �Լ��� �����ϴ� �Լ�
    // �ϳ��� Layer�� ���̿� �����ϸ� true�� ��ȯ 
    public bool ExecuteCommand(int transitionCommand)
    {
        bool isSuccess = false;

        foreach (int layer in layers)
        {
            if (ExecuteCommand(transitionCommand, layer))
                isSuccess = true;
        }

        return isSuccess;
    }

    // �� ExecuteCommand �Լ��� Enum Command ����
    public bool ExecuteCommand(Enum transitionCommand)
        => ExecuteCommand(Convert.ToInt32(transitionCommand));

    // ���� �������� CurrentStateData�� Message�� ������ �Լ�
    public bool SendMessage(int message, int layer, object extraData = null)
        => currentStateDatasByLayer[layer].State.OnReceiveMessage(message, extraData);

    // SendMessage �Լ��� Enum Message ����
    public bool SendMessage(Enum message, int layer, object extraData = null)
        => SendMessage(Convert.ToInt32(message), layer, extraData);

    // ��� Layer�� ���� �������� CurrentStateData�� ������� SendMessage �Լ��� �����ϴ� �Լ�
    // �ϳ��� CurrentStateData�� ������ Message�� �����ߴٸ� true�� ��ȯ
    public bool SendMessage(int message, object extraData = null)
    {
        bool isSuccess = false;
        foreach (int layer in layers)
        {
            if (SendMessage(message, layer, extraData))
                isSuccess = true;
        }
        return isSuccess;
    }

    // �� SendMessage �Լ��� Enum Message ����
    public bool SendMessage(Enum message, object extraData = null)
        => SendMessage(Convert.ToInt32(message), extraData);

    // ��� Layer�� ���� �������� CurrentState�� Ȯ���Ͽ�, ���� State�� T Type�� State���� Ȯ���ϴ� �Լ�
    // CurrentState�� T Type�ΰ� Ȯ�εǸ� ��� true�� ��ȯ��
    public bool IsInState<T>() where T : State<EntityType>
    {
        foreach ((_, StateData data) in currentStateDatasByLayer)
        {
            if (data.State.GetType() == typeof(T))
                return true;
        }
        return false;
    }

    // Ư�� Layer�� ������� �������� CurrentState�� T Type���� Ȯ���ϴ� �Լ�
    public bool IsInState<T>(int layer) where T : State<EntityType>
        => currentStateDatasByLayer[layer].State.GetType() == typeof(T);

    // Layer�� ���� �������� State�� ������
    public State<EntityType> GetCurrentState(int layer = 0) => currentStateDatasByLayer[layer].State;

    // Layer�� ���� �������� State�� Type�� ������
    public Type GetCurrentStateType(int layer = 0) => GetCurrentState(layer).GetType();

    // �ڽ� class���� ������ State �߰� �Լ�
    // �� �Լ����� AddState �Լ��� ����� State�� �߰����ָ��
    protected virtual void AddStates() { }
    // �ڽ� class���� ������ Transition ���� �Լ�
    // �� �Լ����� MakeTransition �Լ��� ����� Transition�� ������ָ� ��
    protected virtual void MakeTransitions() { }
}