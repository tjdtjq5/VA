using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class TextReplacer
{
    // ���忡 �ִ� Ư�� Keyword�� Replace�ϴ� �Լ�.
    // textsByKeyword�� Key�� Replace ��� keyword, Value�� Replace�� ���� Value
    public static string Replace(string text, IReadOnlyDictionary<string, string> textsByKeyword)
    {
        if (textsByKeyword != null)
        {
            // Text = "���� ������ $[Test]���Դϴ�.", Key = "Test", Value = "10";
            // => "���� ������ 10���Դϴ�."
            foreach (var pair in textsByKeyword)
                text = text.Replace($"$[{pair.Key}]", pair.Value);
        }
        return text;
    }

    public static string Replace(string text, string prefixKeyword, IReadOnlyDictionary<string, string> textsByKeyword)
    {
        if (textsByKeyword != null)
        {
            foreach (var pair in textsByKeyword)
                text = text.Replace($"$[{prefixKeyword}.{pair.Key}]", pair.Value);
        }
        return text;
    }

    public static string Replace(string text, IReadOnlyDictionary<string, string> textsByKeyword, string suffixKeyword)
    {
        if (textsByKeyword != null)
        {
            foreach (var pair in textsByKeyword)
                text = text.Replace($"$[{pair.Key}.{suffixKeyword}]", pair.Value);
        }
        return text;
    }

    public static string Replace(string text, string prefixKeyword, IReadOnlyDictionary<string, string> textsByKeyword, string suffixKeyword)
    {
        if (textsByKeyword != null)
        {
            foreach (var pair in textsByKeyword)
                text = text.Replace($"$[{prefixKeyword}.{pair.Key}.{suffixKeyword}]", pair.Value);
        }
        return text;
    }
}
