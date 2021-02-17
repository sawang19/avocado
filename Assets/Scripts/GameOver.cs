using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
	public Text pointsText;
    public void Setup() {
    	gameObject.SetActive(true);
    }

    public void RestartButton() {
    	SceneManager.LoadScene("SampleScene");
    }
    public void ExitButton() {
        SceneManager.LoadScene("MenuScene");
    }
}
