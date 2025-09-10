using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPuzzleCrashAttack : EnemyAttack
{
    private readonly string _puzzleCrashAttackName = "PuzzleCrash";
    private bool _isPuzzleCrash = false;
    
    public override void Initialize(Character character, Transform transform)
    {
        base.Initialize(character, transform);
        
        for (int i = 0; i < Character.SpineSpineAniControllers.Count; i++)
        {
            Character.SpineSpineAniControllers[i].SetEventFunc(_puzzleCrashAttackName, ActionEvent, PuzzleCrashAction);
            Character.SpineSpineAniControllers[i].SetEndFunc(_puzzleCrashAttackName, PuzzleCrashEnd);
        }

        _isPuzzleCrash = false;
    }
    
    protected override void OnAttackEnd()
    {
        if (!_isPuzzleCrash)
        {
            _isPuzzleCrash = true;
            
            Clear();
            this.Character.SetBack(false);
            this.Character.CharacterMove.SetIdle();
            
            PuzzleCrashStart();
            
            return;
        }
        
        base.OnAttackEnd();
    }

    void PuzzleCrashStart()
    {
        Character.SetAnimation(_puzzleCrashAttackName, false);
    }

    void PuzzleCrashAction()
    {
        Managers.Observer.UIPuzzle.BigBoomAni();
        Managers.Observer.CameraController.Shake(0);
    }

    void PuzzleCrashEnd()
    {
        Vector3 originalPos = Managers.Observer.UIPuzzle.Root.position;
        // Managers.Tween.TweenShake(Managers.Observer.UIPuzzle.Root, 5, 0.75f)
        //     .SetOnPerceontCompleted(1, () =>
        //     {
        //         Managers.Observer.UIPuzzle.PuzzleCrash(5);
        //         Managers.Observer.UIPuzzle.Root.position = originalPos;
        //         Managers.Observer.CameraController.Shake(0);
        //         base.OnAttackEnd();
        //     });

        Managers.Observer.UIPuzzle.PuzzleCrash(5);
        Managers.Observer.UIPuzzle.Root.position = originalPos;
        Managers.Observer.CameraController.Shake(0);
        base.OnAttackEnd();
    }
}
