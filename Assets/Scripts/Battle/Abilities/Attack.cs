using System;
using UnityEngine;

[System.Serializable]
public class Attack : Ability
{
    //constructor
    public Attack()
    {
        guiName = "Attack";
        hasTarget = true;
        ow_usable = false;
        MP_cost = 0;
    }
    
    public override string Execute(Fighter source, Fighter target, out bool output)
    {
        if(target.stats.HP == 0)
        {
            output = false;
            return target + " is already dead!";
        }
        string str = source + " attacks " + target;
        //number logic
        double prob, crit = 0, damage = 0;
        bool didCrit = false;

        prob = 3 * Math.Log(source.stats.AGL / target.stats.AGL) + 87;
        if (UnityEngine.Random.Range(0, 100) >= prob) // prob = probability to hit; using > is like inverting the logic
        {
            str += " (Miss...)";
        }
        else
        {
            //check critical
            prob = ((31 * source.stats.LCK) + 200) / 50;
            if (UnityEngine.Random.Range(0, 100) <= prob)
            {
                crit = source.stats.ATK * 3;
                str += " (Critical!)";
                didCrit = true;
            }
            
            damage = 27.241 * Math.Log(source.stats.ATK / source.stats.DEF) + 148.78 + crit;

            damage += (damage * UnityEngine.Random.Range(-5, 5) / 100);
            
            target.stats.HP -= damage;
        }
        UI.ShowDamage(target, (int)damage, didCrit);
        source.animator.SetTrigger("Attack");
        output = true;
        return str;
    }

    public override string GetDescription()
    {
        return "A normal physical attack.";
    }
}
