using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class SpeedReaction : MonoBehaviour
{
    public FloatValue playerSpeed;

    public void Use()
    {
        playerSpeed.runtimeValue *= 1.5f;
        Timer t = new Timer(TimerCallback, null, 5000, 0);
    }

    private void TimerCallback(object o)
    {
        playerSpeed.runtimeValue = playerSpeed.initialValue;
    }
}