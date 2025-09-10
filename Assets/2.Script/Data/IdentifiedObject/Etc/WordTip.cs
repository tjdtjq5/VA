using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordTip", menuName = "ETC/WordTip")]
public class WordTip : ScriptableObject
{
    public string Title => title;
    public Color TitleColor => titleColor;
    public string Explain => explain;

    [SerializeField] private string title;
    [SerializeField] private Color titleColor;
    [SerializeField, TextArea(5, 10)] private string explain;
}
