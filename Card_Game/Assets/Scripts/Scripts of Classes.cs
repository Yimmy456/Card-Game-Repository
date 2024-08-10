using Palmmedia.ReportGenerator.Core.Reporting.Builders.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardItemClass
{
    [SerializeField]
    string _itemName;

    [SerializeField]
    Sprite _itemSprite;

    public string GetItemName()
    {
        return _itemName;
    }

    public Sprite GetItemSprite()
    {
        return _itemSprite;
    }
}
