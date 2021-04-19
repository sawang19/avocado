using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item) 
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);
        transform.SetParent(GameObject.Find("Grid").transform);
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        

        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;
    

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        if (item.itemType == Item.ItemType.firesword)
        {
            spriteRenderer.color = Color.red;
        }
        else if (item.itemType == Item.ItemType.icesword)
        {
            spriteRenderer.color = new Color(0f, 0.7117f, 1f);
        }
        else if (item.itemType == Item.ItemType.holysword)
        {
            spriteRenderer.color = new Color(1f, 0.9847f, 0f);
        }
        else if (item.itemType == Item.ItemType.magicsword)
        {
            spriteRenderer.color = new Color(0.9130f, 0.1355f, 0.9905f);
        }
        
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
