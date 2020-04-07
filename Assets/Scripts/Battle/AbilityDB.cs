using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class AbilityDB
{
    
    private static string processAbility(Fighter f1, Ability ability, Fighter f2, bool checkMP, out bool output)
    {
        string tmp;
        if (checkMP)
        {
            tmp = ability.ExecuteSafe(f1, f2, out output);
        }
        else
        {
            tmp = ability.Execute(f1, f2, out output);
        }
        
        //Update Fighter status
        updateGui(f1);
        updateGui(f2);

        return tmp;
    }

    private static string processString(Fighter f1, string ability, Fighter f2, bool checkMP, out bool output)
    {
        return processAbility(f1, getAbility(ability), f2, checkMP, out output);
    }
    private static string processItem(Fighter f1, ItemInfo item, Fighter f2, out bool output)
    {
        return item.ExecuteSafe(f1, f2, out output);
    }

    public static string Process(Fighter f1, Action action, Fighter f2, bool checkMP, out bool output)
    {
        if(action is Ability)
        {
            return processAbility(f1, (Ability)action, f2, checkMP, out output);
        } else if (action is ItemInfo)
        {
            return processItem(f1, (ItemInfo)action, f2, out output);
        } else
        {
            output = false;
            return "Error in AbilityDB!";
        }
    }

    public static Ability getAbility(string key)
    {
        switch (key)
        {
            case "Attack":
                return new Attack();
            case "Defend":
                return new Defend();
            case "Heal":
                return new Heal();
            case "HealBig":
                return new HealBig();
            case "HealGreat":
                return new HealGreat();
            case "ReviveHalf":
                return new ReviveHalf();
            case "ReviveFull":
                return new ReviveFull();
            default:
                return null;
        }
    }
    
    private static void updateGui(Fighter f)
    {
        if(f is GoodGuy)
        {

            GoodGuy gg = (GoodGuy)f;

            if(gg.inBattle)
                gg.updateStatus();
        }
    }
    
    public static GoodGuy GetGoodGuy(GameObject partyObject, Stats s)
    {
        GoodGuy gg = null;

        switch (s.Name)
        {
            case "Paladin":
                gg = partyObject.AddComponent<Paladin>();
                break;
            case "Archer":
                gg = partyObject.AddComponent<Archer>();
                break;
            case "Boxer":
                gg = partyObject.AddComponent<Boxer>();
                break;
            case "Mage":
                gg = partyObject.AddComponent<Mage>();
                break;
            default:
                break;
        }
        gg.inBattle = false;

        return gg;
    }


}
