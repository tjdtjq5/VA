using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonTree", menuName = "Node/Dungeon/DungeonTree")]
public class DungeonTree : IdentifiedObject
{
    [SerializeField, HideInInspector]
    private DungeonTreeGraph graph;

    [SerializeField]
    private BBNumber defaultHP;
    [SerializeField]
    private BBNumber defaultATK;

    public DungeonNode[] GetNodes()
    => graph.GetNodes();

    public BBNumber DefaultHP => defaultHP;
    public BBNumber DefaultATK => defaultATK;
}
