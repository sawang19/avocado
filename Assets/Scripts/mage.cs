using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public class mage : Enemy
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

    //bullet
    public GameObject bullet;
    public float bulletSpeed = 30.0f;       //speed
    public float fireInterval = 0.3f;       //
    float fireCd = 0;
    private float shootTime;
    public float firerate = 3000f;

    public Transform firepoint;
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
        
        //if(path == null)
        //{
        //    return;
        //}
        //if(currentWayPoint >= path.vectorPath.Count)
        //{
        //    isPathEnd = true;
        //} else
        //{
        //    isPathEnd = false;
        //}

        //Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        //Vector2 force = direction * moveSpeed * Time.deltaTime;
        //rb.AddForce(force);
        //float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        //if(distance < nextWayPointDistance)
        //{
        //    currentWayPoint++;
        //}
    }

    //void UpdatePath()
    //{
    //    if(seeker.IsDone())
    //    {
    //        seeker.StartPath(rb.position, target.position, OnPathComplete);
    //    }
    //}

    //void OnPathComplete(Path p)
    //{
    //    if(!p.error)
    //    {
    //        path = p;
    //        currentWayPoint = 0;
    //    }
    //}

   private IEnumerator MageAttackCo()
    {


        animator.SetBool("Attacking", true);

        animator.SetBool("Walking", false);

        yield return null;
        shoot();
        animator.SetBool("Attacking", false);

        yield return new WaitForSeconds(1f);

        agent.velocity = Vector3.zero;
        currentState = EnemyState.attack;
        animator.SetBool("Walking", true);
        currentState = EnemyState.walk;
    }

    void CheckDistance()
    {

        //target = GameObject.Find("Player").transform;
        //if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius) {
        //    if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
        //    {
        //        Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        //        ChangeAnim(temp - transform.position);
        //        //rb.MovePosition(temp);
        //        agent.SetDestination(target.position);
        //        ChangeState(EnemyState.walk);
        //        animator.SetBool("Walking", true);
        //    }
        //} else
        //{
        //    animator.SetBool("Walking", false);
        //    agent.velocity = Vector2.zero;
        //}

        target = GameObject.Find("Player").transform;
        //Debug.Log("Player x: " + target.position.x);
        //Debug.Log("Player y: " + target.position.y);
        //transform.GetComponent<Renderer>().material.color = Color.green;

        if (Vector3.Distance(target.position, transform.position) <= chaseRadius)
        {
            animator.SetBool("Attacking", false);
            //transform.GetComponent<Renderer>().material.color = Color.cyan;
            moveSpeed = 10.0f;
            if ((currentState == EnemyState.idle || currentState == EnemyState.walk) && currentState != EnemyState.stagger && currentState != EnemyState.attack)
            {
                if (findplayer == true)
                {
                    if (Vector3.Distance(target.position, transform.position) <= attackRadius && currentState != EnemyState.stagger)
                    {
                        Debug.Log("attack!");
                        

                        animator.SetBool("Attacking", true);
                        animator.SetBool("Walking", false);
                        ChangeAnim();
                        agent.velocity = Vector3.zero;
                        shoot();
                        
                    }
                    else if (currentState != EnemyState.attack && currentState != EnemyState.stagger)
                    {
                        //transform.GetComponent<Renderer>().material.color = Color.red;
                        Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                        Vector3 direction = target.position - transform.position;
                        float degree = Vector3.Angle(direction, transform.forward);
                        ChangeAnim();

                        agent.SetDestination(target.position);
                        agent.isStopped = false;
                        ChangeState(EnemyState.walk);
                        animator.SetBool("Walking", true);
                    }
                    

                }
                else if (findplayer == false)
                {
                    //transform.GetComponent<Renderer>().material.color = Color.green;
                    agent.SetDestination(moveSpot);
                    //Debug.Log("Mole's position: " + transform.position);
                    //Debug.Log("Destination: " + moveSpot);
                    Vector3 direction = moveSpot - transform.position;
                    float degree = Vector3.Angle(direction, transform.forward);

                    ChangeAnim();
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
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                Vector3 direction = target.position - transform.position;
                float degree = Vector3.Angle(direction, transform.forward);
                ChangeAnim();

                agent.SetDestination(target.position);
                agent.isStopped = false;
                ChangeState(EnemyState.walk);
                animator.SetBool("Walking", true);
            }

        }
        else if (currentState != EnemyState.attack)
        {
            animator.SetBool("Attacking", false);
            //transform.GetComponent<Renderer>().material.color = Color.green;
            agent.SetDestination(moveSpot);
            //Debug.Log("Mole's position: " + transform.position);
            //Debug.Log("Destination: " + moveSpot);
            Vector3 direction = moveSpot - transform.position;
            float degree = Vector3.Angle(direction, transform.forward);

            ChangeAnim();
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

    void drawFieldOfView()
    {
        Vector3 forward_direction;
        forward_direction = Quaternion.Euler(0, 0, 135) * transform.up * viewradius;
        if (Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.x < 0)
            {
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.right * viewradius;
            }
            else if (agent.velocity.x > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -45) * transform.right * viewradius;
            }
        }
        else if (Mathf.Abs(agent.velocity.x) < Mathf.Abs(agent.velocity.y))
        {
            if (agent.velocity.y < 0)
            {
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.up * viewradius;
            }
            else if (agent.velocity.y > 0)
            {
                forward_direction = Quaternion.Euler(0, 0, -45) * transform.up * viewradius;


            }

        }
        else if (Mathf.Abs(agent.velocity.x) == 0 && Mathf.Abs(agent.velocity.y) == 0)
        {
            if (originaldir == 1)
            {
                forward_direction = Quaternion.Euler(0, 0, -45) * transform.up * viewradius;
            }
            else if (originaldir == 2)
            {
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.up * viewradius;
            }
            else if (originaldir == 3)
            {
                forward_direction = Quaternion.Euler(0, 0, 135) * transform.right * viewradius;
            }
            else if (originaldir == 4)
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
            int mask = LayerMask.GetMask("Wall", "Player");

            hitt = Physics2D.Raycast(ray.origin, ray.direction, viewradius, mask);

            Vector2 pos = mole_pos_v2 + v_2;
            if (hitt.collider != null && hitt.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Hit Player");
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
            Debug.Log(originaldir);
            if (Mathf.Abs(target.position.x - transform.position.x) < Mathf.Abs(target.position.y - transform.position.y))
            {
                if (target.position.y > transform.position.y)
                {
                    SetAnimFloat(Vector2.up);
                }
                else
                {
                    SetAnimFloat(Vector2.down);
                }


            }
            else
            {
                if (target.position.x > transform.position.x)
                {
                    SetAnimFloat(Vector2.right);
                }
                else
                {
                    SetAnimFloat(Vector2.left);
                }
            }
            //else if (originaldir == 2)
            //{
            //    SetAnimFloat(Vector2.down);
            //}
            //else if (originaldir == 3)
            //{
            //    SetAnimFloat(Vector2.left);
            //}
            //else if (originaldir == 4)
            //{
            //    SetAnimFloat(Vector2.right);
            //}


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
        if(Time.time > shootTime)
        {
            shootTime = Time.time + firerate / 1000;
            Instantiate(bullet, firepoint.position, firepoint.rotation);

        }
        
    }
}