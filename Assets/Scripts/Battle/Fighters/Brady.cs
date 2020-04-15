using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brady : BadGuy
{
    public override string Combat_AI()
    {
        //attack a random goodGuy
        Fighter[] ggs = GameObject.Find("BattleManager").GetComponent<BattleManager>().getGuys(false);
        List<Ability> abs = stats.GetAbilities();
        bool output;
        string str;
        do
        {
            str = AbilityDB.Process(this, abs[Random.Range(0, abs.Count)], ggs[Random.Range(0, ggs.Length)], true, out output);
        } while (output == false);
        return str;
    }

    public Brady()
    {
        stats = new Stats("Brady");
        description = "Son of the dark lord, he's not too bad.";

        goldDrop = 150;
        itemsDrop = new Inventory();
        itemsDrop.generateItem(new ItemInfo(new BottledBlessing(), 1));

        dead = defending = false;
    }
}
