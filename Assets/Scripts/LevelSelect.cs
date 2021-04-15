using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    //public BoolValue Level2;
    //public BoolValue Level3;
    private GameObject level1;
    private GameObject level2;
    private GameObject level3;
    private Color32 originalColor;
    void Start()
    {
        level1 = GameObject.Find("Level1");
        level2 = GameObject.Find("Level2");
        level3 = GameObject.Find("Level3");
        originalColor = level1.GetComponent<Image>().color;
    }
    void Update()
    {
        if (PlayerPrefs.GetInt("level2") == 1)
        {
            level2.GetComponent<Image>().color = originalColor;
        }
        if (PlayerPrefs.GetInt("level3") == 1)
        {
            level3.GetComponent<Image>().color = originalColor;
        }
    }

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

    public void Back()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
