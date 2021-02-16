using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pot : MonoBehaviour
{
    private Animator anim;
    public GameObject potObj;
    System.Random ran = new System.Random();
    int numOfPots = 3;
    // Start is called before the first frame update
    void Start()
    {
        
        anim = GetComponent<Animator>();

        
    }

    // Update is called once per frame
    void Update()
    {
        while(numOfPots != 0)
        {
            int i = ran.Next(0, Maze.mazeMap.GetLength(0));
            int j = ran.Next(0, Maze.mazeMap.GetLength(1));
            if(Maze.mazeMap[i,j] == 0)
            {
                Vector3 position = new Vector3(i, j, 0f);
                Instantiate(potObj, position, Quaternion.identity);
                numOfPots--;
            }
        }
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
    }
}
