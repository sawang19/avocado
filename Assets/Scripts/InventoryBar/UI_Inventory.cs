﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplete;

    private void Awake()
    {
        Transform scroll = transform.Find("Scroll");
        Transform viewPort = scroll.Find("Viewport");
        itemSlotContainer = viewPort.Find("itemSlotContainer");
        
        itemSlotTemplete = itemSlotContainer.Find("itemSlotTemplete");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    

    private void RefreshInventoryItems()
    {
        if(inventory.GetItemList().Count <= 6)
        {
            itemSlotContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(745, itemSlotContainer.GetComponent<RectTransform>().sizeDelta.y);
        } else
        {
            itemSlotContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(745 + 120 * (inventory.GetItemList().Count - 6), itemSlotContainer.GetComponent<RectTransform>().sizeDelta.y);
        }
        foreach(Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplete) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 120f;
        foreach(Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplete, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            //itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            //{
            //    // Use item
            //    inventory.UseItem(item);
            //};
            if (item.IsUsable() && itemSlotRectTransform.GetComponent<Button>().CompareTag("itemButton"))
            {
                itemSlotRectTransform.GetComponent<Button>().onClick.AddListener(delegate { inventory.UseItem(item); });
            }


            itemSlotRectTransform.anchoredPosition = new Vector2(60f + x * itemSlotCellSize, 0);
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            if(item.itemType == Item.ItemType.firesword)
            {
                image.color = new Color(1f, 0f, 0f);
            }
            else if (item.itemType == Item.ItemType.icesword)
            {
                image.color = new Color(0f, 0.7117f, 1f);
            }
            else if (item.itemType == Item.ItemType.holysword)
            {
                image.color = new Color(1f, 0.9847f, 0f);
            }
            else if (item.itemType == Item.ItemType.magicsword)
            {
                image.color = new Color(0.9130f, 0.1355f, 0.9905f);
            }

            TextMeshProUGUI uiText = itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            if(item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            } else
            {
                uiText.SetText("");
            }
            
            x++;
        }
    }
}
