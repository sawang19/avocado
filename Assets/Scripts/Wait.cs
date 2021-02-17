using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    // Start is called before the first frame update
    public float wait_time = 5f;
    void Start()
    {
        StartCoroutine(Wait_for_splash());
    }

    IEnumerator Wait_for_splash()
    {
        yield return new WaitForSeconds(wait_time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
