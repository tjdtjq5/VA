using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    [SerializeField] UIScrollView uIScrollView;

    [SerializeField]
    UIScrollViewLayoutStartAxis startAxis;
    [SerializeField]
    int dataCount;
    [SerializeField]
    int selectIndex;
    [SerializeField]
    int rowColmn;
    [SerializeField]
    UIScrollViewLayoutStartCorner startCorner;
    [SerializeField]
    Vector2 spacing;

    [Button]
    public void T()
    {
        List<ICardData> data = new List<ICardData>();
        for (int i = 0; i < dataCount; i++) 
        {
            data.Add(new TestCardData() { Id = i });
        }
        

        uIScrollView.View(startAxis, "TestCard", data, selectIndex, rowColmn, startCorner, spacing.x, spacing.y);
    }

    Stack<GameObject> testCharacterList = new Stack<GameObject>();    
    [Button]
    public void Create()
    {
        testCharacterList.Push(Managers.Resources.Instantiate("Prepab/Character/TestCharacter"));
    }

    [Button]
    public void Delete()
    {
        Managers.Resources.Destroy(testCharacterList.Pop());
    }
}
[System.Serializable]
public class TestA
{
    public string itemCode { get; set; }
    public int itemType { get; set; }
    public string tipName { get; set; }
    public string tipName2 { get; set; }
}
