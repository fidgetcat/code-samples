using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Solvent : ItemType {
    public Solvent()
    {
        Name = "Solvent";
    }
    public override void ApplySelfToAttributes(BaseItem inputItem)
    {

    }

    public override ItemType ReturnCopy()
    {
        return new Solvent();
    }
}
