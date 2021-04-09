using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader2 : MonoBehaviour
{
    public float FillSpeed = 0.5f;
    private float targetProgress = 0;
    public GameObject loadingScreen;
    public Slider slider;

    
    void Update()
    {
        if (slider.value < targetProgress)
        {
            slider.value += FillSpeed * Time.deltaTime;
        }
        if (slider.value == 1f)
        {
            SceneManager.LoadScene("SampleScene2");
        }

    }
    public void IncrementProgress(float newProgress)
    {
        targetProgress = slider.value + newProgress;
    }
    void Start()
    {
        Time.timeScale = 1;
        IncrementProgress(1f);

    }
}