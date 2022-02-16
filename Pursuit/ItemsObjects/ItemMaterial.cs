using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class ItemMaterial
{
    public string Name { get; set; }

    public virtual void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public virtual ItemMaterial ReturnCopy()
    {
        return new ItemMaterial();
    }
}

[Serializable]
public class Copper : ItemMaterial
{
    public Copper()
    {
        Name = "Copper";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Hardness"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Hardness", 10);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Copper();
    }
}


[Serializable]
public class Iron : ItemMaterial
{
    public Iron()
    {
        Name = "Iron";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Hardness"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Hardness", 30);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Iron();
    }
}


[Serializable]
public class Oak : ItemMaterial
{
    public Oak()
    {
        Name = "Oak";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Suppleness"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Suppleness", 10);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Oak();
    }
}

public class Willow : ItemMaterial
{
    public Willow()
    {
        Name = "Willow";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Suppleness"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Suppleness", 30);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Willow();
    }
}

public class Redshroom : ItemMaterial
{
    public Redshroom()
    {
        Name = "Red Shroom";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Magic"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Magic", 10);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Redshroom();
    }
}

public class Orangeshroom : ItemMaterial
{
    public Orangeshroom()
    {
        Name = "Orange Shroom";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Magic"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Magic", 30);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Orangeshroom();
    }
}

[Serializable]
public class Cowleather : ItemMaterial
{
    public Cowleather()
    {
        Name = "Cow Leather";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Sturdiness"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Sturdiness", 10);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Cowleather();
    }
}

[Serializable]
public class Lizardleather : ItemMaterial
{
    public Lizardleather()
    {
        Name = "Lizard Leather";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Lizard Leather"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Sturdiness", 30);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Lizardleather();
    }
}

[Serializable]
public class Chickenegg : ItemMaterial
{
    public Chickenegg()
    {
        Name = "Chicken Egg";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Flavor"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Flavor", 10);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Chickenegg();
    }
}

[Serializable]
public class Duckegg : ItemMaterial
{
    public Duckegg()
    {
        Name = "Duck Egg";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Flavor"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Flavor", 30);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Duckegg();
    }
}

[Serializable]
public class Amethyst : ItemMaterial
{
    public Amethyst()
    {
        Name = "Amethyst";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Beauty"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Beauty", 10);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Amethyst();
    }
}

[Serializable]
public class Topaz : ItemMaterial
{
    public Topaz()
    {
        Name = "Topaz";
    }

    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Beauty"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Beauty", 30);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Topaz();
    }
}
