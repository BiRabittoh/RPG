using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotionBig : Item
{
    public PotionBig()
    {
        name = "Big potion";
        price = 60;
        effect = new HealBig();
        consumable = true;
        description = "It smells like pickles.";
    }
}
