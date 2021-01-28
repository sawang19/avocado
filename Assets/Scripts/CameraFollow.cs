using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;
    private float oneKeySize = 2.5f;
    private float twoKeySize = 4f;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 temp = transform.position;
        temp.x = playerTransform.position.x;
        temp.y = playerTransform.position.y;

        if(Player.keys == 1)
        {
            Camera.main.orthographicSize = oneKeySize;
        }
        if (Player.keys == 2)
        {
            Camera.main.orthographicSize = twoKeySize;
        }
        
        transform.position = temp;
    }
}
