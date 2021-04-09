using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(transform.position != playerTransform.position)
        {
            Vector3 playerPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, playerPosition, smoothing);
        }
        //Vector3 temp = transform.position;
        //temp.x = playerTransform.position.x;
        //temp.y = playerTransform.position.y;

        //transform.position = temp;
    }
}
