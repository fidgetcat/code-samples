using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class RawItem: ItemType
{
    public RawItem()
    {
        Name = "Raw Item";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new RawItem();
    }
}
