using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;
    private int flag = 0;
    System.Random ran = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {    
        if (flag == 0)
        {
            movement.x = 1;
            movement.y = 0;
        }
        if(flag == 1)
        {
            movement.x = -1;
            movement.y = 0;
        }
        if(flag == 2)
        {
            movement.x = 0;
            movement.y = 1;
        }
        if(flag == 3)
        {
            movement.x = 0;
            movement.y = -1;
        }
        UpdateAnimationAndMove();
     

    }

    void UpdateAnimationAndMove()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        MoveCharacter();
    }

    void MoveCharacter()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Walls" || collision.gameObject.tag == "Keys")
        {
            if (flag == 0)
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
            if (flag == 1)
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
            if (flag == 2)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
            }
            if (flag == 3)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
            }
            flag = ran.Next(0, 4);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        flag = ran.Next(0, 4);
    }
}
