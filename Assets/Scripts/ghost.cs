using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;
//using UnityEngine.Experimental.Rendering.Universal;
public class ghost : Enemy
{
    // Start is called before the first frame update
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator animator;
    private Rigidbody2D rb;
    [SerializeField] Transform target;
    public NavMeshAgent agent;
    public FloatValue enemyHealth;
    public float dog_speed = 10.0f;
    public float viewradius = 10.0f;
    public float step = 15;
    public bool findplayer = false;
    private int[,] mole_maze;
    private float waitTime;
    public float startwaitTime;
    public Vector3 moveSpot;
    public int xmin;
    public int xmax;
    public int ymin;
    public int ymax;
    public int xmin0;
    public int xmax0;
    public int ymin0;
    public int ymax0;
    public int originaldir;
    public SpriteRenderer spriteRenderer;
    public int spottedFrame = -100;
    public int showurself = 250;
    public float slowspeedfactor = 0.2f;
    public float ghostspeed = 1f;
    public bool sawbyplayer;
    //public UnityEngine.Experimental.Rendering.Universal.Light2D fieldofview;
    void Start()
    {
        
        currentState = EnemyState.idle;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = 1f;
        startwaitTime = 3f;
        mole_maze = Maze.mazeMap;
        waitTime = startwaitTime;
        sawbyplayer = false;
        originaldir = Random.Range(1, 5);
        //fieldofview = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        //fieldofview.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spottedFrame >= Time.frameCount - showurself)
        {
            spriteRenderer.enabled = true;
            
            if (this.gameObject.transform.rotation != Quaternion.Euler(0, 0, 0))
            {
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            
            CheckDistance();
            drawFieldOfView();
        }
        else
        {
            spriteRenderer.enabled = false;
            
            if (this.gameObject.transform.rotation != Quaternion.Euler(0, 0, 0))
            {
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            agent.speed = ghostspeed;
            CheckDistance();
            drawFieldOfView();
        }

    }

    void CheckDistance()
    {
        target = GameObject.Find("Player").transform;

        if (spriteRenderer.enabled == true)
        {
            sawbyplayer = GameObject.Find("Player").GetComponent<PlayerMovement>().seetheghost;
            //Debug.Log("chekc it: " + sawbyplayer);

            if (sawbyplayer)
            {
                //Vector3 direction;
                //float degree = 0f;
                
                ChangeAnim();
                ChangeState(EnemyState.walk);
                animator.SetBool("Walking", true);
                //fieldofview.enabled = true;
                if (waitTime <= 1)
                {
                    sawbyplayer = false;
                    xmin = (int)target.position.x - 5;
                    xmax = (int)target.position.x + 5;
                    ymin = (int)target.position.y - 5;
                    ymax = (int)target.position.y + 5;


                    if (xmin < 1)
                    {
                        xmin = 1;
                    }
                    if (xmax > 40)
                    {
                        xmax = 40;
                    }
                    if (ymin < 1)
                    {
                        ymin = 1;
                    }
                    if (ymax > 40)
                    {
                        ymax = 40;
                    }


                    int xx = Random.Range(xmin, xmax);
                    int yy = Random.Range(ymin, ymax);
                    while (mole_maze[xx, yy] != 0 || mole_maze[xx - 1, yy] != 0 || mole_maze[xx - 1, yy - 1] != 0 || mole_maze[xx, yy - 1] != 0 || (xx == transform.position.x && yy == transform.position.y))
                    {
                        xx = Random.Range(xmin, xmax);
                        yy = Random.Range(ymin, ymax);
                    }
                    moveSpot = new Vector3(xx, yy, 0);
                    waitTime = startwaitTime;
                    agent.SetDestination(moveSpot);
                    agent.speed = 3f;
                    ChangeAnim();
                    ChangeState(EnemyState.walk);
                    animator.SetBool("Walking", true);
                    //Debug.Log("Change!");
                }
                else
                {
                    //direction = moveSpot - transform.position;
                    //degree = Vector3.Angle(direction, transform.forward);
                    ChangeAnim();
                    agent.speed = 1f;
                    waitTime -= Time.deltaTime;
                    //Debug.Log("Wait time: " + waitTime);
                }

            }
            else if (!sawbyplayer)
            {
                //fieldofview.enabled = false;
                agent.SetDestination(target.position);
                agent.isStopped = false;
                ChangeState(EnemyState.walk);
                animator.SetBool("Walking", true);
            }

            
        }
        else if(spriteRenderer.enabled == false)
        {

            //fieldofview.enabled = false;
            sawbyplayer = GameObject.Find("Player").GetComponent<PlayerMovement>().seetheghost;
            //Debug.Log("chekc it: " + sawbyplayer);
            //Debug.Log("Player x: " + target.position.x);
            //Debug.Log("Player y: " + target.position.y);
            
            Vector3 direction = target.position - transform.position;
            float degree = Vector3.Angle(direction, transform.forward);
            ChangeAnim();

            agent.SetDestination(target.position);
            agent.isStopped = false;

            ChangeState(EnemyState.walk);
            animator.SetBool("Walking", true);
        }

        




    }

    void drawFieldOfView()
    {
        Vector3 forward_direction;
        forward_direction = Quaternion.Euler(0, 0, 120) * transform.up * viewradius;
        if (Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.x < 0)
            {
                forward_direction = Quaternion.Euler(0, 0, 150) * transform.right * viewradius;
            }
            else if (agent.velocity.x > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -30) * transform.right * viewradius;
            }
        }
        else if (Mathf.Abs(agent.velocity.x) < Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.y < 0)
            {
                forward_direction = Quaternion.Euler(0, 0, 150) * transform.up * viewradius;
            }
            else if (agent.velocity.y > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -30) * transform.up * viewradius;


            }

        }
        else if (Mathf.Abs(agent.velocity.x) == 0 && Mathf.Abs(agent.velocity.y) == 0)
        {
            if (originaldir == 1)
            {
                forward_direction = Quaternion.Euler(0, 0, -30) * transform.up * viewradius;
            }
            else if (originaldir == 2)
            {
                forward_direction = Quaternion.Euler(0, 0, 150) * transform.up * viewradius;
            }
            else if (originaldir == 3)
            {
                forward_direction = Quaternion.Euler(0, 0, 150) * transform.right * viewradius;
            }
            else if (originaldir == 4)
            {
                forward_direction = Quaternion.Euler(0, 0, -30) * transform.right * viewradius;
            }

        }
        for (int i = 0; i <= step; i++)
        {

            Vector3 v = Quaternion.Euler(0, 0, (60.0f / step) * i) * forward_direction;
            Vector2 v_2 = v;
            Vector2 mole_pos_v2 = transform.position;
            Ray ray = new Ray(mole_pos_v2, v_2);

            RaycastHit2D hitt = new RaycastHit2D();
            int mask = LayerMask.GetMask("Wall", "Player");

            hitt = Physics2D.Raycast(ray.origin, ray.direction, viewradius, mask);

            Vector2 pos = mole_pos_v2 + v_2;
            if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //Debug.Log("Hit Player");
                pos = hitt.point;
                //Debug.Log("Hit point: " + pos);
                findplayer = true;

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

    

    private void SetAnimFloat(Vector2 setVector)
    {
        animator.SetFloat("Horizontal", setVector.x);
        animator.SetFloat("Vertical", setVector.y);
    }

    private void ChangeAnim()
    {
        if (Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
            else if (agent.velocity.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
        }
        else if (Mathf.Abs(agent.velocity.x) < Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
            else if (agent.velocity.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
        }
        else if (Mathf.Abs(agent.velocity.x) == 0 && Mathf.Abs(agent.velocity.y) == 0)
        {
            //Debug.Log(originaldir);
            if (originaldir == 1)
            {
                SetAnimFloat(Vector2.up);

            }
            else if (originaldir == 2)
            {
                SetAnimFloat(Vector2.down);
            }
            else if (originaldir == 3)
            {
                SetAnimFloat(Vector2.left);
            }
            else if (originaldir == 4)
            {
                SetAnimFloat(Vector2.right);
            }


        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        if (hitInfo.CompareTag("reaper"))
        {


            Destroy(gameObject);


        }
    }
}
