using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedItemView : UIFrame
{
    [SerializeField] private Transform _content;

    private List<NeedItem> _needItems = new List<NeedItem>();
    private readonly string _needItemPrefabPath = "Prefab/UI/Card/Robby/NeedItem";

    public void UISet(ItemValue[] itemValues)
    {
        for(int i = 0; i < _needItems.Count; i++)
        {
            _needItems[i].gameObject.SetActive(i < itemValues.Length);
        }

        for(int i = 0; i < itemValues.Length; i++)
        {
            NeedItem needItem = null;
            if(i < _needItems.Count)
            {
                needItem = _needItems[i];
            }
            else
            {
                needItem = Managers.Resources.Instantiate<NeedItem>(_needItemPrefabPath, _content);
                _needItems.Add(needItem);
            }

            needItem.UISet(itemValues[i]);
        }
    }
}
