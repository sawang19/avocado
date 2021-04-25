using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_ice : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] Transform mageitself;

    public float speed = 1f;
    private float shootingTime;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public int level3_mage_count = 0;
    public int level3_mage_ballnum = 10;
    public bool level3_mage_keppit = true;
    public float waittime = 5f;
    public GameObject bullet_son;
    public GameObject iceEffect;
    //light control
    //public UnityEngine.Experimental.Rendering.Universal.Light2D BSL;
    //public GameObject bsl;





    void Start()
    {
        Vector2 StartPos = new Vector2(transform.position.x, transform.position.y);
        target = GameObject.Find("Player").transform;
        int level = magei.magelevel;
        //bsl = GameObject.Find("icelight");
        //BSL = bsl.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        //BSL.color = Color.blue;
        //transform.GetComponent<SpriteRenderer>().color = Color.blue;
        speed = 3f;
        if (level3_mage_keppit)
        {
            if (level3_mage_count < level3_mage_ballnum)
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
                rb.velocity = (Quaternion.Euler(0, 0, (360 / level3_mage_ballnum) * level3_mage_count) * transform.right).normalized * speed;

            }

        }




    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {




        if (hitInfo.CompareTag("breakable"))
        {
            pot thepot = hitInfo.GetComponent<pot>();
            thepot.Smash();

        }
        GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
        int x = Random.Range(0, 10);
        Destroy(gameObject);

        if(x == 1)
        {
            GameObject ieffect = Instantiate(iceEffect, transform.position, transform.rotation);
            Destroy(effect, 1.5f);
            Destroy(ieffect, 1.5f);
        }
        

        

        

        



    }


}
