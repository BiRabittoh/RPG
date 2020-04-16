using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brady : BadGuy
{
    public Brady()
    {
        stats = new Stats("Brady");
        description = "Son of the dark lord, he's not too bad.";

        goldDrop = 150;
        itemsDrop = new Inventory();
        itemsDrop.generateItem(new ItemInfo(new BottledBlessing(), 1));

        dead = defending = false;
    }
    
    public override string Combat_AI()
    {
        bool output;
        int res;
        Ability ability;
        Fighter target;
        do {
            res = Random.Range(1, 101);
            Debug.Log(res);
            if (res < 70)
            {
                output = true;
                //attack
                Fighter[] ggs = GameObject.Find("BattleManager").GetComponent<BattleManager>().getGuys(false); //TODO: try FindObjectOfType
            
                /*
                List<Fighter> goodGuys = new List<Fighter>();
                foreach(Fighter f in ggs){
                    if(!f.dead)
                        goodGuys.Add(f);
                }
                */

                target = ggs[Random.Range(0, ggs.Length)];

                if(res < 35)
                {
                    //basic
                    ability = new Attack();
                } else {
                    //magic
                    ability = new FireBall();
                }
            } else
            {
                //heal
                ability = new HealGreat();
                target = this;
                if(this.stats.HP < this.stats.MaxHP)
                {
                    output = true;
                } else {
                    output = false;
                }
            }
        } while (output == false);
        
        
        return AbilityDB.Process(this, ability, target, true, out output);
    }
}
