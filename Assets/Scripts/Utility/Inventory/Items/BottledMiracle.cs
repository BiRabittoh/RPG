using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BottledMiracle : Item
{
    public BottledMiracle()
    {
        name = "Miracle in a bottle";
        price = 350;
        effect = new ReviveFull();
        consumable = true;
        description = "Somebody managed to fit a whole miracle inside this bottle.";
    }
}
