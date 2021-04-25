using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI, Pausemenu, PauseButton;
    public Slider mSlider;

    void Update()
    {
        mSlider.value = PlayerPrefs.GetFloat("Volume");
    }

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

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }

}