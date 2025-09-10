using System.Collections;
using System.Collections.Generic;
using Shared.Enums;
using UnityEngine;
using XNode;

public class ResearchNode : Node
{
    [SerializeField]
    private PlayerGrowResearch type;
    [SerializeField]
    private int floor;
    [SerializeField]
    private int index;
    [SerializeField]
    private Research research;

    [Input]
    [SerializeField, HideInInspector]
    private List<ResearchNode> previousNodes = new List<ResearchNode>();

    [Output]
    [SerializeField]
    private ResearchNode thisNode;

    public PlayerGrowResearch Type => type;
    public int Floor => floor;
    public int Index => index;
    public Research Research => research;
    public List<ResearchNode> PreviousNodes => previousNodes;

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
