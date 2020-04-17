using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealBig : Heal
{
    public HealBig()
    {
        guiName = "Mega Heal";
        hasTarget = true;
        ow_usable = true;
        MP_cost = 100;
        heal_amount = 350;
    }
}
