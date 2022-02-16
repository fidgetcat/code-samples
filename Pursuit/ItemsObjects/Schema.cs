using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Schema {
    public string Name { get; set; }
    public string ItemImage { get; set; }
    public List<ItemType> Types { get; set; }
    public Dictionary<string, ItemComponent> Components;


    public Schema()
    {
        ItemImage = "heman";
        Name = "Default";
        Types = new List<ItemType>();
        Components = new Dictionary<string, ItemComponent>();
    }


}

[Serializable]
public class ItemComponent
{
    public ItemComponent()
    {
        Types = new List<ItemType>();
        Materials = new List<ItemMaterial>();
    }

    public List<BaseItem> ReturnEligibleItems()
    {
        Debug.Log("wehew");
        List<BaseItem> listy = new List<BaseItem>();
        foreach (BaseItem thing in GameControl.brain.Player.MyInventory.Items)
        {
            Debug.Log("looped");
            if (CompareComponentToItem(thing))
            {
                Debug.Log("addedded");
                listy.Add(thing);
            }
        }
        return listy;
    }

    public bool CompareComponentToItem(BaseItem input)
    {
        bool typeCheck = false;
        bool matCheck = false;
        if (Types.Count > 0)
        {
            foreach(ItemType thing in Types)
            {
                foreach(ItemType myThing in input.Types)
                {
                    if (myThing.Name==thing.Name)
                    {
                        Debug.Log("typeCheck = true;");
                        typeCheck = true;
                    }
                }
            }
        }
        else
        {
            Debug.Log("typeCheck = true;");
            typeCheck = true;
        }

        if (Materials.Count > 0)
        {
            foreach (ItemMaterial thing in Materials)
            {
                foreach (ItemMaterial myThing in input.Materials)
                {
                    if (thing.Name==myThing.Name)
                    {
                        Debug.Log("matCheck = true;");
                        matCheck = true;
                    }
                }
            }
        }
        else
        {
            Debug.Log("matCheck = true;");
            matCheck = true;
        }

        return typeCheck==true&&matCheck==true;
    }

    public string GiveComponentList()
    {
        string thingy = "";
        thingy += "Component must be a(n): ";
        if (Types.Count < 1){
            thingy += "Anything!";
        }
        else if(Types.Count>=1) {
            foreach (ItemType thing in Types)
            {
                thingy += thing.Name;
                if (Types.Count > 1) {
                    thingy += " OR ";
                }
            }
        }
        thingy += "\n\n";
        thingy += "Component must be made of: ";
        if (Materials.Count < 1)
        {
            thingy += "Anything!";
        }
        else if (Materials.Count >= 1)
        {
            foreach (ItemMaterial thing in Materials)
            {
                thingy += thing.Name;
                if (Materials.Count > 1)
                {
                    thingy += " OR ";
                }
            }
        }

        return thingy;
    }
    public List<ItemType> Types;
    public List<ItemMaterial> Materials;
}
