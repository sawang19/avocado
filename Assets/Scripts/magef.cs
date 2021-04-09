using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;
using System.Security.Cryptography;
public class magef : Enemy
{
    //public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public Animator animator;
    private Rigidbody2D rb;
    [SerializeField] Transform target;
    public NavMeshAgent agent;
    public FloatValue enemyHealth;


    public float viewradius = 10.0f;
    public float step = 15;
    public float viewangle = 90.0f;

    public bool findplayer = false;
    public bool chaseplayer = false;

    private int[,] mole_maze;
    private float waitTime;
    public float startwaitTime;
    public Vector3 moveSpot;
    public int xmin;
    public int xmax;
    public int ymin;
    public int ymax;
    public int originaldir;

    //bullet
    public GameObject bullet;
    private float shootTime;
    public float firerate;
    public static int magelevel;
    public Transform firepoint;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //target = GameObject.FindWithTag("Player").transform;

        //seeker = GetComponent<Seeker>();
        //seeker.StartPath(rb.position, target.position, OnPathComplete);
        //InvokeRepeating("UpdatePath", 0f, 0.5f);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


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


        magelevel = 1;
        firerate = 5000f;



    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        drawFieldOfView();
        CheckDistance();


    }




    void CheckDistance()
    {


        target = GameObject.Find("Player").transform;
        if (currentState == EnemyState.idle || currentState == EnemyState.walk)
        {
            //agent.speed = 1.5f;
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius)
            {
                if (findplayer == true)
                {
                    if (Vector3.Distance(target.position, transform.position) <= attackRadius && !chaseplayer)
                    {
                        findplayer = true;
                        animator.SetBool("Walking", false);
                        animator.SetBool("Attacking", true);
                        ChangeAnim();
                        agent.isStopped = true;

                        ChangeState(EnemyState.attack);



                    }


                    else if (Vector3.Distance(target.position, transform.position) > attackRadius || chaseplayer)
                    {


                        Vector3 direction = target.position - transform.position;
                        float degree = Vector3.Angle(direction, transform.forward);
                        animator.SetBool("Attacking", false);
                        animator.SetBool("Walking", true);
                        ChangeAnim();
                        agent.isStopped = false;
                        agent.SetDestination(target.position);
                        ChangeState(EnemyState.walk);

                    }
                }
                else if (findplayer == false)
                {

                    agent.SetDestination(moveSpot);
                    Vector3 direction = moveSpot - transform.position;
                    float degree = Vector3.Angle(direction, transform.forward);

                    ChangeAnim();

                    animator.SetBool("Attacking", false);
                    ChangeState(EnemyState.walk);
                    animator.SetBool("Walking", true);
                    findplayer = false;
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
            }
            else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
            {

                findplayer = false;
                agent.SetDestination(moveSpot);
                Vector3 direction = moveSpot - transform.position;
                float degree = Vector3.Angle(direction, transform.forward);
                ChangeAnim();

                animator.SetBool("Attacking", false);
                ChangeState(EnemyState.walk);
                animator.SetBool("Walking", true);

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

        }
        else if (currentState == EnemyState.attack)
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius)
            {
                if (Vector3.Distance(target.position, transform.position) <= attackRadius && !chaseplayer)
                {

                    agent.speed = 0;
                    shoot();
                    ChangeAnim();


                }
                else if (Vector3.Distance(target.position, transform.position) > attackRadius || chaseplayer)
                {
                    agent.speed = 1.5f;
                    animator.SetBool("Attacking", false);
                    animator.SetBool("Walking", true);
                    ChangeAnim();
                    agent.isStopped = false;
                    agent.SetDestination(target.position);
                    ChangeState(EnemyState.walk);

                }
            }
            else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
            {

                agent.SetDestination(moveSpot);
                Vector3 direction = moveSpot - transform.position;
                float degree = Vector3.Angle(direction, transform.forward);
                ChangeAnim();

                animator.SetBool("Attacking", false);
                ChangeState(EnemyState.walk);
                animator.SetBool("Walking", true);
                findplayer = false;
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

        }
        else if (currentState == EnemyState.stagger)
        {

            findplayer = true;
            //transform.GetComponent<Renderer>().material.color = Color.red;

            Vector3 direction = target.position - transform.position;
            float degree = Vector3.Angle(direction, transform.forward);
            ChangeAnim();
            drawFieldOfView();
            agent.SetDestination(target.position);
            agent.isStopped = false;
            ChangeState(EnemyState.walk);
            animator.SetBool("Walking", true);

        }




    }

    void drawFieldOfView()
    {
        target = GameObject.Find("Player").transform;
        Vector3 forward_direction;
        forward_direction = Quaternion.Euler(0, 0, viewangle + viewangle / 2) * transform.up * viewradius;
        int countofviewline = 0;
        if (Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.x < 0)
            {
                forward_direction = Quaternion.Euler(0, 0, 180 - viewangle / 2) * transform.right * viewradius;
            }
            else if (agent.velocity.x > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -1 * (viewangle / 2)) * transform.right * viewradius;
            }
        }
        else if (Mathf.Abs(agent.velocity.x) < Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.y < 0)
            {
                forward_direction = Quaternion.Euler(0, 0, 180 - viewangle / 2) * transform.up * viewradius;
            }
            else if (agent.velocity.y > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -1 * (viewangle / 2)) * transform.up * viewradius;


            }

        }

        else if (Mathf.Abs(agent.velocity.x) == 0 && Mathf.Abs(agent.velocity.y) == 0)
        {
            if (Mathf.Abs(target.position.x - transform.position.x) < Mathf.Abs(target.position.y - transform.position.y))
            {
                if (target.position.y > transform.position.y)
                {
                    forward_direction = Quaternion.Euler(0, 0, -1 * (viewangle / 2)) * transform.up * viewradius;

                }
                else
                {
                    forward_direction = Quaternion.Euler(0, 0, 180 - viewangle / 2) * transform.up * viewradius;

                }


            }
            else
            {
                if (target.position.x > transform.position.x)
                {
                    forward_direction = Quaternion.Euler(0, 0, -1 * (viewangle / 2)) * transform.right * viewradius;

                }
                else
                {
                    forward_direction = Quaternion.Euler(0, 0, 180 - viewangle / 2) * transform.right * viewradius;


                }
            }


        }
        for (int i = 0; i <= step; i++)
        {

            Vector3 v = Quaternion.Euler(0, 0, (viewangle / step) * i) * forward_direction;
            Vector2 v_2 = v;
            Vector2 mole_pos_v2 = transform.position;
            Ray ray = new Ray(mole_pos_v2, v_2);

            RaycastHit2D hitt = new RaycastHit2D();
            int mask = LayerMask.GetMask("Wall", "Player");

            hitt = Physics2D.Raycast(ray.origin, ray.direction, viewradius, mask);

            Vector2 pos = mole_pos_v2 + v_2;
            if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Hit Player");
                pos = hitt.point;
                //Debug.Log("Hit point: " + pos);
                findplayer = true;
                countofviewline++;

            }
            if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {

                pos = hitt.point;



            }


            Debug.DrawLine(mole_pos_v2, pos, Color.red);

            /*if (hitt.transform != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //OnEnemySpotted(hitt.transform.gameObject);
            }*/
        }
        if (countofviewline == 0 && findplayer == true)
        {
            chaseplayer = true;
        }
        else
        {
            chaseplayer = false;
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

            if (Mathf.Abs(target.position.x - transform.position.x) < Mathf.Abs(target.position.y - transform.position.y))
            {
                if (target.position.y > transform.position.y)
                {
                    SetAnimFloat(Vector2.up);
                    drawFieldOfView();
                }
                else
                {
                    SetAnimFloat(Vector2.down);
                    drawFieldOfView();
                }


            }
            else
            {
                if (target.position.x > transform.position.x)
                {
                    SetAnimFloat(Vector2.right);
                    drawFieldOfView();
                }
                else
                {
                    SetAnimFloat(Vector2.left);
                    drawFieldOfView();
                }
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

    public void shoot()
    {
        if (Time.time > shootTime)
        {
            shootTime = Time.time + firerate / 1000;
            Instantiate(bullet, firepoint.position, firepoint.rotation);
        }

    }
}
