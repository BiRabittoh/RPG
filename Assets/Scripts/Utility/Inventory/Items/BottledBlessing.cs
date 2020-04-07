using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BottledBlessing : Item
{
    public BottledBlessing()
    {
        name = "Blessing in a bottle";
        price = 150;
        effect = new ReviveHalf();
        consumable = true;
        description = "This portable blessing can be easily carried around.";
    }
}
