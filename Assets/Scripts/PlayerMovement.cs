using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public enum PlayerState
{
    idle,
    Walking,
    Attacking,
    interacting,
    stagger
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    
    public float baseSpeed = 5f;
    public float speedFactor = 1f;
    public float attackBoost = 1f;
    public float changeSpeedUntil = -1f;

    public Rigidbody2D rb;
    Vector3 movement;
    public Animator animator;
    public static int keys = 0;
    public static int coins = 0;
    public static int boots = 0;

    public Text youwin;
    public bool endgame = false;
    public GameOver GameOver;
    public GameWin GameWin;

    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    public bool playerInRange;

    public bool seetheghost;
    public float viewradius;
    public float step = 15;
    public int dir_flag = 0;

    public GameObject dialogBoxForDoor;
    public Text dialogTextForDoor;
    public string dialogForDoor;
    public bool playerInRangeForDoor;

    public FloatValue currentHealth;
    public FloatValue enemyHealth;
    public FloatValue moveSpeed;
    public InventoryItem mykeys;
    public InventoryItem myboots;
    public InventoryItem mycoins;
    public Signal playerHealthSignal;

    [SerializeField]
    NavMeshSurface2d navMeshSurface;

    [SerializeField] private UI_Inventory uiInventory;

    private Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);
        
    }
    private void Start()
    {
        currentState = PlayerState.idle;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        InvokeRepeating("BuildMesh", 1.0f, 0.5f);
        myboots.numberHeld = 0;
        mykeys.numberHeld = 0;
        mycoins.numberHeld = 0;
        keys = 0;
        //ItemWorld.SpawnItemWorld(new Vector3(10, 10), new Item { itemType = Item.ItemType.boots, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(10, 13), new Item { itemType = Item.ItemType.coins, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(10, 16), new Item { itemType = Item.ItemType.keys, amount = 1 });
    }
    // Update is called once per frame
    void Update()
    {
        if(!endgame)
        {
            movement = Vector3.zero;
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if(currentState != PlayerState.stagger)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    StartCoroutine(AttackCo());
                }
                if (currentState == PlayerState.Walking || currentState == PlayerState.idle)
                {
                    UpdateAnimationAndMove();
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab) && playerInRange)
            {
                if (dialogBox.activeInHierarchy)
                {
                    dialogBox.SetActive(false);
                    Time.timeScale = 1;

                }
                else
                {
                    dialogBox.SetActive(true);
                    //dialogText.text = dialog;
                    Time.timeScale = 0;

                }
            }

            if (Input.GetKeyDown(KeyCode.Return) && playerInRangeForDoor && keys < 2)
            {
                if (dialogBoxForDoor.activeInHierarchy)
                {
                    dialogBoxForDoor.SetActive(false);
                    Time.timeScale = 1;

                }
                else
                {
                    dialogBoxForDoor.SetActive(true);
                    dialogTextForDoor.text = dialogForDoor;
                    Time.timeScale = 0;

                }
            }

            if (Input.GetKeyDown(KeyCode.Return) && playerInRangeForDoor && keys >= 2)
            {
                
                GameWinAPI();
            }

            drawFieldOfView();
            if (Time.time < changeSpeedUntil)
            {
                moveSpeed.runtimeValue = baseSpeed * speedFactor;
                //fieldofview.intensity = 0.7f;
            }
            else
            {
                moveSpeed.runtimeValue = baseSpeed;
                speedFactor = 1f;
                //fieldofview.intensity = 2.0f;
                //fieldofview2.intensity = 2.0f;
            }
            //if (Time.time < changeSpeedUntil)
            //{
            //    moveSpeed.runtimeValue = baseSpeed * speedFactor;
            //}
            //else
            //{
            //    moveSpeed.runtimeValue = baseSpeed;
            //    speedFactor = 1f;
            //}

        }

        
    }

    void UseItem(Item item)
    {
        if (item.IsUsable())
        {
            if (item.itemType == Item.ItemType.boots)
            {
                changeSpeedUntil = Time.time + 5;
                speedFactor = 2;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.boots, amount = 1 });

            }
            if(item.itemType == Item.ItemType.hpPotion)
            {
                if (currentHealth.runtimeValue != currentHealth.initialValue)
                {
                    currentHealth.runtimeValue += 1;
                }

                playerHealthSignal.Raise();
                inventory.RemoveItem(new Item { itemType = Item.ItemType.hpPotion, amount = 1 });

            }
            if(item.itemType == Item.ItemType.randomPotion)
            {
                changeSpeedUntil = Time.time + 5;
                speedFactor = Random.Range(0.5f, 3.0f);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.randomPotion, amount = 1 });
            }
        }
    }

    void BuildMesh()
    {
        
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);

    }

    private IEnumerator AttackCo()
    {
        
        animator.SetBool("Attacking", true);
        
        currentState = PlayerState.Attacking;
        
        yield return null;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.5f);
        currentState = PlayerState.Walking;
    }

    void UpdateAnimationAndMove()
    {
        if (movement != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetBool("moving", true);
        } else
        {
            animator.SetBool("moving", false);
        }
        animator.SetBool("Attacking", false);
    }

    void MoveCharacter()
    {
        rb.MovePosition(transform.position + movement * moveSpeed.runtimeValue * Time.fixedDeltaTime);
    }

    public void Knock(Rigidbody2D rb, float knockTime, float damage)
    {
        currentHealth.runtimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.runtimeValue > 0)
        {
            StartCoroutine(KnockCo(rb, knockTime));
        } else
        {

            currentHealth.runtimeValue = currentHealth.initialValue;
            enemyHealth.runtimeValue = enemyHealth.initialValue;
            //this.gameObject.SetActive(false);
            animator.SetBool("moving", false);
            GameOverAPI();
        }
    }

    private IEnumerator KnockCo(Rigidbody2D rb, float knockTime)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            rb.velocity = Vector2.zero;
            currentState = PlayerState.idle;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Keys")
        {
            keys++;
            mykeys.numberHeld = keys;
            FindObjectOfType<AudioManager>().Play("coin");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Coins")
        {
            coins += 10;
            mycoins.numberHeld = coins;
            FindObjectOfType<AudioManager>().Play("coin");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Boots")
        {
            //moveSpeed = 10f;
            myboots.numberHeld += 1;
            //changeSpeedUntil = Time.time + 5;
            //speedFactor = 1.5f;
            FindObjectOfType<AudioManager>().Play("coin");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Door" && keys == 4)
        {
            GameWinAPI();
        }

        if (collision.gameObject.tag == "hpPotion")
        {
            if(currentHealth.runtimeValue != currentHealth.initialValue)
            {
                currentHealth.runtimeValue += 1;
            }
            
            playerHealthSignal.Raise();
            FindObjectOfType<AudioManager>().Play("coin");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "trap")
        {
            Destroy(collision.gameObject);
            currentHealth.runtimeValue -= 1;
            playerHealthSignal.Raise();

            if (currentHealth.runtimeValue < 0)
            {

                currentHealth.runtimeValue = currentHealth.initialValue;
                enemyHealth.runtimeValue = enemyHealth.initialValue;
                animator.SetBool("moving", false);
                GameOverAPI();
            }
        }

        if (collision.gameObject.tag == "randomPotion")
        {
            changeSpeedUntil = Time.time + 5;
            speedFactor = Random.Range(0.5f, 3.0f);
            //StartCoroutine(speedTime());
            FindObjectOfType<AudioManager>().Play("coin");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "sword")
        {
            attackBoost = 2f;
            FindObjectOfType<AudioManager>().Play("coin");
            animator.SetBool("red", true);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "EnemyTag_Ghost")
        {
            changeSpeedUntil = Time.time + 2;
            speedFactor = 0.6f;
            //StartCoroutine(speedTime());
            FindObjectOfType<AudioManager>().Play("coin");
            //Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "Items")
        {
            ItemWorld itemWorld = collision.gameObject.GetComponent<ItemWorld>();
            if (itemWorld != null)
            {
                inventory.AddItem(itemWorld.GetItem());
                Debug.Log("The length is " + inventory.GetItemList().Count);
                itemWorld.DestroySelf();
                FindObjectOfType<AudioManager>().Play("coin");
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("npc"))
        {
            playerInRange = true;
            //Debug.Log("Player in Range");
        }
        if (other.CompareTag("Door"))
        {
            playerInRangeForDoor = true;
            //Debug.Log("Player in Range");
        }

        //if(other.CompareTag("Items"))
        //{
        //    ItemWorld itemWorld = other.GetComponent<ItemWorld>();

        //    if (itemWorld != null)
        //    {
        //        inventory.AddItem(itemWorld.GetItem());
        //        Debug.Log("The length is " + inventory.GetItemList().Count);
        //        itemWorld.DestroySelf();
        //        FindObjectOfType<AudioManager>().Play("coin");
        //    }
        //}
        
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("npc"))
        {
            playerInRange = false;
            dialogBox.SetActive(false);
            //Debug.Log("Player out Range");
        }
        if (other.CompareTag("Door"))
        {
            playerInRangeForDoor = false;
            dialogBoxForDoor.SetActive(false);
            //Debug.Log("Player out Range");
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
        FindObjectOfType<AudioManager>().Pause("background");
    }

    void drawFieldOfView()
    {
        Vector3 forward_direction;
        forward_direction = Quaternion.Euler(0, 0, 135) * transform.up * viewradius;

        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            if (movement.x < 0)
            {
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.right * viewradius;
                dir_flag = 3;
            }
            else if (movement.x > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -45) * transform.right * viewradius;
                dir_flag = 4;
            }
        }
        else if (Mathf.Abs(movement.x) < Mathf.Abs(movement.y))
        {
            if (movement.y < 0)
            {
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.up * viewradius;
                dir_flag = 2;
            }
            else if (movement.y > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -45) * transform.up * viewradius;
                dir_flag = 1;

            }

        }
        else if (Mathf.Abs(movement.x) == 0 && Mathf.Abs(movement.y) == 0)
        {
            if (dir_flag == 1)
            {
                forward_direction = Quaternion.Euler(0, 0, -45) * transform.up * viewradius;
            }
            else if (dir_flag == 2)
            {
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.up * viewradius;
            }
            else if (dir_flag == 3)
            {
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.right * viewradius;

            }
            else if (dir_flag == 4)
            {
                forward_direction = Quaternion.Euler(0, 0, -45) * transform.right * viewradius;
            }

        }

        for (int i = 0; i <= step; i++)
        {

            Vector3 v = Quaternion.Euler(0, 0, (90.0f / step) * i) * forward_direction;
            Vector2 v_2 = v;
            Vector2 mole_pos_v2 = transform.position;
            Ray ray = new Ray(mole_pos_v2, v_2);

            RaycastHit2D hitt = new RaycastHit2D();
            int mask = LayerMask.GetMask("Enemy_dog", "Enemy_ghost", "Wall");

            hitt = Physics2D.Raycast(ray.origin, ray.direction, viewradius, mask);

            Vector2 pos = mole_pos_v2 + v_2;
            if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Enemy_ghost"))
            {
                Debug.Log("I see the ghost");
                pos = hitt.point;
                //Debug.Log("Hit point: " + pos);
                seetheghost = true;
                OnEnemySpottedghost(hitt.transform.gameObject);


            }
            if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Enemy_dog"))
            {
                Debug.Log("I see the dog");
                pos = hitt.point;
                //Debug.Log("Hit point: " + pos);
                //OnEnemySpotteddog(hitt.transform.gameObject);


            }


            if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                //Debug.Log("Hit Wall");
                pos = hitt.point;

                //Debug.Log("Hit point: " + pos);

            }


            Debug.DrawLine(mole_pos_v2, pos, Color.red);

        }
    }

    void OnEnemySpottedghost(GameObject enemy)
    {
        enemy.GetComponent<ghost>().spottedFrame = Time.frameCount;
    }

    
}
