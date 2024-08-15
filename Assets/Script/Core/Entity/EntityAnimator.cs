using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    private BaseLayerBehaviour _stateMachine;
    public Animator Animator { get; private set; }

    private Entity _entity;
    private MoveController _moveController;
    private EntityMovement _entityMovement;

    private readonly static int kSpeedHash = Animator.StringToHash("speed");
    private readonly static int kDeadHash = Animator.StringToHash("isDead");
    private readonly static int kDashHash = Animator.StringToHash("isDash");
    private readonly static int kIsStunningHash = Animator.StringToHash("isStunning");
    private readonly static int kIsSleepingHash = Animator.StringToHash("isSleeping");


    public void Setup(Entity entity)
    {
        Animator = _animator;
        _stateMachine = _animator.GetBehaviour<BaseLayerBehaviour>();

        _entity = entity;

        _entityMovement = _entity?.Movement;
        _moveController = _entityMovement?.MoveController;

        _stateMachine.onStateEnter -= OnStateEnter;
        _stateMachine.onStateEnter += OnStateEnter;
        _stateMachine.onStateUpdate -= OnStateUpdate;
        _stateMachine.onStateUpdate += OnStateUpdate;
    }

    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
    }

    public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Animator?.SetBool(kDeadHash, _entity.IsDead);
        if (!_entity.IsDead)
        {
            if (_entityMovement)
                Animator?.SetBool(kDashHash, _entityMovement.IsDashing);

            if (_moveController)
                Animator?.SetFloat(kSpeedHash, _moveController.Weight);

            animator.SetBool(kIsStunningHash, _entity.IsInState<StunningState>());
            animator.SetBool(kIsSleepingHash, _entity.IsInState<SleepingState>());
        }
    }
}
