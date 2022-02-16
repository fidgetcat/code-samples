using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PotionSchema : Schema {

    public PotionSchema()
    {
        ItemImage = "poshn";
        Name = "Potion";
        Types.Add(new Drinkable());
        ItemComponent s1 = new ItemComponent();
        s1.Types.Add(new Reagent());
        ItemComponent r1 = new ItemComponent();
        r1.Types.Add(new Egg());
        r1.Types.Add(new Crystal());
        r1.Types.Add(new Ore());
        Components.Add("Reagent", r1);
        Components.Add("Catalyst", s1);
    }
}

[Serializable]
public class SwordSchema : Schema
{

    public SwordSchema()
    {
        ItemImage = "iSword";
        Name = "Sword";
        Types.Add(new Weapon());
        ItemComponent s1 = new ItemComponent();
        s1.Types.Add(new Ore());
        s1.Types.Add(new Crystal());
        ItemComponent r1 = new ItemComponent();
        r1.Types.Add(new Lumber());
        Components.Add("Handle", r1);
        Components.Add("Blade", s1);
    }
}

public class PantsSchema : Schema
{

    public PantsSchema()
    {
        ItemImage = "fPants";
        Name = "Pants";
        Types.Add(new Clothing());
        ItemComponent s1 = new ItemComponent();
        s1.Types.Add(new Fabric());
        ItemComponent r1 = new ItemComponent();
        r1.Types.Add(new Reagent());
        r1.Types.Add(new Ore());
        Components.Add("Embellishment", r1);
        Components.Add("Base", s1);
    }
}

public class Ring : Schema
{

    public Ring()
    {
        ItemImage = "fRing";
        Name = "Ring";
        Types.Add(new Jewelry());
        ItemComponent s1 = new ItemComponent();
        s1.Types.Add(new Crystal());
        ItemComponent r1 = new ItemComponent();
        r1.Types.Add(new Ore());
        r1.Types.Add(new Lumber());
        Components.Add("Setting", r1);
        Components.Add("Piece", s1);
    }
}
