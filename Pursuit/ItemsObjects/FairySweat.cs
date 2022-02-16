using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FairySweat : ItemMaterial
{
    public FairySweat()
    {
        Name = "Fairy Sweat";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Magicalness"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Magicalness", 7);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new FairySweat();
    }
}
