using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chest : MonoBehaviour
{
    private Animator anim;
    System.Random ran = new System.Random();
    public GameObject item;

    //int numOfPots = 3;
    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //while(numOfPots != 0)
        //{
        //    int i = ran.Next(0, Maze.mazeMap.GetLength(0));
        //    int j = ran.Next(0, Maze.mazeMap.GetLength(1));
        //    if(Maze.mazeMap[i,j] == 0)
        //    {
        //        Vector3 position = new Vector3(i, j, 0f);
        //        GameObject p = Instantiate(potObj, position, Quaternion.identity);
        //        p.transform.SetParent(grid.transform);
        //        numOfPots--;
        //    }
        //}
    }

    public void Smash()
    {
        //Debug.Log("break pot");
        anim.SetBool("open", true);
        StartCoroutine(breakCo());
        
    }


    IEnumerator breakCo()
    {
        yield return new WaitForSeconds(.8f);
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
        if(item.CompareTag("sword"))
        {
            Instantiate(item, transform.position, Quaternion.identity);

        } else if(item.CompareTag("Keys"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.keys, amount = 1 });
        } else if(item.CompareTag("Coins"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.coins, amount = 1 });
        } else if(item.CompareTag("Boots"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.boots, amount = 1 });
        } else if(item.CompareTag("hpPotion"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.hpPotion, amount = 1 });
        } else if(item.CompareTag("randomPotion"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.randomPotion, amount = 1 });
        }
        else if (item.CompareTag("Shield"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.shield, amount = 1 });
        }
        else if (item.CompareTag("Rocket"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.rocket, amount = 1 });
        }
        else if (item.CompareTag("warmDrink"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.warmDrink, amount = 1 });
        }
        else if (item.CompareTag("coldDrink"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.coldDrink, amount = 1 });
        }
        else if (item.CompareTag("torch"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.torch, amount = 1 });
        }
        else if (item.CompareTag("sun"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.sun, amount = 1 });
        }
        else if (item.CompareTag("avocado"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.avocado, amount = 1 });
        }
        else if (item.CompareTag("item_firesword"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.firesword, amount = 1 });
        }
        else if (item.CompareTag("item_icesword"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.icesword, amount = 1 });
        }
        else if (item.CompareTag("item_holysword"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.holysword, amount = 1 });
        }
        else if (item.CompareTag("item_magicsword"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.magicsword, amount = 1 });
        }
        else if (item.CompareTag("item_reaper"))
        {
            ItemWorld.SpawnItemWorld(transform.position, new Item { itemType = Item.ItemType.reaper, amount = 1 });
        }

    }
}
