using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventoryButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventoryUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openInventory()
    {
        Time.timeScale = 0;
        inventoryUI.SetActive(true);
    }
}
