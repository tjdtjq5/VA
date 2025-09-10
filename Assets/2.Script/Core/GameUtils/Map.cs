using System;
using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    private readonly string _layerName = "Layer{0}";
    private readonly string _numbering = "00";
    private readonly Vector3 _sizeValue = new Vector3(0.0575f, 0.0575f, 0); 
    private readonly Vector3 _posValue = new Vector3(0.0137f, -0.1044f, 1);
    private readonly int _cloneCount = 5;
    
    // [SerializeField]
    // private List<float> layerDistances = new List<float>();
    [SerializeField]
    private List<float> CloneDistance = new List<float>();

    public void Initialize(Transform target)
    {
        // Sorting();
        CreateClone(target);
    }
#if UNITY_EDITOR
    [Button]
    public void Naming()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>();
        for (int i = 0; i < children.Count; i++)
        {
            children[i].name = CSharpHelper.Format_H(_layerName, i.ToString(_numbering)); 
            
            List<Transform> children2 = children[i].gameObject.FindChilds<Transform>();

            int sort = (children.Count - i) * 10;
          
            for (int j = 0; j < children2.Count; j++)
            {
                children2[j].name = "Struct";
                RenameChildrenRecursively(children2[j], "");
            }

            void RenameChildrenRecursively(Transform parent, string baseName)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform child = parent.GetChild(i);
                    child.name = baseName + "(" + i.ToString(_numbering) + ")";
                    
                    if (child.childCount > 0)
                    {
                        RenameChildrenRecursively(child, child.name);
                    }
                }
            }
        }
        
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
    }
    public void Restoration()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>();
        for (int i = 0; i < children.Count; i++)
        {
            children[i].position = Vector3.zero;
            children[i].localScale = Vector3.one;
        }
        
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
    }
    [Button]
    public void MinusSortingLayer()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>(true);
        for (int i = 0; i < children.Count; i++)
        {
            if(children[i].GetComponent<SpriteRenderer>() != null)
            {
                int currentSortingOrder = children[i].GetComponent<SpriteRenderer>().sortingOrder;
                children[i].GetComponent<SpriteRenderer>().sortingOrder = currentSortingOrder - 1;
            }
        }
    }
    [Button]
    public void PlusSortingLayer()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>(true);
        for (int i = 0; i < children.Count; i++)
        {
            if(children[i].GetComponent<SpriteRenderer>() != null)
            {
                int currentSortingOrder = children[i].GetComponent<SpriteRenderer>().sortingOrder;
                children[i].GetComponent<SpriteRenderer>().sortingOrder = currentSortingOrder + 1;
            }
        }
    }
    [Button]
    void CreateClone()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>();
        for (int i = 0; i < children.Count; i++)
        {
            MapLayer childMapLayer = children[i].GetOrAddComponent<MapLayer>();
            
            float cloneDistance = 0;
            if (i < CloneDistance.Count)
                cloneDistance = CloneDistance[i];
            
            childMapLayer.Initialize(null, cloneDistance, _cloneCount);
        }
        
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
    }
    [Button]
    void DeleteClone()
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>();
        for (int i = 0; i < children.Count; i++)
        {
            MapLayer childMapLayer = children[i].GetOrAddComponent<MapLayer>();
            childMapLayer.Clear();
        }
        
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
    }
#endif
    // public void Sorting()
    // {
    //     List<Transform> children = this.gameObject.FindChilds<Transform>();
    //     for (int i = 0; i < children.Count; i++)
    //     {
    //         if (i < layerDistances.Count)
    //         {
    //             children[i].position = _posValue * layerDistances[i];
    //             children[i].localScale = Vector3.one + _sizeValue * layerDistances[i];
    //         }
    //         else
    //         {
    //             children[i].position = Vector3.zero;
    //             children[i].localScale = Vector3.one;
    //         }
    //     }
    // }

    void CreateClone(Transform target)
    {
        List<Transform> children = this.gameObject.FindChilds<Transform>();
        for (int i = 0; i < children.Count; i++)
        {
            MapLayer childMapLayer = children[i].GetOrAddComponent<MapLayer>();
            
            float cloneDistance = 0;
            if (i < CloneDistance.Count)
                cloneDistance = CloneDistance[i];
            
            childMapLayer.Initialize(target, cloneDistance, _cloneCount);
        }
    }
}
