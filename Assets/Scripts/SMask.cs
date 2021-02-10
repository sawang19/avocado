using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMask : MonoBehaviour
{
    public Transform target;
    public float addsize = 15.0f;
    public bool larger = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerMovement.keys == 1 && larger == true)
        {
            transform.localScale = new Vector3(transform.localScale.x + addsize, transform.localScale.y + addsize, transform.localScale.z);
            larger = false;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, 20 * Time.deltaTime);
    }
}
