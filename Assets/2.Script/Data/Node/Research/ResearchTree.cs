using System.Collections;
using System.Collections.Generic;
using Shared.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchTree", menuName = "Node/Research/ResearchTree")]
public class ResearchTree : IdentifiedObject
{
    [SerializeField, HideInInspector]
    private ResearchTreeGraph graph;

    public ResearchNode[] GetNodes()
    => graph.GetNodes();

}
