using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI, Pausemenu, PauseButton;
    
    public void Pause()
    {
        PauseUI.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0;
        

    }
    public void Resume()
    {
        PauseUI.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1;

    }

    public void ExitButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

}