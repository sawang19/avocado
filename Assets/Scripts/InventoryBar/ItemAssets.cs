using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;
    public Sprite boots;
    public Sprite coins;
    public Sprite keys;
    public Sprite hpPotion;
    public Sprite randomPotion;
    public Sprite shield;
    public Sprite rocket;
    public Sprite warmDrink;
    public Sprite coldDrink;
    public Sprite generalsword;
    public Sprite firesword;
    public Sprite icesword;
    public Sprite holysword;
    public Sprite magicsword;
    public Sprite reaper;
}
