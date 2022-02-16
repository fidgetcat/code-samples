using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RashFortify : Skill
{
    public RashFortify()
    {
        cost = 10;
        SkillDescription = "High quality, no progress";
        SkillImage = "Rash Fortify";
        Name = "Rash Fortify";
        IsPassive = false;
    }

    public override void CraftingFunction(CraftingManager inManager, SkillModifier mod)
    {
        if (inManager.Stamina >= CalculateCost(mod))
        {
            float qualityadd = 4 * CurrentLevel;
            float progadd = 0f;

            qualityadd += mod.QualityFlat;
            qualityadd *= mod.QualityMult;

            mod.ApplySpecialModifiers(ref qualityadd, ref progadd);

            inManager.ModifyQuality(qualityadd);
            inManager.ModifyStamina(-CalculateCost(mod));
        }
    }
}
