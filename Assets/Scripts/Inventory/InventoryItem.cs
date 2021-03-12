using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
	public string itemDescription;
	public Sprite itemImage;
	public int numberHeld;
	public bool usable;
	public bool unique;
	public UnityEvent thisEvent;

	public void use()
    {
		//Debug.Log("USING ITEM");
		thisEvent.Invoke();
    }

	public void DecreaseAmount()
    {
		numberHeld--;
		if (numberHeld < 0)
        {
			numberHeld = 0;
        }
    }
}
