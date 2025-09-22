using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.Enums;
using UnityEngine;

public class Item : IdentifiedObject
{
    public bool IsAlphabet => _isAlphabet;
    public Grade Grade => _grade;

    public string ToValueString(BBNumber count)
    {
        if (IsAlphabet)
        {
            return count.Alphabet();
        }
        else
        {
            return count.ToInt().ToString();
        }
    }

    [SerializeField] private bool _isAlphabet;
    [SerializeField] private Grade _grade;
}
