using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.Universal;

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
    public DynamicJoystick dynamicJoystick;
    //public SimpleInput simpleInput;
    public PlayerState currentState;

    //Timer
    public GameObject textDisplay;
    public int secondsLeft = 180;
    public bool takingAway = false;

    public Text keyNum;
    public Text coinNum;

    public float baseSpeed = 5f;
    public float speedFactor = 1f;
    public float attackBoost = 1f;
    public float changeSpeedUntil = -1f;

    public Rigidbody2D rb;
    Vector3 movement;
    public Animator animator;

    public static int keys = 0;
    public static int coins = 100;
    public static int boots = 0;

    public Text youwin;
    public bool endgame = false;
    public GameOver GameOver;
    public GameWin GameWin;

    public GameObject helpButton;

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

    public bool playerInRangeForTrigger;

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

    public int[,] mazeMap;
    public bool isProtected = false;
    public bool isProtectedFromIce = false;
    public bool isProtectedFromFire = false;
    public float protectedUntil = -1;
    public float protectedFromIceUntil = -1;
    public float protectedFromFireUntil = -1;
    //public bool isProtected = false;
    //public float protectedUntil = -1;

    private Color32 playerColor;
    private bool rocketWorking;

    public Sprite openDoor;
    public Sprite switchTrigger;
    public Sprite talkToNPC;

    /// <new>
    /// /////////
    /// </new>
    private int POTION_HP = 1;
    private int POTION_HP_COST = 30;
    private int POTION_RANDOM = 2;
    private int POTION_RANDOM_COST = 2;
    private int SHIELD = 3;
    private int SHIELD_COST = 3;
    private int ROCKET = 4;
    private int ROCKET_COST = 4;
    private int BOOTS = 5;
    private int BOOTS_COST = 5;

    //light control
    public UnityEngine.Experimental.Rendering.Universal.Light2D SL;
    public UnityEngine.Experimental.Rendering.Universal.Light2D FL;
    public UnityEngine.Experimental.Rendering.Universal.Light2D SD;
    public GameObject fl;
    public GameObject sl;
    public GameObject sd;

    public UnityEngine.Experimental.Rendering.Universal.Light2D BL;
    public GameObject bl;
    //longrange attack
    public Transform firepoint;
    public Transform swordpoint;
    //Holic
    public GameObject bullet;
    //flame
    public GameObject bullet2;
    //ice
    public GameObject bullet3;
    //soul reaper
    public GameObject bullet4;
    //auto
    public GameObject bullet5;
    public GameObject theplayer;
    private float shootTime;
    public int swordtype;

    //trigger
    public Maze maze;

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
        Time.timeScale = 0;
        playerHealthSignal.Raise();
        mazeMap = Maze.mazeMap;
        playerColor = Color.white;
        rocketWorking = false;
        if (PlayerPrefs.GetInt("levels") == 2)
        {
            Debug.Log("text display");
            textDisplay.SetActive(true);
            if(secondsLeft % 60 >= 10)
            {
                textDisplay.GetComponent<Text>().text = "0" + secondsLeft / 60 + ":" + secondsLeft % 60;
            } else
            {
                textDisplay.GetComponent<Text>().text = "0" + secondsLeft / 60 + ":0" + secondsLeft % 60;
            }
            
        }


        //light control
        fl = GameObject.Find("flashlight");
        sl = GameObject.Find("selflight");
        sd = GameObject.Find("shield");
        FL = fl.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        SL = sl.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        SD = sd.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        fl.SetActive(false);
        if (PlayerPrefs.GetInt("levels") == 1 || PlayerPrefs.GetInt("levels") == 2)
        {
            sl.SetActive(false);
        }
        //sl.SetActive(false);
        sd.SetActive(false);
        bl = GameObject.Find("backgroundlight");
        BL = bl.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        swordtype = 0;

        Debug.Log("player prefs level = " + PlayerPrefs.GetInt("levels"));

        //ItemWorld.SpawnItemWorld(new Vector3(10, 10), new Item { itemType = Item.ItemType.boots, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(10, 13), new Item { itemType = Item.ItemType.coins, amount = 1 });
        //ItemWorld.SpawnItemWorld(new Vector3(10, 16), new Item { itemType = Item.ItemType.keys, amount = 1 });
    }
    // Update is called once per frame
    void Update()
    {
        if (!endgame)
        {

            movement = Vector3.zero;
            //movement.x = Input.GetAxisRaw("Horizontal");
            //movement.y = Input.GetAxisRaw("Vertical");
            if (!rocketWorking)
            {
                movement.x = SimpleInput.GetAxisRaw("Horizontal");
                movement.y = SimpleInput.GetAxisRaw("Vertical");
            }


            if (currentState != PlayerState.stagger)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    StartCoroutine(AttackCo());
                }
                if (currentState == PlayerState.Walking || currentState == PlayerState.idle)
                {
                    UpdateAnimationAndMove();
                }
            }

            if (takingAway == false && secondsLeft > 0)
            {
                StartCoroutine(TimerTake());
            }

            if (PlayerPrefs.GetInt("levels") == 2 && secondsLeft <= 0)
            {
                GameOverAPI();
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
            if (Time.time > protectedFromIceUntil)
            {
                isProtectedFromIce = false;
                //transform.GetComponent<SpriteRenderer>().color = playerColor;
                //sd.SetActive(false);

            }
            if (Time.time > protectedFromFireUntil)
            {
                isProtectedFromFire = false;

                //sd.SetActive(false);
            }
            if (Time.time > protectedUntil)
            {
                isProtected = false;
                Debug.Log("isProtected is false");
                //sd.SetActive(false);
            }
            if (Time.time > protectedFromFireUntil && Time.time > protectedFromIceUntil && Time.time > protectedUntil)
            {
                sd.SetActive(false);
            }

        }


    }

    public void npcanddoor()
    {
        if (playerInRange)
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
                FindObjectOfType<Subtegral.DialogueSystem.Runtime.Dialog>().SetToStart();
                Time.timeScale = 0;

            }
        }

        if (playerInRangeForDoor && keys < 2)
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

        if (playerInRangeForTrigger)
        {
            Debug.Log("trigger");
            foreach (GameObject triggerPrefab in maze.triggerList)
            {
                triggerPrefab.GetComponent<Animator>().SetBool("switch", true);
            }
            foreach (GameObject spikePrefab in maze.spikePrefabs)
            {
                spikePrefab.GetComponent<Animator>().SetBool("trigger", true);
            }

        }

        if (playerInRangeForDoor && keys >= 2)
        {

            FindObjectOfType<AudioManager>().Play("gamewin");
            GameWinAPI();
        }

    }

    void UseItem(Item item)
    {
        if (item.IsUsable())
        {
            if (item.itemType == Item.ItemType.boots)
            {
                FindObjectOfType<AudioManager>().Play("boots");
                changeSpeedUntil = Time.time + 5;
                speedFactor = 2;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.boots, amount = 1 });

            }
            if (item.itemType == Item.ItemType.hpPotion)
            {
                FindObjectOfType<AudioManager>().Play("hpPotion");
                if (currentHealth.runtimeValue != currentHealth.initialValue)
                {
                    currentHealth.runtimeValue += 1;
                }

                playerHealthSignal.Raise();
                inventory.RemoveItem(new Item { itemType = Item.ItemType.hpPotion, amount = 1 });

            }
            if (item.itemType == Item.ItemType.randomPotion)
            {
                FindObjectOfType<AudioManager>().Play("randomPotion");
                changeSpeedUntil = Time.time + 5;
                speedFactor = Random.Range(0.5f, 3.0f);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.randomPotion, amount = 1 });
                
            }
            if (item.itemType == Item.ItemType.shield)
            {
                FindObjectOfType<AudioManager>().Play("randomPotion");
                sd.SetActive(true);
                isProtected = true;
                protectedUntil = Time.time + 5;
                Debug.Log("protected = " + protectedUntil);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.shield, amount = 1 });
            }
            if (item.itemType == Item.ItemType.warmDrink)
            {
                FindObjectOfType<AudioManager>().Play("randomPotion");
                //sd.SetActive(true);
                transform.GetComponent<SpriteRenderer>().color = Color.red;
                isProtectedFromIce = true;
                protectedFromIceUntil = Time.time + 5;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.warmDrink, amount = 1 });

            }
            if (item.itemType == Item.ItemType.coldDrink)
            {
                FindObjectOfType<AudioManager>().Play("randomPotion");
                //sd.SetActive(true);
                isProtectedFromFire = true;
                protectedFromFireUntil = Time.time + 5;
                inventory.RemoveItem(new Item { itemType = Item.ItemType.coldDrink, amount = 1 });
            }
            if (item.itemType == Item.ItemType.rocket)
            {
                FindObjectOfType<AudioManager>().Play("randomPotion");
                rocketWorking = true;
                int mazeMapX = mazeMap.GetLength(0);//2 * mazeWidth + 1;
                int mazeMapY = mazeMap.GetLength(1);//2 * mazeHeight + 1;
                int i = Random.Range(1, mazeMapX - 1);
                int j = Random.Range(1, mazeMapY - 1);
                while (mazeMap[i, j] != 0)
                {
                    i = Random.Range(1, mazeMapX - 1);
                    j = Random.Range(1, mazeMapY - 1);
                }
                rb.position = new Vector3(i, j, 0f);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.rocket, amount = 1 });
                StartCoroutine(RocketCo());
            }
            if (item.itemType == Item.ItemType.generalsword)
            {
                if (swordtype != 0)
                {
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.generalsword, amount = 1 });
                    SwitchWeapon(0);
                }
            }
            if (item.itemType == Item.ItemType.firesword)
            {
                if(swordtype != 2)
                {
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.firesword, amount = 1 });
                    SwitchWeapon(2);
                }
            }
            if (item.itemType == Item.ItemType.icesword)
            {
                if (swordtype != 3)
                {
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.icesword, amount = 1 });
                    SwitchWeapon(3);
                }
            }
            if (item.itemType == Item.ItemType.holysword)
            {
                if (swordtype != 1)
                {
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.holysword, amount = 1 });
                    SwitchWeapon(1);
                }
            }
            if (item.itemType == Item.ItemType.magicsword)
            {
                if (swordtype != 5)
                {
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.magicsword, amount = 1 });
                    SwitchWeapon(5);
                }
            }
            if (item.itemType == Item.ItemType.reaper)
            {
                if (swordtype != 4)
                {
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.reaper, amount = 1 });
                    SwitchWeapon(4);
                }
            }
        }
    }

    public bool BuyItem(int item)
    {
        Debug.Log("suc");
        if (item == POTION_HP)
        {
            if (POTION_HP_COST > coins)
            {
                return false;
            }
            coins -= POTION_HP_COST;
            coinNum.text = "x " + coins;
            inventory.AddItem(new Item { itemType = Item.ItemType.hpPotion, amount = 1 });

        }
        else if (item == POTION_RANDOM)
        {
            if (POTION_RANDOM_COST > coins)
            {
                return false;
            }
            coins -= POTION_RANDOM_COST;
            coinNum.text = "x " + coins;
            inventory.AddItem(new Item { itemType = Item.ItemType.randomPotion, amount = 1 });

        }
        else if (item == BOOTS)
        {
            if (BOOTS_COST > coins)
            {
                return false;
            }
            coins -= BOOTS_COST;
            coinNum.text = "x " + coins;
            inventory.AddItem(new Item { itemType = Item.ItemType.boots, amount = 1 });
        }
        else if (item == SHIELD)
        {
            if (SHIELD_COST > coins)
            {
                return false;
            }
            coins -= SHIELD_COST;
            coinNum.text = "x " + coins;
            inventory.AddItem(new Item { itemType = Item.ItemType.shield, amount = 1 });

        }
        else if (item == ROCKET)
        {
            if (ROCKET_COST > coins)
            {
                return false;
            }
            coins -= ROCKET_COST;
            coinNum.text = "x " + coins;
            inventory.AddItem(new Item { itemType = Item.ItemType.rocket, amount = 1 });
        }

        return true;
    }

    void BuildMesh()
    {

        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);

    }

    public void SwitchWeapon(int newSword)
    {

        if (swordtype == 0)
        {
            swordtype = newSword;
            inventory.AddItem(new Item { itemType = Item.ItemType.generalsword, amount = 1 });

        }
        else if(swordtype == 2)
        {
            swordtype = newSword;
            inventory.AddItem(new Item { itemType = Item.ItemType.firesword, amount = 1 });
        }
        else if (swordtype == 3)
        {
            swordtype = newSword;
            inventory.AddItem(new Item { itemType = Item.ItemType.icesword, amount = 1 });
        }
        else if (swordtype == 1)
        {
            swordtype = newSword;
            inventory.AddItem(new Item { itemType = Item.ItemType.holysword, amount = 1 });
        }
        else if (swordtype == 5)
        {
            swordtype = newSword;
            inventory.AddItem(new Item { itemType = Item.ItemType.magicsword, amount = 1 });
        }
        else if (swordtype == 4)
        {
            swordtype = newSword;
            inventory.AddItem(new Item { itemType = Item.ItemType.reaper, amount = 1 });
        }


    }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        if (secondsLeft % 60 < 10)
        {
            textDisplay.GetComponent<Text>().text = "0" + secondsLeft / 60 + ":0" + secondsLeft % 60;
        }
        else
        {
            textDisplay.GetComponent<Text>().text = "0" + secondsLeft / 60 + ":" + secondsLeft % 60;
        }

        takingAway = false;
    }

    private IEnumerator AttackCo()
    {

        animator.SetBool("Attacking", true);

        currentState = PlayerState.Attacking;
        if (swordtype == 1)
        {
            shoot1();
        }
        else if (swordtype == 2)
        {
            shoot2();
        }
        else if (swordtype == 3)
        {
            shoot3();
        }

        else if (swordtype == 4)
        {
            shoot4();
        }
        else if (swordtype == 5)
        {
            shoot5();
        }

        yield return null;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.5f);
        currentState = PlayerState.Walking;
    }

    public IEnumerator ChangeColorCo()
    {


        yield return new WaitForSeconds(0.2f);
        transform.GetComponent<SpriteRenderer>().color = playerColor;

    }

    public IEnumerator ChangeColorCoIce(float freezetime)
    {


        yield return new WaitForSeconds(freezetime);
        transform.GetComponent<SpriteRenderer>().color = playerColor;

    }



    public IEnumerator ShieldBreak()
    {
        sd.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        sd.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        sd.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        sd.SetActive(true);
    }

    private IEnumerator RocketCo()
    {

        yield return new WaitForSeconds(0.05f);
        rocketWorking = false;
    }

    void UpdateAnimationAndMove()
    {
        if (movement != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetBool("moving", true);
        }
        else
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
        //currentHealth.runtimeValue -= damage;
        //playerHealthSignal.Raise();
        if (!isProtected)
        {
            sd.SetActive(false);
            FindObjectOfType<AudioManager>().Play("hurt");
            transform.GetComponent<SpriteRenderer>().color = Color.red;
            currentHealth.runtimeValue -= damage;
            playerHealthSignal.Raise();
        }
        else
        {


            //protectedUntil--;

            //if (protectedUntil == 0)
            //{
            //    Debug.Log("Current is turn off : ");
            //    isProtected = false;
            //    sd.SetActive(false);
            //}
            //else
            //{
            //    StartCoroutine(ShieldBreak());
            //}
            StartCoroutine(ShieldBreak());


        }

        if (currentHealth.runtimeValue > 0)
        {



            StartCoroutine(KnockCo(rb, knockTime));
            if (transform.GetComponent<SpriteRenderer>().color == playerColor)
            {
                StartCoroutine(ChangeColorCo());
            }

        }
        else
        {

            currentHealth.runtimeValue = currentHealth.initialValue;
            enemyHealth.runtimeValue = enemyHealth.initialValue;
            //this.gameObject.SetActive(false);
            animator.SetBool("moving", false);
            FindObjectOfType<AudioManager>().Play("gameover");
            GameOverAPI();
        }
    }

    private IEnumerator KnockCo(Rigidbody2D rb, float knockTime)
    {
        if (rb != null)
        {
            yield return new WaitForSeconds(knockTime);
            transform.GetComponent<SpriteRenderer>().color = playerColor;
            rb.velocity = Vector2.zero;
            currentState = PlayerState.idle;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "trap")
        {
            FindObjectOfType<AudioManager>().Play("trap");
            Destroy(collision.gameObject);
            currentHealth.runtimeValue -= 1;
            playerHealthSignal.Raise();

            if (currentHealth.runtimeValue < 0)
            {

                currentHealth.runtimeValue = currentHealth.initialValue;
                enemyHealth.runtimeValue = enemyHealth.initialValue;
                animator.SetBool("moving", false);
                FindObjectOfType<AudioManager>().Play("gameover");
                GameOverAPI();
            }
        }

        if (collision.gameObject.tag == "sword")
        {
            attackBoost = 2f;
            FindObjectOfType<AudioManager>().Play("coin");
            animator.SetBool("red", true);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Items")
        {
            ItemWorld itemWorld = collision.gameObject.GetComponent<ItemWorld>();
            if (itemWorld.GetItem().itemType == Item.ItemType.keys)
            {
                keys++;
                keyNum.text = "x " + keys;

            }
            else if (itemWorld.GetItem().itemType == Item.ItemType.coins)
            {
                coins++;
                coinNum.text = "x " + coins;

            }
            else if (itemWorld != null)
            {
                inventory.AddItem(itemWorld.GetItem());


            }
            itemWorld.DestroySelf();
            FindObjectOfType<AudioManager>().Play("coin");
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("npc"))
        {
            playerInRange = true;
            helpButton.SetActive(true);
            helpButton.GetComponent<Image>().sprite = talkToNPC;
            //Debug.Log("Player in Range");
        }
        if (other.CompareTag("Door"))
        {
            playerInRangeForDoor = true;
            helpButton.SetActive(true);
            helpButton.GetComponent<Image>().sprite = openDoor;
            //Debug.Log("Player in Range");
        }
        if (other.CompareTag("trigger"))
        {
            
            playerInRangeForTrigger = true;
            helpButton.SetActive(true);
            helpButton.GetComponent<Image>().sprite = switchTrigger;
        }

        if (other.CompareTag("Spike"))
        {

            Debug.Log("spike: " + protectedUntil);

            if (!isProtected)
            {
                sd.SetActive(false);
                FindObjectOfType<AudioManager>().Play("hurt");
                transform.GetComponent<SpriteRenderer>().color = Color.red;
                currentHealth.runtimeValue -= 1f;
                playerHealthSignal.Raise();
                StartCoroutine(ChangeColorCo());
            }
            else
            {



                //protectedUntil -= 0.5f;
                //Debug.Log("protected = " + protectedUntil);
                //Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
                //if (protectedUntil == 0)
                //{
                //    Debug.Log("Current is turn off : ");
                //    isProtected = false;
                //    sd.SetActive(false);

                //}
                //else
                //{

                //    StartCoroutine(ShieldBreak());
                //}
                StartCoroutine(ShieldBreak());
                Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());


            }



            if (currentHealth.runtimeValue < 0)
            {

                currentHealth.runtimeValue = currentHealth.initialValue;
                enemyHealth.runtimeValue = enemyHealth.initialValue;
                animator.SetBool("moving", false);
                FindObjectOfType<AudioManager>().Play("gameover");
                GameOverAPI();
            }

        }

        if (other.CompareTag("Bullet"))
        {


            if (!isProtected)
            {
                //sd.SetActive(false);
                FindObjectOfType<AudioManager>().Play("hurt");
                transform.GetComponent<SpriteRenderer>().color = Color.red;
                currentHealth.runtimeValue -= 1f;
                playerHealthSignal.Raise();
                StartCoroutine(ChangeColorCo());
            }
            else
            {


                //protectedUntil--;

                //if (protectedUntil == 0)
                //{
                //    Debug.Log("Current is turn off : ");
                //    isProtected = false;
                //    sd.SetActive(false);
                //}
                //else
                //{
                //    StartCoroutine(ShieldBreak());
                //}
                StartCoroutine(ShieldBreak());


            }



            if (currentHealth.runtimeValue < 0)
            {

                currentHealth.runtimeValue = currentHealth.initialValue;
                enemyHealth.runtimeValue = enemyHealth.initialValue;
                animator.SetBool("moving", false);
                FindObjectOfType<AudioManager>().Play("gameover");
                GameOverAPI();
            }

        }

        if (other.CompareTag("fire"))
        {


            if (!isProtected && !isProtectedFromFire)
            {
                //sd.SetActive(false);
                FindObjectOfType<AudioManager>().Play("hurt");
                transform.GetComponent<SpriteRenderer>().color = Color.red;
                currentHealth.runtimeValue -= 1f;
                playerHealthSignal.Raise();
                StartCoroutine(ChangeColorCo());
            }
            else
            {


                //protectedUntil--;

                //if (protectedUntil == 0)
                //{
                //    Debug.Log("Current is turn off : ");
                //    isProtected = false;
                //    sd.SetActive(false);
                //}
                //else
                //{
                //    StartCoroutine(ShieldBreak());
                //}
                StartCoroutine(ShieldBreak());

            }



            if (currentHealth.runtimeValue < 0)
            {

                currentHealth.runtimeValue = currentHealth.initialValue;
                enemyHealth.runtimeValue = enemyHealth.initialValue;
                animator.SetBool("moving", false);
                FindObjectOfType<AudioManager>().Play("gameover");
                GameOverAPI();
            }

        }

        if (other.CompareTag("ice"))
        {


            if (!isProtected && !isProtectedFromIce)
            {
                //sd.SetActive(false);
                FindObjectOfType<AudioManager>().Play("hurt");
                transform.GetComponent<SpriteRenderer>().color = Color.cyan;

                currentHealth.runtimeValue -= 1f;
                playerHealthSignal.Raise();
                changeSpeedUntil = Time.time + 1.5f;
                speedFactor = 0.0f;
                StartCoroutine(ChangeColorCoIce(1.5f));
            }
            else
            {


                //protectedUntil--;

                //if (protectedUntil == 0)
                //{
                //    Debug.Log("Current is turn off : ");
                //    isProtected = false;
                //    sd.SetActive(false);
                //}
                //else
                //{
                //    StartCoroutine(ShieldBreak());
                //}
                StartCoroutine(ShieldBreak());


            }



            if (currentHealth.runtimeValue < 0)
            {

                currentHealth.runtimeValue = currentHealth.initialValue;
                enemyHealth.runtimeValue = enemyHealth.initialValue;
                animator.SetBool("moving", false);
                FindObjectOfType<AudioManager>().Play("gameover");
                GameOverAPI();
            }

        }

        if (other.CompareTag("bigice") || other.CompareTag("EnemyTag_SlimeIce"))
        {
            if (!isProtected && !isProtectedFromFire)
            {
                sd.SetActive(false);
                transform.GetComponent<SpriteRenderer>().color = Color.cyan;
                changeSpeedUntil = Time.time + 2;
                speedFactor = 0f;
                StartCoroutine(ChangeColorCoIce(2f));
            }

        }

        if (other.CompareTag("EnemyTag_Ghost"))
        {
            changeSpeedUntil = Time.time + 2;
            speedFactor = 0.6f;
            //StartCoroutine(speedTime());
            FindObjectOfType<AudioManager>().Play("ghost");
            if (BL.intensity > 0.0f)
            {
                BL.intensity -= 0.5f;
            }
            //Destroy(collision.gameObject);
        }

    }

    private void OnTriggerStay(Collider other)
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
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("npc"))
        {
            playerInRange = false;
            helpButton.SetActive(false);
            //dialogBox.SetActive(false);
            //Debug.Log("Player out Range");
        }
        if (other.CompareTag("Door"))
        {
            playerInRangeForDoor = false;
            helpButton.SetActive(false);
            //dialogBoxForDoor.SetActive(false);
            //Debug.Log("Player out Range");
        }
        if (other.CompareTag("trigger"))
        {
            playerInRangeForTrigger = false;
            helpButton.SetActive(false);
        }
    }



    public void GameOverAPI()
    {
        GameOver.Setup();
        Time.timeScale = 0;
        endgame = true;
        FindObjectOfType<AudioManager>().Pause("background");
    }

    public void GameWinAPI()
    {
        GameWin.Setup();
        Time.timeScale = 0;
        endgame = true;
        FindObjectOfType<AudioManager>().Pause("background");
    }

    public void Attack()
    {
        StartCoroutine(AttackCo());
    }

    void drawFieldOfView()
    {
        Vector3 forward_direction;
        forward_direction = Quaternion.Euler(0, 0, 135) * transform.up * viewradius;

        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            if (movement.x < 0)
            {
                //left
                //firepoint.transform.Rotate(180, 0, 0);
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.right * viewradius;
                firepoint.transform.localPosition = new Vector3(-1, 0, 0);
                if (dir_flag == 1)
                {
                    firepoint.transform.Rotate(0, 0, -270);
                }
                else if (dir_flag == 2)
                {
                    firepoint.transform.Rotate(0, 0, -90);
                }
                else if (dir_flag == 4)
                {
                    firepoint.transform.Rotate(0, 0, -180);
                }
                dir_flag = 3;

            }
            else if (movement.x > 0)
            {
                //right
                forward_direction = Quaternion.Euler(0, 0, -45) * transform.right * viewradius;
                firepoint.transform.localPosition = new Vector3(1, 0, 0);
                print("Dir: " + firepoint.transform.right);

                if (dir_flag == 1)
                {
                    firepoint.transform.Rotate(0, 0, -90);
                }
                else if (dir_flag == 2)
                {
                    firepoint.transform.Rotate(0, 0, -270);
                }
                else if (dir_flag == 3)
                {
                    firepoint.transform.Rotate(0, 0, -180);
                }
                dir_flag = 4;

            }
        }
        else if (Mathf.Abs(movement.x) < Mathf.Abs(movement.y))
        {
            if (movement.y < 0)
            {
                //down
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.up * viewradius;
                firepoint.transform.localPosition = new Vector3(0, -1, 0);
                print("Dir: " + firepoint.transform.right);
                if (dir_flag == 1)
                {
                    firepoint.transform.Rotate(0, 0, -180);
                }
                else if (dir_flag == 3)
                {
                    firepoint.transform.Rotate(0, 0, -270);
                }
                else if (dir_flag == 4)
                {
                    firepoint.transform.Rotate(0, 0, -90);
                }

                dir_flag = 2;

            }
            else if (movement.y > 0)
            {
                //up
                forward_direction = Quaternion.Euler(0, 0, -45) * transform.up * viewradius;
                firepoint.transform.localPosition = new Vector3(0, 1, 0);
                print("Dir: " + firepoint.transform.right);

                if (dir_flag == 2)
                {
                    firepoint.transform.Rotate(0, 0, -180);
                }
                else if (dir_flag == 3)
                {
                    firepoint.transform.Rotate(0, 0, -90);
                }
                else if (dir_flag == 4)
                {
                    firepoint.transform.Rotate(0, 0, -270);
                }
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
            int mask = LayerMask.GetMask("Enemy", "Enemy_ghost", "Wall");

            hitt = Physics2D.Raycast(ray.origin, ray.direction, viewradius, mask);

            Vector2 pos = mole_pos_v2 + v_2;
            if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Enemy_ghost"))
            {

                pos = hitt.point;

                seetheghost = true;
                OnEnemySpottedghost(hitt.transform.gameObject);


            }
            if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {

                pos = hitt.point;

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

    public void shoot5()
    {
        if (Time.time > shootTime)
        {
            shootTime = Time.time + 3000 / 1000;
            if (dir_flag == 4)
            {
                Instantiate(bullet5, firepoint.position, firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(0.0f, -1.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(0.0f, 1.0f, 0.0f), firepoint.rotation);
                //Instantiate(bullet5, firepoint.position + new Vector3(-1.0f, -1.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(1.0f, 0.0f, 0.0f), firepoint.rotation);
            }
            else if (dir_flag == 3)
            {
                Instantiate(bullet5, firepoint.position, firepoint.rotation);

                Instantiate(bullet5, firepoint.position + new Vector3(0.0f, -1.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(0.0f, 1.0f, 0.0f), firepoint.rotation);
                //Instantiate(bullet5, firepoint.position + new Vector3(-1.0f, -1.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(-1.0f, 0.0f, 0.0f), firepoint.rotation);
            }
            else if (dir_flag == 2)
            {
                Instantiate(bullet5, firepoint.position, firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(0.0f, -1.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(1.0f, 0.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(-1.0f, 0.0f, 0.0f), firepoint.rotation);
                //Instantiate(bullet5, firepoint.position + new Vector3(0.0f, 1.0f, 0.0f), firepoint.rotation);
            }
            else if (dir_flag == 1)
            {
                Instantiate(bullet5, firepoint.position, firepoint.rotation);
                //Instantiate(bullet5, firepoint.position + new Vector3(0.0f, -1.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(1.0f, 0.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(-1.0f, 0.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet5, firepoint.position + new Vector3(0.0f, 1.0f, 0.0f), firepoint.rotation);


            }
        }
    }

    public void shoot4()
    {
        if (Time.time > shootTime)
        {
            shootTime = Time.time + 3000 / 1000;
            if (dir_flag == 4)
            {
                Instantiate(bullet4, firepoint.position, firepoint.rotation);
            }
            else if (dir_flag == 3)
            {
                Instantiate(bullet4, firepoint.position, firepoint.rotation);
            }
            else if (dir_flag == 2)
            {
                Instantiate(bullet4, firepoint.position, firepoint.rotation);
            }
            else if (dir_flag == 1)
            {
                Instantiate(bullet4, firepoint.position, firepoint.rotation);
            }

        }

    }

    public void shoot3()
    {
        if (Time.time > shootTime)
        {
            shootTime = Time.time + 1000 / 1000;
            if (dir_flag == 4)
            {
                Instantiate(bullet3, swordpoint.position, Quaternion.Euler(0, 0, 90));
            }
            else if (dir_flag == 3)
            {
                Instantiate(bullet3, swordpoint.position, Quaternion.Euler(0, 0, -90));
            }
            else if (dir_flag == 2)
            {
                Instantiate(bullet3, swordpoint.position, swordpoint.rotation);
            }
            else if (dir_flag == 1)
            {
                Instantiate(bullet3, swordpoint.position, Quaternion.Euler(0, 0, 180));
            }

        }

    }

    public void shoot2()
    {
        if (Time.time > shootTime)
        {
            shootTime = Time.time + 1000 / 1000;
            if (dir_flag == 4)
            {
                Instantiate(bullet2, swordpoint.position, Quaternion.Euler(0, 0, 90));
            }
            else if (dir_flag == 3)
            {
                Instantiate(bullet2, swordpoint.position, Quaternion.Euler(0, 0, -90));
            }
            else if (dir_flag == 2)
            {
                Instantiate(bullet2, swordpoint.position, swordpoint.rotation);
            }
            else if (dir_flag == 1)
            {
                Instantiate(bullet2, swordpoint.position, Quaternion.Euler(0, 0, 180));
            }

        }

    }

    public void shoot1()
    {
        if (Time.time > shootTime)
        {
            shootTime = Time.time + 3000 / 1000;
            if (dir_flag == 4)
            {
                Instantiate(bullet, firepoint.position + new Vector3(0.0f, 0.4f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(0.0f, -0.4f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(0.2f, 0.2f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(0.2f, -0.2f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(0.4f, 0.0f, 0.0f), firepoint.rotation);
            }
            else if (dir_flag == 3)
            {
                Instantiate(bullet, firepoint.position + new Vector3(0.0f, 0.4f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(0.0f, -0.4f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(-0.2f, 0.2f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(-0.2f, -0.2f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(-0.4f, 0.0f, 0.0f), firepoint.rotation);
            }
            else if (dir_flag == 2)
            {
                Instantiate(bullet, firepoint.position + new Vector3(0.4f, 0.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(-0.4f, 0.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(-0.2f, -0.2f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(0.2f, -0.2f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(0.0f, -0.4f, 0.0f), firepoint.rotation);
            }
            else if (dir_flag == 1)
            {
                Instantiate(bullet, firepoint.position + new Vector3(0.4f, 0.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(-0.4f, 0.0f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(-0.2f, 0.2f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(0.2f, 0.2f, 0.0f), firepoint.rotation);
                Instantiate(bullet, firepoint.position + new Vector3(0.0f, 0.4f, 0.0f), firepoint.rotation);
            }
        }
        
    }
}
