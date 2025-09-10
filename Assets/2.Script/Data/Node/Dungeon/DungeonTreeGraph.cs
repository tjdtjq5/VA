using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

[CreateAssetMenu(fileName = "DungeonTreeGraph", menuName = "Node/Dungeon/DungeonTreeGraph")]
public class DungeonTreeGraph : NodeGraph
{
    public DungeonNode[] GetNodes() 
    => nodes.Where(node => node is not null).Cast<DungeonNode>().ToArray();
}
