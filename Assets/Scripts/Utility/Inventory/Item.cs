using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public string name;
    public int price;
    public Ability effect;
    public bool consumable;
    public string description;
}
