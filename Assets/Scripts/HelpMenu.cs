using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenu : MonoBehaviour
{
    public GameObject HelpPanel, HelpButton;
  
    public void Pause()
    {
        HelpPanel.SetActive(true);
        HelpButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        HelpPanel.SetActive(false);
        HelpButton.SetActive(true);
        Time.timeScale = 1f;
    }
}
