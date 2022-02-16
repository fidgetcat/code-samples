using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Drinkable: ItemType
{
    public Drinkable()
    {
        Name = "Drinkable";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Drinkable();
    }
}
