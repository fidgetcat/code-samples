using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mushroom: ItemMaterial
{
    public Mushroom()
    {
        Name = "Mushroom";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Deliciousness"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Deliciousness", 10);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Mushroom();
    }
}
