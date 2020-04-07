using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BadGuy : Fighter
{
    [Header("Enemy stuff")]
    public int goldDrop;
    public Inventory itemsDrop;

    public abstract string Combat_AI();

    public override void GetGUI()
    {
        damage_tooltip = GameObject.Find("en_damage");
    }
}
