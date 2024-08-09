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

    private readonly static int kSpeedHash = Animator.StringToHash("speed");
    private readonly static int kDeadHash = Animator.StringToHash("isDead");


    private void Awake()
    {
        Animator = _animator;
        _stateMachine = _animator.GetBehaviour<BaseLayerBehaviour>();

        _moveController = GetComponent<MoveController>();
        _entity = GetComponent<Entity>();

        _stateMachine.onStateEnter += OnStateEnter;
        _stateMachine.onStateUpdate += OnStateUpdate;
    }

    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
    }

    public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_moveController)
            Animator?.SetFloat(kSpeedHash, _moveController.Weight);

        Animator?.SetBool(kDeadHash, _entity.IsDead);
    }
}
