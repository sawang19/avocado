using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpMenu : MonoBehaviour
{
    public GameObject HelpPanel, HelpButton;
    public GameObject itemPanel, enemyPanel;
    public Button itemButton, enemyButton;

    public static readonly Color COLORINUSE = new Color32(248, 212, 72, 255);
    public static readonly Color COLORNOTUSE = new Color32(0, 0, 0, 0);

    public void Pause()
    {
        HelpPanel.SetActive(true);
        HelpButton.SetActive(false);
        changeToItemDescription();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        HelpPanel.SetActive(false);
        HelpButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void changeToItemDescription()
    {
        enemyPanel.SetActive(false);
        itemPanel.SetActive(true);
        changeButtonColor(ref itemButton, COLORINUSE);
        changeButtonColor(ref enemyButton, COLORNOTUSE);
    }

    public void changeToEnemyDescription()
    {
        itemPanel.SetActive(false);
        enemyPanel.SetActive(true);
        changeButtonColor(ref enemyButton, COLORINUSE);
        changeButtonColor(ref itemButton, COLORNOTUSE);
    }

    public void changeButtonColor(ref Button button, Color c)
    {
        button.GetComponent<Image>().color = c;
    }

}
