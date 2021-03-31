using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Setting, Menu;
    public Slider mSlider;

    void Update()
    {
        mSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    public void OpenSetting()
    {
        Setting.SetActive(true);
        Menu.SetActive(false);
    }

    public void CloseSetting()
    {
        Setting.SetActive(false);
        Menu.SetActive(true);

    }
    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }

}
