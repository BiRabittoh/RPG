using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotionGreat : Item
{
    public PotionGreat()
    {
        name = "Great potion";
        price = 130;
        effect = new HealGreat();
        consumable = true;
        description = "It smells like candy.";
    }
}
