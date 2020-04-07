using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ganfaul : Brady
{
    public Ganfaul()
    {
        stats = new Stats("Ganfaul");
        description = "The Dark Lord himself. Be careful, he's tough.";

        goldDrop = 200;
        itemsDrop = new Inventory();

        dead = defending = false;
    }
}
