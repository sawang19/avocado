using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceandfire : MonoBehaviour
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

        rb.velocity = transform.right * speed;



    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {


        if (hitInfo.CompareTag("Enemies") || hitInfo.CompareTag("EnemyTag_Ghost") || hitInfo.CompareTag("EnemyTag_SlimeLava") || hitInfo.CompareTag("EnemyTag_SlimeIce"))
        {
            Rigidbody2D hit = hitInfo.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if (hitInfo.GetComponent<Enemy>().currentState != EnemyState.stagger)
                {
                    float boost = Component.FindObjectOfType<PlayerMovement>().attackBoost;
                    if (hitInfo.gameObject.CompareTag("EnemyTag_Ghost"))
                    {
                        hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                        hitInfo.GetComponent<Enemy>().Knock(hit, 0.2f, 0);
                    }
                    else
                    {
                        hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                        hitInfo.GetComponent<Enemy>().Knock(hit, 0.2f, boost);
                    }

                }
            }
            
            
        }

        if (hitInfo.CompareTag("breakable"))
        {
            pot thepot = hitInfo.GetComponent<pot>();
            thepot.Smash();
            

        }
        if (hitInfo.CompareTag("Player") || hitInfo.CompareTag("bullet_p1") || hitInfo.CompareTag("fire") || hitInfo.CompareTag("ice") || hitInfo.CompareTag("bigice") || hitInfo.CompareTag("reaper") || hitInfo.CompareTag("Spike") || hitInfo.CompareTag("firesword") || hitInfo.CompareTag("icesword") || hitInfo.CompareTag("auto") || hitInfo.CompareTag("trigger"))
        {
            Debug.Log("Nothing");
        }
        

        //GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);


        Destroy(gameObject,0.45f);

        //Destroy(effect, 3f);
        








    }


}
