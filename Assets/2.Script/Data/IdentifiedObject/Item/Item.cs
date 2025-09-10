using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : IdentifiedObject
{
    public bool IsAlphabet => _isAlphabet;

    [SerializeField] private bool _isAlphabet;
}
