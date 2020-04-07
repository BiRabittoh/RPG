using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Nothing : Item
{
    public Nothing()
    {
        name = "<Empty>";
        price = -1;
        effect = null;
        consumable = false;
        description = "Nothing.";
    }
}
