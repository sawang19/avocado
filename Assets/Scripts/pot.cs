using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class pot : MonoBehaviour
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
        anim.SetBool("smash", true);
        StartCoroutine(breakCo());
        
    }


    IEnumerator breakCo()
    {
        yield return new WaitForSeconds(.25f);
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
    }
}
