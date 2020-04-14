using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int currentLevel;
    public float posx, posy, posz;
    public float rot0, rot1, rot2, rot3;
    public List<string> enemiesKilled;
    public Dictionary<string, Stats> party;
    public List<ItemInfo> inventory;
    public int gold;
    public string fighting;
    public DateTime timestamp;
    public float timerfloat;
}
