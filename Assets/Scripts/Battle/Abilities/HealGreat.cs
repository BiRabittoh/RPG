using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealGreat : Heal
{
    public HealGreat()
    {
        dbName = "HealGreat";
        guiName = "Giga Heal";
        hasTarget = true;
        ow_usable = true;
        MP_cost = 150;
        heal_amount = 500;
    }
}
