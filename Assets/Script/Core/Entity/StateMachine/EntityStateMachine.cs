using System.Diagnostics;

public class EntityStateMachine : MonoStateMachine<Entity>
{
    protected override void AddStates()
    {
        AddState<EntityDefaultState>();
        AddState<DeadState>();
        AddState<DashState>();
        AddState<PrecedingState>();
        AddState<CastingSkillState>();
        AddState<ChargingSkillState>();
        AddState<InSkillPrecedingActionState>();
        AddState<InSkillActionState>();
        AddState<StunningState>();
        AddState<SleepingState>();
    }

    protected override void MakeTransitions()
    {
        // Default State
        MakeTransition<EntityDefaultState, DashState>(state => Owner.Movement?.IsDashing ?? false);
        MakeTransition<EntityDefaultState, PrecedingState>(state => Owner.Movement?.IsPreceding ?? false);
        MakeTransition<EntityDefaultState, CastingSkillState>(EntityStateCommand.ToCastingSkillState);
        MakeTransition<EntityDefaultState, ChargingSkillState>(EntityStateCommand.ToChargingSkillState);
        MakeTransition<EntityDefaultState, InSkillPrecedingActionState>(EntityStateCommand.ToInSkillPrecedingActionState);
        MakeTransition<EntityDefaultState, InSkillActionState>(EntityStateCommand.ToInSkillActionState);

        // DashState
        MakeTransition<DashState, EntityDefaultState>(state => !Owner.Movement.IsDashing);

        // PrecedingState
        MakeTransition<PrecedingState, EntityDefaultState>(state => !Owner.Movement.IsPreceding);

        // Skill State
        // Casting State
        MakeTransition<CastingSkillState, InSkillPrecedingActionState>(EntityStateCommand.ToInSkillPrecedingActionState);
        MakeTransition<CastingSkillState, InSkillActionState>(EntityStateCommand.ToInSkillActionState);
        MakeTransition<CastingSkillState, EntityDefaultState>(state => !IsSkillInState<CastingState>(state));

        // Charging State
        MakeTransition<ChargingSkillState, InSkillPrecedingActionState>(EntityStateCommand.ToInSkillPrecedingActionState);
        MakeTransition<ChargingSkillState, InSkillActionState>(EntityStateCommand.ToInSkillActionState);
        MakeTransition<ChargingSkillState, EntityDefaultState>(state => !IsSkillInState<ChargingState>(state));

        // PrecedingAction State
        MakeTransition<InSkillPrecedingActionState, InSkillActionState>(EntityStateCommand.ToInSkillActionState);
        MakeTransition<InSkillPrecedingActionState, EntityDefaultState>(state => !IsSkillInState<InPrecedingActionState>(state));

        //Action State
        MakeTransition<InSkillActionState, EntityDefaultState>(state => (state as InSkillActionState).IsStateEnded);

        // CC State
        // Stuning State
        MakeAnyTransition<StunningState>(EntityStateCommand.ToStunningState);

        // Sleeping State
        MakeAnyTransition<SleepingState>(EntityStateCommand.ToSleepingState);

        MakeAnyTransition<EntityDefaultState>(EntityStateCommand.ToDefaultState);

        MakeAnyTransition<DeadState>(state => Owner.IsDead);
        MakeTransition<DeadState, EntityDefaultState>(state => !Owner.IsDead);
    }

    public bool IsSkillInState<T>(State<Entity> state) where T : State<Skill>
    => (state as EntitySkillState).RunningSkill.IsInState<T>();
}
