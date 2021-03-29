using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
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
            SceneManager.LoadScene("SampleScene");
        }

    }
    public void IncrementProgress(float newProgress)
    {
        targetProgress = slider.value + newProgress;
    }
    void Start()
    {
        IncrementProgress(1f);

    }
}