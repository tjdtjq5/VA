using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : State<Entity>
{
    private PlayerController _playerController;
    private MoveController _moveController;

    protected override void Setup()
    {
        _playerController = Entity.GetComponent<PlayerController>();
        _moveController = Entity.Movement?.MoveController;
    }

    public override void Enter()
    {
        if (_playerController)
            _playerController.enabled = false;

        //if (_moveController)
        //    _moveController.Stop();
    }

    public override void Exit()
    {
        if (_playerController)
            _playerController.enabled = true;
    }
}
