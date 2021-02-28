using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxScript : MonoBehaviour
{
    public GameObject dialogBox;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void closeDialogue()
    {
        // if (dialogBox.activeInHierarchy) 
        // {
        dialogBox.SetActive(false);
        Time.timeScale = 1;
        // }
    }
}
