using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public BoolValue Level2;
    public BoolValue Level3;
    public void LoadLevel1()
    {
        PlayerPrefs.SetInt("levels", 1);
        SceneManager.LoadScene("Loading");
    }
    public void LoadLevel2()
    {
        if(PlayerPrefs.GetInt("level2") == 1)
        {
            PlayerPrefs.SetInt("levels", 2);
            SceneManager.LoadScene("Loading2");

        }
    }
    public void LoadLevel3()
    {
        if (PlayerPrefs.GetInt("level3") == 1)
        {
            PlayerPrefs.SetInt("levels", 3);
            SceneManager.LoadScene("Loading3");

        }
    }

}
