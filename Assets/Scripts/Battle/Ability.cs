using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Ability : Action
{
    public string dbName;
    public string guiName;
    public bool hasTarget;
    public double MP_cost;
    public bool ow_usable;
    
    public override string ExecuteSafe(Fighter source, Fighter target, out bool output)
    {
        if (source.stats.MP < MP_cost)
        {
            output = false;
            return "Not enough MP!";
        }
        else
        {
            string tmp = Execute(source, target, out output);
            if (output)
            {
                source.stats.MP -= MP_cost;
            }
            return tmp;
        }
    }

    public override string ToString()
    {
        if (MP_cost == 0)
        {
            return guiName;
        } else
        {
            return guiName + " (-" + MP_cost + "MP)";
        }
    }
}
