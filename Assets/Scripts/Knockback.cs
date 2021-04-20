using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;

    [SerializeField]
    NavMeshSurface2d[] navMeshSurfaces;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            Debug.Log("smash pot");
            collision.GetComponent<pot>().Smash();


        }

        if (collision.gameObject.CompareTag("chest") && this.gameObject.CompareTag("Player"))
        {

            collision.GetComponent<chest>().Smash();


        }



        /*if (collision.gameObject.CompareTag("bullet_p1"))
        {
            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if(hit != null)
            {
                float boost = Component.FindObjectOfType<PlayerMovement>().attackBoost;
                hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                collision.GetComponent<Enemy>().Knock(hit, knockTime, damage * boost);
            }
            
        }*/

        if (this.gameObject.CompareTag("reaper") && collision.gameObject.CompareTag("Player"))
        {

            return;
        }

        if (this.gameObject.CompareTag("firesword") && collision.gameObject.CompareTag("Player"))
        {

            return;
        }
        if (this.gameObject.CompareTag("icesword") && collision.gameObject.CompareTag("Player"))
        {

            return;
        }





        if (collision.gameObject.CompareTag("Enemies") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("EnemyTag_Ghost") || collision.gameObject.CompareTag("EnemyTag_SlimeLava"))
        {

            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if (!(this.gameObject.CompareTag("Enemies") && (collision.gameObject.CompareTag("Enemies") || collision.gameObject.CompareTag("EnemyTag_Ghost") || collision.gameObject.CompareTag("EnemyTag_SlimeLava"))))
                {

                    Vector2 difference = hit.transform.position - transform.position;
                    difference = difference.normalized * thrust;
                    hit.AddForce(difference, ForceMode2D.Impulse);



                    if ((collision.gameObject.CompareTag("Enemies") || collision.gameObject.CompareTag("EnemyTag_Ghost") || collision.gameObject.CompareTag("EnemyTag_SlimeLava")) && collision.isTrigger)
                    {
                        
                        if (collision.GetComponent<Enemy>().currentState != EnemyState.stagger)
                        {
                            float boost = Component.FindObjectOfType<PlayerMovement>().attackBoost;
                            if (collision.gameObject.CompareTag("EnemyTag_Ghost"))
                            {
                                hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                                collision.GetComponent<Enemy>().Knock(hit, knockTime, 0);
                            }
                            else
                            {
                                hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                                collision.GetComponent<Enemy>().Knock(hit, knockTime, damage * boost);
                            }

                        }

                    }
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        if (!(this.gameObject.CompareTag("bullet_p1")))
                        {
                            Debug.Log("You want die?");
                            if (collision.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                            {
                                hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                                collision.GetComponent<PlayerMovement>().Knock(hit, knockTime, damage);
                            }
                        }

                    }
                }




            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {

            //int retVal = await DoSomeAsync(collision);
            //collision.GetComponent<pot>().Smash();
            for (int i = 0; i < navMeshSurfaces.Length; i++)
            {

                Debug.Log("hit");
                navMeshSurfaces[i].BuildNavMesh();

            }


        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<pot>().Smash();
        }

        if (collision.gameObject.CompareTag("chest") && this.gameObject.CompareTag("Player"))
        {

            collision.GetComponent<chest>().Smash();


        }

        if (this.gameObject.CompareTag("Enemies") && collision.gameObject.CompareTag("Enemies"))
        {

            return;
        }

        if (this.gameObject.CompareTag("reaper") && collision.gameObject.CompareTag("Player"))
        {

            return;
        }

        if (this.gameObject.CompareTag("firesword") && collision.gameObject.CompareTag("Player"))
        {

            return;
        }
        if (this.gameObject.CompareTag("icesword") && collision.gameObject.CompareTag("Player"))
        {

            return;
        }

        if (collision.gameObject.CompareTag("Enemies") || collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hold your fire");
            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);


                if (collision.gameObject.CompareTag("Player"))
                {
                    if (!(this.gameObject.CompareTag("bullet_p1")))
                    {
                        if (collision.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                        {
                            hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                            collision.GetComponent<PlayerMovement>().Knock(hit, knockTime, damage);
                        }
                    }


                }

            }
        }
    }

}
