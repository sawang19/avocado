using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;
//using UnityEngine.Experimental.Rendering.Universal;
public class Dog : Enemy
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
    public float viewradius;
    public float FOV;
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
    public int originaldir;
    public SpriteRenderer spriteRenderer;
    public int spottedFrame = -100;
    public int showurself = 250;
    public bool sawbyplayer;
    //public UnityEngine.Experimental.Rendering.Universal.Light2D fieldofview;
    //public Seeker seeker;
    //public Path path;
    //public int currentWayPoint = 0;
    //public bool isPathEnd = false;
    //public float nextWayPointDistance = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        viewradius = 8.0f;
        FOV = 30;
        dog_speed = 8.0f;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        startwaitTime = 6f;
        //fieldofview = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        mole_maze = Maze.mazeMap;
        waitTime = startwaitTime;
        xmin = (int)transform.position.x - 10;
        xmax = (int)transform.position.x + 10;
        ymin = (int)transform.position.y - 10;
        ymax = (int)transform.position.y + 10;
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
        originaldir = Random.Range(1, 5);
        moveSpot = new Vector3(xx, yy, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (spottedFrame >= Time.frameCount - showurself)
        {
            

            if (this.gameObject.transform.rotation != Quaternion.Euler(0, 0, 0))
            {
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            agent.speed = 0f;
            CheckDistance();
            drawFieldOfView();
        }
        else
        {
            

            if (this.gameObject.transform.rotation != Quaternion.Euler(0, 0, 0))
            {
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            agent.speed = dog_speed;
            CheckDistance();
            drawFieldOfView();
        }

    }

    void CheckDistance()
    {

     

        target = GameObject.Find("Player").transform;
        //Debug.Log("Player x: " + target.position.x);
        //Debug.Log("Player y: " + target.position.y);
        transform.GetComponent<Renderer>().material.color = Color.green;

        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            transform.GetComponent<Renderer>().material.color = Color.cyan;
            
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                if (findplayer == true)
                {
                    //fieldofview.color = Color.red;
                    
                    Vector3 direction = target.position - transform.position;
                    float degree = Vector3.Angle(direction, transform.forward);
                    ChangeAnim();

                    agent.SetDestination(target.position);
                    agent.isStopped = false;
                    agent.speed = 8f;
                    ChangeState(EnemyState.walk);
                    animator.SetBool("Walking", true);
                }
                else if (findplayer == false)
                {
                    //fieldofview.color = Color.green;
                    agent.SetDestination(moveSpot);
                    //Debug.Log("Mole's position: " + transform.position);
                    //Debug.Log("Destination: " + moveSpot);
                    Vector3 direction = moveSpot - transform.position;
                    float degree = Vector3.Angle(direction, transform.forward);

                    ChangeAnim();
                    ChangeState(EnemyState.walk);
                    animator.SetBool("Walking", true);
                    if (Vector3.Distance(transform.position, moveSpot) < 2f)
                    {

                        if (waitTime <= 0)
                        {
                            xmin = (int)target.position.x - 6;
                            xmax = (int)target.position.x + 6;
                            ymin = (int)target.position.y - 6;
                            ymax = (int)target.position.y + 6;
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
                        }
                        else
                        {
                            direction = moveSpot - transform.position;
                            degree = Vector3.Angle(direction, transform.forward);
                            ChangeAnim();
                            waitTime -= Time.deltaTime;
                        }

                    }
                }


            }
            else if (currentState == EnemyState.stagger)
            {
                findplayer = true;
                //fieldofview.color = Color.red;
                Vector3 direction = target.position - transform.position;
                float degree = Vector3.Angle(direction, transform.forward);
                ChangeAnim();

                agent.SetDestination(target.position);
                agent.isStopped = false;
                agent.speed = 8f;
                
                ChangeState(EnemyState.walk);
                animator.SetBool("Walking", true);
            }

        }
        else
        {
            
            
            if (findplayer == false)
            {
                //fieldofview.color = Color.green;
                agent.SetDestination(moveSpot);
                agent.speed = 3f;
                //Debug.Log("Mole's position: " + transform.position);
                //Debug.Log("Destination: " + moveSpot);
                Vector3 direction = moveSpot - transform.position;
                float degree = Vector3.Angle(direction, transform.forward);
                ChangeAnim();                ChangeState(EnemyState.walk);
                animator.SetBool("Walking", true);
                transform.GetComponent<Renderer>().material.color = Color.green;
                if (Vector3.Distance(transform.position, moveSpot) < 2f)
                {

                    if (waitTime <= 0)
                    {
                        xmin = (int)transform.position.x - 10;
                        xmax = (int)transform.position.x + 10;
                        ymin = (int)transform.position.y - 10;
                        ymax = (int)transform.position.y + 10;
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


                    }
                    else
                    {
                        direction = moveSpot - transform.position;
                        degree = Vector3.Angle(direction, transform.forward);
                        ChangeAnim();
                        waitTime -= Time.deltaTime;
                    }

                }
            }
            else if(findplayer == true)
            {
                //fieldofview.color = Color.red;


                Vector3 direction = target.position - transform.position;
                float degree = Vector3.Angle(direction, transform.forward);
                ChangeAnim();

                agent.SetDestination(target.position);
                agent.isStopped = false;
                
                
                ChangeState(EnemyState.walk);
                animator.SetBool("Walking", true);
            }

            
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
                forward_direction = Quaternion.Euler(0, 0, 180 - FOV / 2f) * transform.right * viewradius;
            }
            else if (agent.velocity.x > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -1*FOV/2f) * transform.right * viewradius;
            }
        }
        else if (Mathf.Abs(agent.velocity.x) < Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.y < 0)
            {
                forward_direction = Quaternion.Euler(0, 0, 180-FOV/2f) * transform.up * viewradius;
            }
            else if (agent.velocity.y > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -1 * FOV / 2f) * transform.up * viewradius;


            }

        }
        else if (Mathf.Abs(agent.velocity.x) == 0 && Mathf.Abs(agent.velocity.y) == 0)
        {
            if (originaldir == 1)
            {
                forward_direction = Quaternion.Euler(0, 0, -1 * FOV / 2f) * transform.up * viewradius;
            }
            else if (originaldir == 2)
            {
                forward_direction = Quaternion.Euler(0, 0, 180 - FOV / 2f) * transform.up * viewradius;
            }
            else if (originaldir == 3)
            {
                forward_direction = Quaternion.Euler(0, 0, 180 - FOV / 2f) * transform.right * viewradius;
            }
            else if (originaldir == 4)
            {
                forward_direction = Quaternion.Euler(0, 0, -1 * FOV / 2f) * transform.right * viewradius;
            }

        }
        for (int i = 0; i <= step; i++)
        {

            Vector3 v = Quaternion.Euler(0, 0, (FOV / step) * i) * forward_direction;
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
            /*if (hitt.transform != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //OnEnemySpotted(hitt.transform.gameObject);
            }*/
        }
    }

    /*private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemies")|| collision.CompareTag("Walls")|| collision.CompareTag("trap"))
        {
            Debug.Log("HitTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT enemy");
        
            xmin = (int)transform.position.x - 10;
            xmax = (int)transform.position.x + 10;
            ymin = (int)transform.position.y - 10;
            ymax = (int)transform.position.y + 10;
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
        }
    }*/

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
                originaldir = 3;
            }
            else if (agent.velocity.x > 0)
            {
                SetAnimFloat(Vector2.right);
                originaldir = 4;
            }
        }
        else if (Mathf.Abs(agent.velocity.x) < Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.y < 0)
            {
                SetAnimFloat(Vector2.down);
                originaldir = 2;
            }
            else if (agent.velocity.y > 0)
            {
                SetAnimFloat(Vector2.up);
                originaldir = 1;
            }
        }
        else if (Mathf.Abs(agent.velocity.x) == 0 && Mathf.Abs(agent.velocity.y) == 0)
        {
            //Debug.Log(originaldir);
            animator.SetBool("Walking", false);
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
}
