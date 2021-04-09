using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameWin : MonoBehaviour
{
    public BoolValue Level2_available;
    public BoolValue Level3_available;
    public void Setup() {
        if (PlayerPrefs.GetInt("levels") == 1)
        {
            PlayerPrefs.SetInt("level2", 1);
        }
        if (PlayerPrefs.GetInt("levels") == 2)
        {
            PlayerPrefs.SetInt("level3", 1);
        }


        gameObject.SetActive(true);
    }

    public void RestartButton() {
    	SceneManager.LoadScene("SampleScene");
    }
    public void RestartButton2()
    {
        SceneManager.LoadScene("SampleScene2");
    }
    public void RestartButton3()
    {
        SceneManager.LoadScene("SampleScene3");
    }

    public void ExitButton() {
        SceneManager.LoadScene("MenuScene");
    }
    public void NextGameButton() {
        Time.timeScale = 1;
        if (PlayerPrefs.GetInt("levels") == 1)
        {
            PlayerPrefs.SetInt("levels", 2);
            SceneManager.LoadScene("Loading2");
        }
        else if (PlayerPrefs.GetInt("levels") == 2)
        {
            PlayerPrefs.SetInt("levels", 3);
            SceneManager.LoadScene("Loading3");
        }
        
    }
}
