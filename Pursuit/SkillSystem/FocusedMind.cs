using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FocusedMind : Skill
{
    public FocusedMind()
    {
        cost = 20;
        SkillDescription = "Multiplies quality output for three skill uses";
        SkillImage = "Focused mind";
        Name = "Focused Mind";
        IsPassive = false;
        IsInstant = true;
    }

    public override void CraftingFunction(CraftingManager inManager, SkillModifier mod)
    {
        if (inManager.Stamina >= CalculateCost(mod))
        {
            inManager.CurrentAffects.Add(new SkillAffect(Name, 3, SkillDescription, SkillImage));
            inManager.ModifyStamina(-CalculateCost(mod));
        }
}

    public override void AffectFunction(CraftingManager inManager, SkillModifier mod)
    {

        float modamt = 1f + (0.2f*CurrentLevel);
        Debug.Log(Name + modamt.ToString());
        mod.QualityMult *= modamt;

    }
}
