using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutant : Orc
{
    public Mutant()
    {
        stats = new Stats("Mutant");
        description = "He's kinda strong, but still as dumb as a rock.";

        goldDrop = 60;
        itemsDrop = new Inventory();
        itemsDrop.generateItem(new ItemInfo(new Potion(), 1));
        //itemsDrop.generateItem(new ItemInfo(new Key(), 1));

        dead = defending = false;
    }
}
