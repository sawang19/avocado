using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_fire : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    public float speed = 50f;
    private float shootingTime;
    public Rigidbody2D rb;
    public Signal playerHealthSignal;
    public GameObject impactEffect;
    public float factor = 0.1f;
    void Start()
    {
        Vector2 StartPos = new Vector2(transform.position.x, transform.position.y);
        target = GameObject.Find("Player").transform;
        Vector2 bulletDir = (Vector2)target.position - StartPos;
        rb.velocity = bulletDir * speed * factor;
        Debug.Log(rb.velocity);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        Debug.Log("Bullet hit player!");
        PlayerMovement theplayer = hitInfo.GetComponent<PlayerMovement>();
        if(theplayer != null)
        {
            theplayer.currentHealth.runtimeValue -= 1;
            playerHealthSignal.Raise();
            if (theplayer.currentHealth.runtimeValue < 0)
            {

                theplayer.currentHealth.runtimeValue = theplayer.currentHealth.initialValue;
                theplayer.enemyHealth.runtimeValue = theplayer.enemyHealth.initialValue;
                theplayer.animator.SetBool("moving", false);
                FindObjectOfType<AudioManager>().Play("gameover");
                theplayer.GameOverAPI();
            }
        }
        GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
        
        Destroy(gameObject);
        Destroy(effect, 0.5f);
        
        
    }

    
}
