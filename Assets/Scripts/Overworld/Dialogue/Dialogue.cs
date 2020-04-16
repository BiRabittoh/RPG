﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public AudioClip sound = null;

    [TextArea(3, 10)] public string[] sentences;
}
