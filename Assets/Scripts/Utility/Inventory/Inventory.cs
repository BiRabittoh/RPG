using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private ItemInfo nothing = new ItemInfo(new Nothing(), -1);

    private Dictionary<string, ItemInfo> content;
    

    //constructor
    public Inventory()
    {
        content = new Dictionary<string, ItemInfo>();
        cleanContent();
    }

    public Inventory(Inventory inventory)
    {
        foreach(ItemInfo item in new List<ItemInfo>(inventory.getContents()))
        {
            Debug.Log("adding " + item.item.name);
            generateItem(item);
            Debug.Log("added " + item.item.name);
        }
    }

    public Inventory(IEnumerable<ItemInfo> list)
    {
        foreach (ItemInfo item in list)
        {
            generateItem(item);
        }
    }

    private void cleanContent()
    {
        List<string> delete = new List<string>();
        foreach (KeyValuePair<string, ItemInfo> kv in content)
        {
            if (kv.Value.quantity == 0)
            {
                delete.Add(kv.Key);
            }
        }

        foreach(string s in delete)
        {
            content.Remove(s);
        }
    }

    public List<ItemInfo> getContents()
    {
        cleanContent();
        List<ItemInfo> tmp = new List<ItemInfo>();
        foreach (KeyValuePair<string, ItemInfo> kv in content)
        {
            if(kv.Value.quantity > 0)
            {
                tmp.Add(kv.Value);
            }
        }

        if (tmp.Count == 0)
        {
            tmp.Add(nothing);
        }

        return tmp;
    }

    public int countItems(string key)
    {
        ItemInfo tmp;
        if (content == null)
        {
            content = new Dictionary<string, ItemInfo>();
            return 0;
        }
        if(content.TryGetValue(key, out tmp))
        {
            return tmp.quantity;
        } else
        {
            return 0;
        }
    }
    
    public void generateItem(ItemInfo ii)
    {
        if(countItems(ii.item.name) <= 0)
        {
            content.Add(ii.item.name, new ItemInfo(ii.item, ii.quantity));
        } else
        {
            content[ii.item.name].quantity += ii.quantity;
        }
        cleanContent();
    }
    public void destroyItem(string key, int quantity)
    {
        if (countItems(key) < 1)
        {
            return;
        }
        else
        {
            if(content[key].quantity < quantity)
            {
                content[key].quantity = 0;
            } else
            {
                content[key].quantity -= quantity;
            }
        }
        cleanContent();
    }
    public ItemInfo readItem(string key)
    {
        ItemInfo tmp;
        if (content.TryGetValue(key, out tmp))
        {
            return tmp;
        }
        else
        {
            return null;
        }
    }

    public void transferItem(string name, Inventory other)
    {
        if (other.countItems(name) < 1)
        {
            return;
        } else
        {
            generateItem(other.readItem(name));
            other.destroyItem(name, other.readItem(name).quantity);
        }
        cleanContent();
    }

    public void addInventory(Inventory inv)
    {
        cleanContent();
        foreach (ItemInfo info in inv.getContents())
        {
            if(!(info.item is Nothing))
            {
                transferItem(info.item.name, inv);
            }
        }
    }
}
