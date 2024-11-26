using System;
using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class Map : MonoBehaviour
{
    private readonly string _layerName = "Layer{0}";
    private readonly string _numbering = "00";
    private readonly Vector3 _sizeValue = new Vector3(0.0575f, 0.0575f, 0); 
    private readonly Vector3 _posValue = new Vector3(0.0137f, -0.1044f, 1);
    private readonly int _cloneCount = 100;
    
    [SerializeField]
    private List<float> layerDistances = new List<float>();
    [SerializeField]
    private List<float> CloneDistance = new List<float>();

    private void Start()
    {
        Sorting();
        CreateClone();
    }

    [Button]
    public void Naming()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>();
        for (int i = 0; i < children.Count; i++)
        {
            children[i].name = CSharpHelper.Format_H(_layerName, i.ToString(_numbering)); 
            
            List<SpriteRenderer> children2 = children[i].gameObject.FindChilds<SpriteRenderer>();

            int sort = children.Count - i;
            for (int j = 0; j < children2.Count; j++)
            {
                children2[j].name = j.ToString(_numbering);
                children2[j].sortingOrder = sort;
            }
        }
    }
    [Button]
    public void Sorting()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>();
        for (int i = 0; i < children.Count; i++)
        {
            if (i < layerDistances.Count)
            {
                children[i].position = _posValue * layerDistances[i];
                children[i].localScale = Vector3.one + _sizeValue * layerDistances[i];
            }
            else
            {
                children[i].position = Vector3.zero;
                children[i].localScale = Vector3.one;
            }
        }
    }
    [Button]
    public void Restoration()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>();
        for (int i = 0; i < children.Count; i++)
        {
            children[i].position = Vector3.zero;
            children[i].localScale = Vector3.one;
        }
    }

    void CreateClone()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>();
        int min = -_cloneCount / 2;
        int max = _cloneCount / 2;
        for (int i = 0; i < children.Count; i++)
        {
            Transform child = children[i];
            Vector3 originPos = child.position;
            float cloneDistance = 0;
            if (i < CloneDistance.Count)
                cloneDistance = CloneDistance[i];
            
            for (int j = min; j < max; j++)
            {
                Transform clone = Instantiate(child, this.transform);
                clone.position = new Vector3(originPos.x + j * cloneDistance, originPos.y, originPos.z);
            }
        }
    }
}
