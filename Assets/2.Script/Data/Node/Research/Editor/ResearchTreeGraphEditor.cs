using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(ResearchTreeGraph))]
public class ResearchTreeGraphEditor : NodeGraphEditor
{
  private Vector2[] nodePositions;

    private readonly int spacingBetweenNodes = 100;

    public override void OnOpen()
    {
        target.nodes.Remove(null);
        nodePositions = target.nodes.Select(node => node.position).ToArray();
    }

    public override void OnGUI(){
        if(CheckNodePositionUpdate()){
            UpdateNodePositions();
        }
    }

    private bool CheckNodePositionUpdate(){
        for(int i = 0; i < nodePositions.Length; i++){
            if(nodePositions[i] != target.nodes[i].position){
                return true;
            }
        }
        return false;
    }
    
    private void UpdateNodePositions(){

        if(target.nodes.Count == 0) return;

        target.nodes = target.nodes.OrderBy(node => node.position.x).ToList();
        nodePositions = target.nodes.Select(node => node.position).ToArray();
        
        var nodes = target.nodes;
        var indexField = typeof(ResearchNode).GetField("index", BindingFlags.NonPublic | BindingFlags.Instance);
        var typeField = typeof(ResearchNode).GetField("type", BindingFlags.NonPublic | BindingFlags.Instance);
        
        ResearchTreeGraph graph = target as ResearchTreeGraph;
        
        for(int i = 0; i < nodes.Count; i++)
        {
            ResearchNode node = (ResearchNode)nodes[i];

            typeField.SetValue(node, graph.Type);

            var index = 0;
            var stageNodes = nodes.Where(n => ((ResearchNode)n).Floor == node.Floor).ToList();

            for(int j = 1; j < stageNodes.Count; j++)
            {
                if(stageNodes[j].position.x - stageNodes[j - 1].position.x >= spacingBetweenNodes)
                {
                    index++;
                }

                indexField.SetValue(stageNodes[j], index);
            }
        }
    }
    public override Node CopyNode(Node original)
    {
        var newNode = base.CopyNode(original);
        UpdateNodePositions();
        return newNode;
    }
    public override Node CreateNode(Type type, Vector2 position)
    {
        var node = base.CreateNode(type, position);
        UpdateNodePositions();
        return node;
    }

    public override void RemoveNode(Node node)
    {
        base.RemoveNode(node);

        if(target.nodes.Count == 0){
            nodePositions = Array.Empty<Vector2>();
        }else{
            UpdateNodePositions();
        }
    }
}
