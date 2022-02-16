using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Synthesized : Skill
{
    public Synthesized()
    {
        cost = 0;
        SkillDescription = "Increases progress output by a factor";
        SkillImage = "Synthesis";
        Name = "Synthesized";
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
            mod.ProgressMult *= modamt;
        }
    }
}
