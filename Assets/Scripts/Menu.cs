using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public BoolValue level2;
    public BoolValue level3;

    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        FindObjectOfType<AudioManager>().Play("MenuScene");
    }

    public void PlayGame()
    {
        //if(level3.initialValue)
        //{
        //    PlayerPrefs.SetInt("levels", 3);
        //    SceneManager.LoadScene("Loading3");

        //} else if(level2.initialValue)
        //{
        //    PlayerPrefs.SetInt("levels", 2);
        //    SceneManager.LoadScene("Loading2");
        //}
        //else
        //{
        //    PlayerPrefs.SetInt("levels", 1);
        //    SceneManager.LoadScene("Loading");
        //}
        if (PlayerPrefs.GetInt("level3") == 1)
        {
            PlayerPrefs.SetInt("levels", 3);
            SceneManager.LoadScene("Loading3");

        }
        else if (PlayerPrefs.GetInt("level2") == 1)
        {
            PlayerPrefs.SetInt("levels", 2);
            SceneManager.LoadScene("Loading2");
        }
        else
        {
            PlayerPrefs.SetInt("levels", 1);
            SceneManager.LoadScene("Loading");
        }

    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("Levels");
    }

    
    //public void LoadLevel(int sceneIndex)
    //{
    //    StartCoroutine(LoadSceneAsyncronously(sceneIndex));
    //}

    //IEnumerator LoadSceneAsyncronously(int sceneIndex)
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
    //    loadingScreen.SetActive(true);
    //    while (!operation.isDone)
    //    {
    //        float progress = Mathf.Clamp01(operation.progress / .9f);
    //        slider.value = progress;
    //        yield return null;
    //    }

    //}
}
