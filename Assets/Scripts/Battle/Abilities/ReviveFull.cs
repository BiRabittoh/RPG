using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReviveFull : Ability
{
    public ReviveFull()
    {
        guiName = "Miracle";
        hasTarget = true;
        ow_usable = true;
        MP_cost = 200;
    }

    public override string Execute(Fighter source, Fighter target, out bool output)
    {
        if (target.stats.HP == 0)
        {
            target.dead = false;
            target.stats.HP = target.stats.MaxHP;
            UI.ShowDamage(target, (int)target.stats.MaxHP, false); //this should give a negative result
            if (target.animator)
            {
                target.animator.SetTrigger("Alive");
            }
            output = true;
            return target + " was brought back to life and fully healed!";
        }
        else
        {
            output = false;
            return target + " isn't dead yet!";
        }
    }

    public override string GetDescription()
    {
        return "Revives a party member and maxes out all their stats.";
    }
}
