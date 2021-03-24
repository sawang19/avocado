using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SliderControl : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer audioMixer;
    public GameObject Setting, Menu;
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
        audioMixer.SetFloat("Volume", volume);
    }

}
