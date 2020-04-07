using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brady : Boss
{
    public override string Combat_AI()
    {
        throw new System.NotImplementedException();
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
