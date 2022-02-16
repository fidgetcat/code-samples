using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class ItemType{
    public string Name { get; set; }

    public virtual void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public virtual ItemType ReturnCopy()
    {
        return new ItemType();
    }
}

public class Fabric : ItemType
{
    public Fabric()
    {
        Name = "Fabric";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Solvent();
    }
}

public class Weapon : ItemType
{
    public Weapon()
    {
        Name = "Weapon";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Weapon();
    }
}

public class Ore : ItemType
{
    public Ore()
    {
        Name = "Ore";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Ore();
    }
}

public class Lumber : ItemType
{
    public Lumber()
    {
        Name = "Lumber";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Lumber();
    }
}

public class Reagent : ItemType
{
    public Reagent()
    {
        Name = "Reagent";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Reagent();
    }
}

public class Egg : ItemType
{
    public Egg()
    {
        Name = "Egg";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Egg();
    }
}

public class Crystal : ItemType
{
    public Crystal()
    {
        Name = "Crystal";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Crystal();
    }
}

public class Clothing : ItemType
{
    public Clothing()
    {
        Name = "Clothing";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Clothing();
    }
}

public class Jewelry : ItemType
{
    public Jewelry()
    {
        Name = "Jewelry";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Jewelry();
    }
}


