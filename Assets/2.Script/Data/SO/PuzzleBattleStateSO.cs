using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleBattleStateSO", menuName = "MySO/PuzzleBattleStateSO")]
public class PuzzleBattleStateSO : ScriptableObject
{
    public PuzzleBattleState State => _state;

    [SerializeField, SerializeReference] private PuzzleBattleState _state;
}