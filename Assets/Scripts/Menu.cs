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

    public void PlayGame()
    {
        if(level2.initialValue)
        {
            SceneManager.LoadScene("Loading2");
        } else
        {
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
