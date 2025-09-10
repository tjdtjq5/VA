using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.Enums;
using UnityEngine;
using XNode;

[CreateAssetMenu(fileName = "ResearchTreeGraph", menuName = "Node/Research/ResearchTreeGraph")]
public class ResearchTreeGraph : NodeGraph
{
    public ResearchNode[] GetNodes() 
    => nodes.Where(node => node is not null).Cast<ResearchNode>().ToArray();
    public PlayerGrowResearch Type => type;

    [SerializeField] private PlayerGrowResearch type;
}
