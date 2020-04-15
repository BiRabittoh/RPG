using System.Collections.Generic;
using UnityEngine;

public class Orc : BadGuy
{
    public Orc() {
        stats = new Stats("Orc");
        description = "Dumb and weak, the most pathetic enemy you can think of.";

        goldDrop = 43;
        itemsDrop = new Inventory();
        itemsDrop.generateItem(new ItemInfo(new Potion(), 1));
        //itemsDrop.generateItem(new ItemInfo(new Key(), 1));
        
        dead = defending = false;
    }

    public override string Combat_AI()
    {
        //attack a random goodGuy
        Fighter[] temp = GameObject.Find("BattleManager").GetComponent<BattleManager>().getGuys(false);
        bool output;
        string str;
        do
        {
            str = AbilityDB.Process(this, new Attack(), temp[Random.Range(0, temp.Length)], true, out output);
        } while (output == false);
        return str;
    }
}

//2.16 0 22.9
//0 180 0