using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SwiftTouch : Skill
{
    public SwiftTouch()
    {
        cost = 20;
        SkillDescription = "Multiplies progress output for three skill uses";
        SkillImage = "Swift Touch v 2";
        Name = "Swift Touch";
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
        
        float modamt = 1f + (0.2f * CurrentLevel);
        Debug.Log(Name + modamt.ToString());
        mod.ProgressMult *= modamt;

    }
}
