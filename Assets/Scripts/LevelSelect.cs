using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public BoolValue Level2;
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Loading");
    }
    public void LoadLevel2()
    {
        if(Level2.initialValue)
        {
            SceneManager.LoadScene("Loading2");

        }
    }

}
