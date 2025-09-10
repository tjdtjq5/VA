using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

public class DungeonNode : Node
{
    [SerializeField]
    private int stage;
    [SerializeField]
    private int index;
    [SerializeField]
    private DungeonRoom dungeonRoom;

    [Input]
    [SerializeField, HideInInspector]
    private List<DungeonNode> previousNodes = new List<DungeonNode>();

    [Output]
    [SerializeField]
    private DungeonNode thisNode;
    
    public int Stage => stage;
    public int Index => index;
    public DungeonRoom DungeonRoom => dungeonRoom;
    public List<DungeonNode> PreviousNodes => previousNodes;

    protected override void Init()
    {
        base.Init();
        thisNode = this;
    }

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "thisNode")
        {
            return thisNode;
        }
        return null;
    }
}
