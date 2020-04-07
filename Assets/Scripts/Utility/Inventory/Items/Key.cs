using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Key : Item
{
    public Key()
    {
        name = "Key";
        price = 32;
        effect = null;
        consumable = false;
        description = "This should open a door somewhere.";
    }
}
