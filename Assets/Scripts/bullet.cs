using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] Transform mageitself;
    
    public float speed = 3f;
    private float shootingTime;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    //public int level3_mage_count = 0;
    //public int level3_mage_ballnum = 10;

    //public int level2_mage_count = 0;
    //public int level2_mage_ballnum = 3;
    //public bool level3_mage_keppit = true;
    //public bool level2_mage_keppit = true;
    public float waittime = 5f;
    //public GameObject bullet_son;

    //light control
    public UnityEngine.Experimental.Rendering.Universal.Light2D BSL;
    public GameObject bsl;

    

    

    void Start()
    {
        Vector2 StartPos = new Vector2(transform.position.x, transform.position.y);
        target = GameObject.Find("Player").transform;
        //int level = mage.magelevel;
        //bsl = GameObject.Find("basiclight");
        //BSL = bsl.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        float x = 0f;
        float y = 0f;
        //BSL.color = Color.white;
        Vector3 temp = new Vector3(x, y, 0);
        Vector3 temp_target = target.position;
        temp_target += temp;
        Vector2 bulletDir = (Vector2)temp_target - StartPos;
        rb.velocity = bulletDir.normalized * speed;
        /*Vector2 StartPos = new Vector2(transform.position.x, transform.position.y);
        target = GameObject.Find("Player").transform;
        int level = mage.magelevel;
        bsl = GameObject.Find("bulletlight");
        BSL = bsl.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        float x = 0f;
        float y = 0f;
        if (level == 0)
        {
            //BSL.color = Color.white;
            Debug.Log("Level: " + level);
            x = 0;
            y = 0;
            Vector3 temp = new Vector3(x, y, 0);
            Vector3 temp_target = target.position;
            temp_target += temp;
            Vector2 bulletDir = (Vector2)temp_target - StartPos;
            rb.velocity = bulletDir.normalized * speed;
        }
        else if(level == 1)
        {
            //BSL.color = Color.red;
            Debug.Log("Level: " + level);
            if (level2_mage_keppit)
            {
                if (level2_mage_count < level2_mage_ballnum)
                {
                    level2_mage_count++;
                    if (level2_mage_count == level2_mage_ballnum)
                    {
                        level2_mage_keppit = false;
                    }
                    else
                    {
                        Instantiate(bullet_son, transform.position, transform.rotation);

                    }
                    Vector2 bulletDir = (Vector2)target.position - StartPos;
                    if(level2_mage_count == 1)
                    {
                        rb.velocity = (Quaternion.Euler(0, 0, -30f) * bulletDir).normalized * speed;
                    }
                    else if(level2_mage_count == 2)
                    {
                        rb.velocity = (Quaternion.Euler(0, 0, 0f) * bulletDir).normalized * speed;
                    }
                    else if (level2_mage_count == 3)
                    {
                        rb.velocity = (Quaternion.Euler(0, 0, 30f) * bulletDir).normalized * speed;
                    }
                }
            }
        }
        else if(level == 2)
        {
            //BSL.color = Color.blue;
            Debug.Log("Level: " + level);
            transform.GetComponent<SpriteRenderer>().color = Color.blue;
            speed = 3f;
            if (level3_mage_keppit)
            {
                if(level3_mage_count < level3_mage_ballnum)
                {
                    level3_mage_count++;
                    if (level3_mage_count == level3_mage_ballnum)
                    {
                        level3_mage_keppit = false;
                    }
                    else
                    {
                        Instantiate(bullet_son, transform.position, transform.rotation);
                        
                    }
                    
                   
                    rb.velocity = Quaternion.Euler(0, 0, (360 / level3_mage_ballnum) * level3_mage_count) * transform.right * speed;
                    
                    

                }

            }
            
        }
        
        */


    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        

       
        if (hitInfo.CompareTag("breakable"))
        {
            pot thepot = hitInfo.GetComponent<pot>();
            thepot.Smash();
            
        }
        GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
        
        
        Destroy(gameObject);
        
        Destroy(effect, 3f);



    }


}
