using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FireBall : Ability
{
    public FireBall()
    {
        dbName = "FireBall";
        guiName = "Fireball";
        hasTarget = true;
        ow_usable = false;
        MP_cost = 70;
    }
    public override string Execute(Fighter source, Fighter target, out bool output)
    {
        if(target.stats.HP == 0)
        {
            output = false;
            return target + " is already dead!";
        }
        string str = source + " throws a fireball at " + target + "!";
        double damage = 0;

        //TODO: number logic
        damage = 300;

        damage += (damage * UnityEngine.Random.Range(-5, 5) / 100);
        target.stats.HP -= damage;

        UI.ShowDamage(target, (int)damage, false);
        source.animator.SetTrigger("MagicAttack");
        output = true;
        return str;
    }

    public override string GetDescription()
    {
        return "A strong, fire-based, magic projectile.";
    }
}
