using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReviveHalf : Ability
{
    public ReviveHalf()
    {
        dbName = "ReviveHalf";
        guiName = "Blessing";
        hasTarget = true;
        ow_usable = true;
        MP_cost = 100;
    }

    public override string Execute(Fighter source, Fighter target, out bool output)
    {
        if (target.stats.HP == 0)
        {
            double tmp = target.stats.HP;
            target.dead = false;
            target.stats.HP = (target.stats.MaxHP / 2) + (target.stats.MaxHP * 0.05);
            UI.ShowDamage(target, (int)(tmp - target.stats.HP), false); //this should give a negative result
            if(target.animator)
                target.animator.SetTrigger("Alive");
            output = true;
            return target + " was brought back to life!";
        } else
        {
            output = false;
            return target + " isn't dead yet!";
        }
    }

    public override string GetDescription()
    {
        return "Revives a party member with 50% of his health.";
    }
}
