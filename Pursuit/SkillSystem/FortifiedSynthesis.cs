using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FortifiedSynthesis : Skill
{
    public FortifiedSynthesis()
    {
        cost = 5;
        SkillDescription = "Moderate quality and progress";
        SkillImage = "Synthesis II";
        Name = "Fortified Synthesis";
        IsPassive = false;
    }

    public override void CraftingFunction(CraftingManager inManager, SkillModifier mod)
    {
        if (inManager.Stamina >= CalculateCost(mod))
        {
            float qualityadd = 2 * CurrentLevel;
            float progressadd = 2 * CurrentLevel;

            qualityadd += mod.QualityFlat;
            qualityadd *= mod.QualityMult;

            progressadd += mod.ProgressFlat;
            progressadd *= mod.ProgressMult;

            mod.ApplySpecialModifiers(ref qualityadd, ref progressadd);

            inManager.ModifyQuality(qualityadd);
            inManager.ModifyProgress(progressadd);
            inManager.ModifyStamina(-CalculateCost(mod));
        }
    }
}
