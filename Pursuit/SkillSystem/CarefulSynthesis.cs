using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarefulSynthesis : Skill
{
    public CarefulSynthesis()
    {
        cost = 10;
        SkillDescription = "High progress, low quality";
        SkillImage = "Fortified Synthesized";
        Name = "Careful Synthesis";
        IsPassive = false;
    }


    public override void CraftingFunction(CraftingManager inManager, SkillModifier mod)
    {
        if (inManager.Stamina >= CalculateCost(mod))
        {
            float progressadd = 2 * CurrentLevel;
            float qadd = 0f;

            progressadd += mod.ProgressFlat;
            progressadd *= mod.ProgressMult;

            mod.ApplySpecialModifiers(ref qadd, ref progressadd);

            inManager.ModifyProgress(progressadd);
            inManager.ModifyStamina(-CalculateCost(mod));
        }
    }
}
