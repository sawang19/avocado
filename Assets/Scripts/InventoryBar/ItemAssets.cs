﻿using System.Collections;
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
}
