using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameWin : MonoBehaviour
{
    public BoolValue Level2_available;
    public void Setup() {
        Level2_available.initialValue = true;
    	gameObject.SetActive(true);
    }

    public void RestartButton() {
    	SceneManager.LoadScene("SampleScene");
    }
    public void RestartButton2()
    {
        SceneManager.LoadScene("SampleScene2");
    }
    //public void RestartButton3()
    //{
    //    SceneManager.LoadScene("SampleScene3");
    //}

    public void ExitButton() {
        SceneManager.LoadScene("MenuScene");
    }
    public void NextGameButton() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Loading2");
    }
}
