﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        boots,
        coins,
        keys,
        hpPotion,
        randomPotion
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
            case ItemType.hpPotion: return ItemAssets.Instance.hpPotion;
            case ItemType.randomPotion: return ItemAssets.Instance.randomPotion;
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
            case ItemType.hpPotion:
            case ItemType.randomPotion:
                return true;
        }
    }

    public bool IsUsable()
    {
        switch (itemType)
        {
            default:
            case ItemType.boots:
            case ItemType.hpPotion:
            case ItemType.randomPotion:
                return true;
            case ItemType.coins:
            case ItemType.keys:
                return false;
        }
    }
}
