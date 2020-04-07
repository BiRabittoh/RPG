using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timer = 0f;
    public int seconds;

    void Update()
    {
        timer += Time.deltaTime;
        //seconds = (int)timer % 60;
    }
}
