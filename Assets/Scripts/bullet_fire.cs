using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_fire : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    [SerializeField] Transform mageitself;

    public float speed = 1f;
    private float shootingTime;
    public Rigidbody2D rb;
    public GameObject impactEffect;
    public GameObject fireEffect;

    public int level2_mage_count = 0;
    public int level2_mage_ballnum = 3;
    public bool level2_mage_keppit = true;
    public float waittime = 5f;
    public float angle = 45f;
    public GameObject bullet_son;

    //light control
    //public UnityEngine.Experimental.Rendering.Universal.Light2D BSL;
    //public GameObject bsl;


    void Start()
    {
        Vector2 StartPos = new Vector2(transform.position.x, transform.position.y);
        target = GameObject.Find("Player").transform;
        int level = magef.magelevel;
        //bsl = GameObject.Find("firelight");
        //BSL = bsl.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        //BSL.color = Color.red;
        
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
                if (level2_mage_count == 1)
                {
                    rb.velocity = (Quaternion.Euler(0, 0, -1*angle) * bulletDir).normalized * speed;
                }
                else if (level2_mage_count == 2)
                {
                    rb.velocity = (Quaternion.Euler(0, 0, 0f) * bulletDir).normalized * speed;
                }
                else if (level2_mage_count == 3)
                {
                    rb.velocity = (Quaternion.Euler(0, 0, angle) * bulletDir).normalized * speed;
                }
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
        GameObject feffect = Instantiate(fireEffect, transform.position, transform.rotation);

        Destroy(gameObject);

        Destroy(effect, 3f);

        Destroy(feffect, 3f);



    }


}
