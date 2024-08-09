using System.Diagnostics;

public class EntityStateMachine : MonoStateMachine<Entity>
{
    protected override void AddStates()
    {
        AddState<EntityDefaultState>();
        AddState<DeadState>();
    }

    protected override void MakeTransitions()
    {
        // Dead State
        MakeTransition<DeadState, EntityDefaultState>(state => !Owner.IsDead);

        MakeAnyTransition<EntityDefaultState>(EntityStateCommand.ToDefaultState);
        MakeAnyTransition<DeadState>(state => Owner.IsDead);
    }
}
