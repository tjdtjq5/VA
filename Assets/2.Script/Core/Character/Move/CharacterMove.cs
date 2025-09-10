using System;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    Move move;
    private Character _character;
    
    public bool IsMoving { get => move.IsMoving; set => move.IsMoving = value; }
    public string MovingAnimationName { get => move.MovingAnimationName; set => move.MovingAnimationName = value; }
    public void SetSpeed(float value = 1f) => move.SetSpeed(value);
    public void SetMove(Vector3 destPos, Action onStop = null) => move.SetMove(destPos, onStop);
    public void SetMoveDontStopMotion(Vector3 destPos, Action onStop = null) => move.SetMoveDontStopMotion(destPos, onStop);
    public void SetMoveDontStopMotion(string aniName, bool isLoop, Vector3 destPos, Action onStop = null) => move.SetMoveDontStopMotion(aniName, isLoop, destPos, onStop);
    public void SetMoveActionEnd(Vector3 destPos, Action onMoveEnd = null) => move.SetMoveActionEnd(destPos, onMoveEnd);
    public void SetTimeMove(string aniName, Vector3 destPos, float time, Action onStop = null) => move.SetTimeMove(aniName, destPos, time, onStop); 
    public void SetTimeMove(Vector3 destPos, float time, Action onStop = null) => move.SetTimeMove(null, destPos, time, onStop);
    public void SetStop() => move.SetStop(false);
    public void SetIdle() => move.SetIdle();
    
    private bool _isInitialized = false;
    
    
    public void Initialize(Character character)
    {
        this._character = character;
        
        if (move == null)
            move = new Move();
        
        move.Initialize(character, this.transform);

        _isInitialized = true;
    }
    
    private void Update()
    {
        if (_isInitialized)
        {
            move.Update();
        }
    }

    public void Clear() => move?.Clear();
}
