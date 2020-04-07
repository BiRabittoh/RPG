using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Potion : Item
{
    public Potion()
    {
        name = "Potion";
        price = 25;
        effect = new Heal();
        consumable = true;
        description = "It smells like sulfur.";
    }
}
