using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(EntityMovement))]
public class PlayerController : MonoBehaviour
{
    private Entity entity;

    private void Start()
    {
        entity = GetComponent<Entity>();

        if (!GameOptionManager.IsRelease)
        {
            Managers.Input.AddKeyDownAction(KeyCode.LeftArrow, LeftArrowDown);
            Managers.Input.AddKeyDownAction(KeyCode.RightArrow, RightArrowDown);
            Managers.Input.AddKeyDownAction(KeyCode.UpArrow, UpArrowDown);
            Managers.Input.AddKeyDownAction(KeyCode.DownArrow, DownArrowDown);

            Managers.Input.AddKeyUpAction(KeyCode.LeftArrow, LeftArrowUp);
            Managers.Input.AddKeyUpAction(KeyCode.RightArrow, RightArrowUp);
            Managers.Input.AddKeyUpAction(KeyCode.UpArrow, UpArrowUp);
            Managers.Input.AddKeyUpAction(KeyCode.DownArrow, DownArrowUp);
        }
    }

    #region Input
    Vector3 _inputVector = Vector3.zero;
    float _inputMoveValue = .05f;
    void LeftArrowDown()
    {
        _inputVector = new Vector3(-_inputMoveValue, _inputVector.y, _inputVector.z);
    }
    void LeftArrowUp()
    {
        if (_inputVector.x > 0)
            return;

        _inputVector = new Vector3(0, _inputVector.y, _inputVector.z);
    }
    void RightArrowDown()
    {
        _inputVector = new Vector3(_inputMoveValue, _inputVector.y, _inputVector.z);
    }
    void RightArrowUp()
    {
        if (_inputVector.x < 0)
            return;

        _inputVector = new Vector3(0, _inputVector.y, _inputVector.z);
    }
    void UpArrowDown()
    {
        _inputVector = new Vector3(_inputVector.x, _inputVector.y, _inputMoveValue);
    }
    void UpArrowUp()
    {
        if (_inputVector.z < 0)
            return;

        _inputVector = new Vector3(_inputVector.x, _inputVector.y, 0);
    }
    void DownArrowDown()
    {
        _inputVector = new Vector3(_inputVector.x, _inputVector.y, -_inputMoveValue);
    }
    void DownArrowUp()
    {
        if (_inputVector.z > 0)
            return;

        _inputVector = new Vector3(_inputVector.x, _inputVector.y, 0);
    }
    private void Update()
    {
        if (_inputVector != Vector3.zero)
        {
            entity.Movement.Destination = this.transform.position + _inputVector;
        }
    }
    #endregion
}
