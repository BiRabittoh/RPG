using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Heal : Ability
{
    protected double heal_amount;

    public Heal()
    {
        dbName = "Heal";
        guiName = "Heal";
        hasTarget = true;
        ow_usable = true;
        MP_cost = 50;
        heal_amount = 200;
    }

    public override string Execute(Fighter source, Fighter target, out bool output)
    {
        double amount = heal_amount;
        output = false;
        if(target.stats.HP <= 0)
        {
            return source + " can't heal " + target + " because they're dead!";
        }
        if(target.stats.HP == target.stats.MaxHP)
        {
            return target + " already has max HP, they don't need healing.";
        }
        amount += (amount * 0.05);
        target.stats.HP += amount;
        UI.ShowDamage(target, -(int)amount, false); //this should give a negative result
        output = true;
        return target + " recovered " + (int)amount + " HP.";
        
    }

    public override string GetDescription()
    {
        return "Heals about " + heal_amount + "HP to one target.";
    }
}
