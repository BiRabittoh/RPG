using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Defend : Ability
{

    //constructor
    public Defend()
    {
        guiName = "Defend";
        hasTarget = false;
        ow_usable = false;
        MP_cost = 0;
    }

    public override string Execute(Fighter source, Fighter target, out bool output)
    {
        source.defending = true;
        source.stats.DEF *= 2;

        output = true;
        return source + " is defending!";
    }

    public override string GetDescription()
    {
        return "Double your defence for one turn.";
    }
}
