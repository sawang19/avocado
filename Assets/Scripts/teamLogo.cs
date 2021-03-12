using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teamLogo : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Logo1, Logo2, Logo3, Logo4, Logo5, Logo6, Logo7, Logo8, Logo9, Logo10, Logo11, Logo12, Logo13, Logo14,
        Logo15, Logo16, Logo17, Logo18, Logo19, Logo20, Logo21, Logo22, Logo23, Logo24;
    public float wait_time = 0.1f;
    void Start()
    {

        StartCoroutine(Wait_for_splash());
    }

    IEnumerator Wait_for_splash()
    {
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("teamLogo");
        foreach (GameObject go in gameObjectArray)
        {
            go.SetActive(false);


        }

        Logo1.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        
        Logo1.SetActive(false);
        Logo2.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo2.SetActive(false);
        Logo3.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo3.SetActive(false);
        Logo4.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo4.SetActive(false);
        Logo5.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo5.SetActive(false);
        Logo6.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo6.SetActive(false);
        Logo7.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo7.SetActive(false);
        Logo8.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo8.SetActive(false);
        Logo9.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo9.SetActive(false);
        Logo10.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo10.SetActive(false);
        Logo11.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo11.SetActive(false);
        Logo12.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo12.SetActive(false);
        Logo13.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo13.SetActive(false);
        Logo14.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo14.SetActive(false);
        Logo15.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo15.SetActive(false);
        Logo16.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo16.SetActive(false);
        Logo17.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo17.SetActive(false);
        Logo18.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo18.SetActive(false);
        Logo19.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo19.SetActive(false);
        Logo20.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo20.SetActive(false);
        Logo21.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo21.SetActive(false);
        Logo22.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo22.SetActive(false);
        Logo23.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo23.SetActive(false);
        Logo24.SetActive(true);
        yield return new WaitForSeconds(wait_time);
        Logo24.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);





    }
}
