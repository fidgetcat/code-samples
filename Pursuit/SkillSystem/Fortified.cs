using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Fortified : Skill
{
    public Fortified()
    {
        cost = 0;
        SkillDescription = "Increases quality output by a factor";
        SkillImage = "Fortified";
        Name = "Fortified";
        IsPassive = true;
        IsInstant = true;
    }

    public override void CraftingFunction(CraftingManager inManager, SkillModifier mod)
    {
        //butt
    }

    public override void AffectFunction(CraftingManager inManager, SkillModifier mod)
    {
        if (inManager.Stamina >= CalculateCost(mod))
        {
            float modamt = 1f + (0.03f * CurrentLevel);
            mod.QualityMult *= modamt;
        }
    }

    public override void PassiveFunction(CraftingManager inManager)
    {
        inManager.CurrentAffects.Add(new SkillAffect(this.Name, -1,SkillDescription,SkillImage));
    }
}
