using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Torch : MonoBehaviour
{
    public UnityEngine.Experimental.Rendering.Universal.Light2D TL;
    public GameObject tl;
    public UnityEngine.Experimental.Rendering.Universal.Light2D BL;
    public GameObject bl;

    // Start is called before the first frame update
    void Start()
    {
        tl = GameObject.Find("torchlight");
        TL = tl.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        bl = GameObject.Find("backgroundlight");
        BL = bl.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        if (BL.intensity >= 2f)
        {
            tl.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (BL.intensity >= 2f)
        {
            tl.SetActive(false);
        }
        else if (BL.intensity < 1f)
        {
            tl.SetActive(true);
        }
    }
}
