using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public class mole : Enemy
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
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

    void CheckDistance()
    {
        //if(Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        //{

        //    if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
        //    {
        //        Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        //        ChangeAnim(temp - transform.position);
        //        rb.MovePosition(temp);
        //        ChangeState(EnemyState.walk);
        //        animator.SetBool("Walking", true);
        //    }
        //} else
        //{
        //    animator.SetBool("Walking", false);
        //}
        target = GameObject.Find("Player").transform;
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius) {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeAnim(temp - transform.position);
                //rb.MovePosition(temp);
                agent.SetDestination(target.position);
                ChangeState(EnemyState.walk);
                animator.SetBool("Walking", true);
            }
        } else
        {
            animator.SetBool("Walking", false);
            agent.velocity = Vector2.zero;
        }
            
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        animator.SetFloat("Horizontal", setVector.x);
        animator.SetFloat("Vertical", setVector.y);
    }

    private void ChangeAnim(Vector2 direction)
    {
        if(Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.y))
        {
            if(agent.velocity.x < 0)
            {
                SetAnimFloat(Vector2.left);
            } else if(agent.velocity.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
        } else if(Mathf.Abs(agent.velocity.x) < Mathf.Abs(agent.velocity.y))
        {
            if(agent.velocity.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
            else if(agent.velocity.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if(currentState != newState)
        {
            currentState = newState;
        }
    }
}
