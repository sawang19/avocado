using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        boots,
        coins,
        keys
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch(itemType)
        {
            default:
            case ItemType.boots: return ItemAssets.Instance.boots;
            case ItemType.coins: return ItemAssets.Instance.coins;
            case ItemType.keys: return ItemAssets.Instance.keys;
        }
    }

    public bool IsStackable()
    {
        switch(itemType)
        {
            default:
            case ItemType.boots:
            case ItemType.coins:
            case ItemType.keys:
                return true;
        }
    }

    public bool IsUsable()
    {
        switch (itemType)
        {
            default:
            case ItemType.boots:
                return true;
            case ItemType.coins:
            case ItemType.keys:
                return false;
        }
    }
}
