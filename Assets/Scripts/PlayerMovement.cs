using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerState
{
    Walking,
    Attacking
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;
    public static int keys = 0;
    public static int coins = 0;
    public static int boots = 0;

    public Text keyAmount;
    public Text youwin;
    public bool endgame = false;
    public GameOver GameOver;
    public GameWin GameWin;

    private void Start()
    {
        currentState = PlayerState.Walking;
    }
    // Update is called once per frame
    void Update()
    {
        if(!endgame)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Jump") && currentState != PlayerState.Attacking)
            {
                animator.SetBool("Attacking", true);
            }
            else
            {
                UpdateAnimationAndMove();
            }
        }
    }

    void UpdateAnimationAndMove()
    {
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("Attacking", false);
        MoveCharacter();
    }

    void MoveCharacter()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Keys")
        {
            keys++;
            keyAmount.text = "Keys: " + keys;
            FindObjectOfType<AudioManager>().Play("coin");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Coins")
        {
            coins += 10;
            FindObjectOfType<AudioManager>().Play("coin");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Boots")
        {
            moveSpeed = 10f;
            FindObjectOfType<AudioManager>().Play("coin");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Enemies")
        {
            keys = 0;
            GameOverAPI();
        }

        if (collision.gameObject.tag == "Door" && keys == 4)
        {
            GameWinAPI();
        }

    }

    public void GameOverAPI()
    {
        GameOver.Setup();
        endgame = true;
        FindObjectOfType<AudioManager>().Pause("background");
    }

    public void GameWinAPI()
    {
        GameWin.Setup();
        endgame = true;
        FindObjectOfType<AudioManager>().Pause("bacground");
    }
}
