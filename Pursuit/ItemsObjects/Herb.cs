using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Herb : ItemMaterial {
    public Herb()
    {
        Name = "Herb";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {
        if (inputItem.ItemAttributes.ContainsKey("Efficacy"))
        {

        }
        else
        {
            inputItem.ItemAttributes.Add("Efficacy", 30);
        }
    }

    public override ItemMaterial ReturnCopy()
    {
        return new Herb();
    }
}
