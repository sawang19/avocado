using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class bullet_p4 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] Transform mageitself;

    public float speed = 3f;
    private float shootingTime;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    
    public float waittime = 5f;
    //public GameObject bullet_son;
    public int level2_mage_count = 0;
    public int level2_mage_ballnum = 3;
    public bool level2_mage_keppit = true;
    
    public float angle = 45f;
    //light control
    public UnityEngine.Experimental.Rendering.Universal.Light2D BSL;
    public GameObject bsl;

    public GameObject bullet_son;
    public float distance;
    public GameObject closest;




    void Start()
    {

        Vector2 StartPos = new Vector2(transform.position.x, transform.position.y);
        target = GameObject.Find("Player").transform;

        rb.velocity = transform.right * speed;
        
        GameObject[] items2 = GameObject.FindGameObjectsWithTag("EnemyTag_Ghost");

        GameObject[] AllEnemy = items2;

        distance = Mathf.Infinity;
        RaycastHit2D hitt = new RaycastHit2D();
        int mask = LayerMask.GetMask("Enemy", "Enemy_ghost", "Wall");
        foreach (GameObject item in AllEnemy)
        {

            var diff = item.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && curDistance < 30)
            {
                Ray ray = new Ray(transform.position, item.transform.position - transform.position);
                hitt = Physics2D.Raycast(ray.origin, ray.direction, 100, mask);


                if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Enemy_ghost"))
                {
                    distance = curDistance;
                    closest = item;

                }


            }

        }
        Debug.Log(closest);
        if (closest != null)
        {
            Debug.Log("auto aim");
            Vector3 temp_target = closest.transform.position;
            Vector2 bulletDir = (Vector2)temp_target - StartPos;
            rb.velocity = bulletDir.normalized * speed;
            //Debug.DrawLine(mole_pos_v2, pos, Color.red);

        }
        else
        {
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
                    //Vector2 bulletDir = (Vector2)target.position - StartPos;
                    if (level2_mage_count == 1)
                    {
                        rb.velocity = (Quaternion.Euler(0, 0, -1 * angle) * transform.right).normalized * speed;
                    }
                    else if (level2_mage_count == 2)
                    {
                        rb.velocity = (Quaternion.Euler(0, 0, 0f) * transform.right).normalized * speed;
                    }
                    else if (level2_mage_count == 3)
                    {
                        rb.velocity = (Quaternion.Euler(0, 0, angle) * transform.right).normalized * speed;
                    }
                }
            }
        }


        



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
            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);


            Destroy(gameObject);

            Destroy(effect, 3f);

        }
        if (hitInfo.CompareTag("Player") || hitInfo.CompareTag("bullet_p1") || hitInfo.CompareTag("fire") || hitInfo.CompareTag("ice") || hitInfo.CompareTag("bigice") || hitInfo.CompareTag("reaper") || hitInfo.CompareTag("Spike") || hitInfo.CompareTag("firesword") || hitInfo.CompareTag("icesword") || hitInfo.CompareTag("auto") || hitInfo.CompareTag("trigger"))
        {
            Debug.Log("Nothing");
        }


        else
        {
            Debug.Log("why explode");
            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);


            Destroy(gameObject);

            Destroy(effect, 1f);
        }








    }


}
