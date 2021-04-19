using System.Collections;
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
        randomPotion,
        shield,
        rocket,
        warmDrink,
        coldDrink,
        generalsword,
        firesword,
        icesword,
        magicsword,
        holysword,
        reaper
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
            case ItemType.shield: return ItemAssets.Instance.shield;
            case ItemType.rocket: return ItemAssets.Instance.rocket;
            case ItemType.warmDrink: return ItemAssets.Instance.warmDrink;
            case ItemType.coldDrink: return ItemAssets.Instance.coldDrink;
            case ItemType.firesword: return ItemAssets.Instance.firesword;
            case ItemType.generalsword: return ItemAssets.Instance.generalsword;
            case ItemType.icesword: return ItemAssets.Instance.icesword;
            case ItemType.holysword: return ItemAssets.Instance.holysword;
            case ItemType.magicsword: return ItemAssets.Instance.magicsword;
            case ItemType.reaper: return ItemAssets.Instance.reaper;

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
            case ItemType.shield:
            case ItemType.rocket:
            case ItemType.warmDrink:
            case ItemType.coldDrink:
                return true;
            case ItemType.firesword:
            case ItemType.icesword:
            case ItemType.holysword:
            case ItemType.magicsword:
            case ItemType.reaper:
            case ItemType.generalsword:
                return false;
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
            case ItemType.shield:
            case ItemType.rocket:
            case ItemType.warmDrink:
            case ItemType.coldDrink:
            case ItemType.firesword:
            case ItemType.icesword:
            case ItemType.holysword:
            case ItemType.magicsword:
            case ItemType.reaper:
            case ItemType.generalsword:
                return true;
            case ItemType.coins:
            case ItemType.keys:
                return false;
        }
    }
}
