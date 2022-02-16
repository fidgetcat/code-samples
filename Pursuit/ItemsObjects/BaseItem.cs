using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseItem{
    public int Value { get; set; }
    public float Quality { get; set; }
    public string ItemImage { get; set; }
    public string ItemName { get; set; }
    public List<ItemMaterial> Materials { get; set; }
    public List<ItemType> Types { get; set; }
    public Dictionary<string, int> ItemAttributes { get; set; }

    public BaseItem()
    {
        ItemAttributes = new Dictionary<string, int>();
        Materials = new List<ItemMaterial>();
        Types = new List<ItemType>();
        ItemName = "Default";
        Value = 1;
        Quality = 1;
        ItemImage = "heman";
    }

    public string GiveAttributes()
    {
        string thingy = "";
        foreach(KeyValuePair<string,int> thing in ItemAttributes)
        {
            thingy += thing.Key;
            thingy += ": ";
            thingy += thing.Value;
            thingy += "\n";
        }
        return thingy;
    }

    public string GiveItemInfo()
    {
        string thingy = "";

        thingy += ItemName;
        thingy += "\nValue: ";
        thingy += Value.ToString();
        thingy += " Quality: ";
        thingy += Quality.ToString();
        thingy += "%";
        thingy += "\n\nTypes:\n";
        foreach (ItemType item in Types)
        {
            thingy += item.Name;
            thingy += "\n";
        }
        thingy += "\nThis "+ItemName+" is made of:\n";
        foreach (ItemMaterial item in Materials)
        {
            thingy += item.Name;
            thingy += "\n";
        }
        thingy += "\nItem Attributes:\n" + GiveAttributes();

        return thingy;
    }
}

public class CopperOre : BaseItem
{
    public CopperOre()
    {
        ItemName = "Copper Ore";
        Value = 20;
        Types.Add(new RawItem());
        Types.Add(new Ore());
        Materials.Add(new Copper());
        ItemImage = "core";
    }
}

public class IronOre : BaseItem
{
    public IronOre()
    {
        ItemName = "Iron Ore";
        Value = 80;
        Types.Add(new RawItem());
        Types.Add(new Ore());
        Materials.Add(new Iron());
        ItemImage = "core";
    }
}

public class OakSlab : BaseItem
{
    public OakSlab()
    {
        ItemName = "Oak Slab";
        Value = 20;
        Types.Add(new RawItem());
        Types.Add(new Lumber());
        Materials.Add(new Oak());
        ItemImage = "wud";
    }
}

public class WillowSlab : BaseItem
{
    public WillowSlab()
    {
        ItemName = "Willow Slab";
        Value = 80;
        Types.Add(new RawItem());
        Types.Add(new Lumber());
        Materials.Add(new Willow());
        ItemImage = "wud";
    }
}

public class RedShroom : BaseItem
{
    public RedShroom()
    {
        ItemName = "Red Shroom";
        Value = 20;
        Types.Add(new RawItem());
        Types.Add(new Reagent());
        Materials.Add(new Redshroom());
        ItemImage = "shrum";
    }
}

public class OrangeShroom : BaseItem
{
    public OrangeShroom()
    {
        ItemName = "Orange Shroom";
        Value = 80;
        Types.Add(new RawItem());
        Types.Add(new Reagent());
        Materials.Add(new Orangeshroom());
        ItemImage = "shrum";
    }
}

public class CowLeather : BaseItem
{
    public CowLeather()
    {
        ItemName = "Cow Leather";
        Value = 20;
        Types.Add(new RawItem());
        Types.Add(new Fabric());
        Materials.Add(new Cowleather());
        ItemImage = "Leather";
    }
}

public class LizardLeather : BaseItem
{
    public LizardLeather()
    {
        ItemName = "Lizard Leather";
        Value = 80;
        Types.Add(new RawItem());
        Types.Add(new Fabric());
        Materials.Add(new Lizardleather());
        ItemImage = "Leather";
    }
}

public class ChickenEgg : BaseItem
{
    public ChickenEgg()
    {
        ItemName = "Chicken Egg";
        Value = 20;
        Types.Add(new RawItem());
        Types.Add(new Egg());
        Materials.Add(new Chickenegg());
        ItemImage = "Egg";
    }
}

public class DuckEgg : BaseItem
{
    public DuckEgg()
    {
        ItemName = "Duck Egg";
        Value = 80;
        Types.Add(new RawItem());
        Types.Add(new Egg());
        Materials.Add(new Duckegg());
        ItemImage = "Egg";
    }
}

public class AmethystGem : BaseItem
{
    public AmethystGem()
    {
        ItemName = "Amethyst Gem";
        Value = 200;
        Types.Add(new RawItem());
        Types.Add(new Crystal());
        Materials.Add(new Amethyst());
        ItemImage = "Gem";
    }
}

public class TopazGem : BaseItem
{
    public TopazGem()
    {
        ItemName = "Topaz Gem";
        Value = 500;
        Types.Add(new RawItem());
        Types.Add(new Crystal());
        Materials.Add(new Topaz());
        ItemImage = "Gem";
    }
}