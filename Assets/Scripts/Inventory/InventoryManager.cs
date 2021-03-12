using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [Header("Inventory Information")]
    public PlayerInventory playerInventory;
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Text descriptionText;
    [SerializeField] private GameObject useButton;
    public InventoryItem currentItem;

    public void SetTextAndButton(string description, bool buttonActive)
    {
        descriptionText.text = description;
        if (buttonActive)
        {
            useButton.SetActive(true);
        }
        else
        {
            useButton.SetActive(false);
        }
    }

    void makeInventorySlots()
    {
        if (playerInventory)
        {
            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {
                if (playerInventory.myInventory[i].numberHeld <= 0) continue;

                GameObject tmp = Instantiate(blankInventorySlot, inventoryPanel.transform.position, Quaternion.identity);
                tmp.transform.SetParent(inventoryPanel.transform);
                InventorySlot newslot = tmp.GetComponent<InventorySlot>();
                if (newslot)
                {
                    newslot.Setup(playerInventory.myInventory[i], this);
                }

            }
        }
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        ClearInventorySlots();
        makeInventorySlots();
        SetTextAndButton("", false);
        
    }

    public void SetupDescriptionAndButton(string newDescriptionString, bool isButtonUsable, InventoryItem newItem)
    {
        currentItem = newItem;
        descriptionText.text = newDescriptionString;
        useButton.SetActive(isButtonUsable);
    }

    void ClearInventorySlots()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }


    public void useButtonPressed()
    {
        if (currentItem)
        {
            currentItem.use();
            ClearInventorySlots();
            makeInventorySlots();
            if (currentItem.numberHeld == 0)    SetTextAndButton("", false);
        }
    }
}
