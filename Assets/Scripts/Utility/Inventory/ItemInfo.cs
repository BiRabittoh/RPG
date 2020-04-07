using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInfo : Action
{
    public Item item;
    public int quantity;

    public ItemInfo(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public override string Execute(Fighter source, Fighter target, out bool output)
    {
        if (item.effect != null)
        {
            return AbilityDB.Process(source, item.effect, target, false, out output);
        }
        else
        {
            output = false;
            return "This item has no effect in battle!";
        }
    }

    public override string ExecuteSafe(Fighter source, Fighter target, out bool output)
    {
        if (item.effect != null)
        {
            if (quantity == 0)
            {
                output = false;
                return "You don't have any " + item.name + "!";
            }
            else
            {
                string tmp = Execute(source, target, out output);
                if (item.consumable && output)
                {
                    quantity -= 1;
                }
                return tmp;
            }
        } else
        {
            output = false;
            return "This item has no effect in battle!";
        }
        
    }

    public override string GetDescription()
    {
        return item.description + "\n" + item.effect.GetDescription();
    }

    public override string ToString()
    {
        if(quantity == -1)
        {
            return item.name;
        }
        return item.name + " : " + quantity;
    }
}
