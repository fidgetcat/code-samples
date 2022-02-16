using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill{
    public string Name { get; set; }
    public string SkillDescription { get; set; }
    public string SkillImage { get; set; }
    public bool IsPassive, IsInstant;

    public int CooldownCounter { get; set; }
    public int cost;
    protected int CurrentLevel, RequiredLevel, MaxLevel, CooldownTime;
    protected Dictionary<string, int> Prerequisites;

    public Skill()
    {
        Debug.Log("Skillcreated");
        SkillDescription = "wurf";
        IsPassive = false;
        IsInstant = false;
        SkillImage = "heman";
        Prerequisites = new Dictionary<string, int>();
        cost = 1;
        RequiredLevel = 1;
        CurrentLevel = 0;
        MaxLevel = 10;
        Name = "Default";
        CooldownTime = 1;
        ResetCooldown();
    }

   public string GetSkillInfo()
    {
        string derp = "";
        derp += Name;
        derp += ": ";
        derp += CurrentLevel;
        derp += "/";
        derp += MaxLevel;
        derp += "\nStamina Cost: ";
        derp += cost;
        derp += "\n";
        derp += SkillDescription;
        derp += "\nPrerequisites:\n";
        derp += GetPrerequisitesToString();
        return derp;
    }

    public string GetCurrentLevel()
    {
        string derp = "";
        derp += CurrentLevel;
        derp += "/";
        derp += MaxLevel;
        return derp;
    }

    public string GetMaxLevel()
    {
        return MaxLevel.ToString();
    }

    public int GetLevelInt()
    {
        return CurrentLevel;
    }

    public bool CheckMax()
    {
        if (CurrentLevel < MaxLevel)
        {
            return false;
        }
        else
            return true;
    }

    public bool CheckMax(int mod)
    {
        if (CurrentLevel+mod < MaxLevel)
        {
            return false;
        }
        else
            return true;
    }

    public string GetSkillLevelString()
    {
        return CurrentLevel.ToString();
    }

    public string GetPrerequisitesToString()
    {
        string derpy = "";
        foreach(KeyValuePair<string,int> entry in Prerequisites)
        {
            
            derpy += entry.Key;
            derpy += ": ";
            derpy += entry.Value.ToString();
            derpy += "\n";
        }
        return derpy;
    }

    public bool CheckRequirements(Dictionary<string, Skill> inSkills, int inLevel)
    {
        bool pass = true;
        int reqs = Prerequisites.Count;
        if (reqs > 0)
        {
            foreach (KeyValuePair<string, int> entry in Prerequisites)
            {
                if (inSkills.ContainsKey(entry.Key) && inSkills[entry.Key].CurrentLevel >= entry.Value)
                {
                    if (GameControl.brain.Player.Level < RequiredLevel)
                        pass = false;
                }
                else if (!inSkills.ContainsKey(entry.Key))
                {

                }
                else
                {
                    pass = false;
                }
            }
        }
        else
        {
            if (GameControl.brain.Player.Level < RequiredLevel)
                pass = false;
        }
        return pass;
    }

    public bool CheckCooldown()
    {
        if (CooldownCounter <= 0)
            return true;
        else
            return false;
    }

    public void PutOnCooldown()
    {
        CooldownCounter = CooldownTime;
    }

    public void TickCooldown()
    {
        CooldownCounter--;
        if (CooldownCounter < 0)
            CooldownCounter = 0;
    }

    public void ResetCooldown()
    {
        CooldownCounter = 0;
    }

    public bool IsZero()
    {
        if (CurrentLevel > 0)
        {
            return false;
        }
        else
            return true;
    }

    public virtual void CraftingFunction(CraftingManager inManager, SkillModifier mod)
    {

    }

    public virtual void AffectFunction(CraftingManager inManager, SkillModifier mod)
    {

    }

    public virtual void PassiveFunction(CraftingManager inManager)
    {
        inManager.CurrentAffects.Add(new SkillAffect(this.Name, -1, SkillDescription, SkillImage));
    }


    public void LevelUp()
    {
        if (CurrentLevel < MaxLevel)
        {
            CurrentLevel++;
        }
    }

    public float CalculateCost(SkillModifier mod)
    {
        return cost * mod.StamMod;
    }
}

public class SkillAffect{
    public string AffectName, Description, Image;
    public int Count;

    public SkillAffect(string name, int cnt)
    {
        AffectName = name;
        Count = cnt;
    }

    public SkillAffect ReturnCopy()
    {
        return new SkillAffect(this.AffectName, this.Count, this.Description, this.Image);
    }

    public SkillAffect(string name, int cnt, string desc, string img)
    {
        Description = desc;
        Image = img;
        AffectName = name;
        Count = cnt;
    }

    public bool UseAffect(CraftingManager inman)
    {
        Count--;
        if (Count <= 0)
        {
            Count = 0;
            return true;
        }
        else
            return false;
    }


}

public class SkillModifier
{
    public float ProgressFlat, ProgressMult, QualityFlat, QualityMult, StamMod;
    public bool SetProgZero, SetQualZero, SetQualSame, SetProgSame;

    public SkillModifier()
    {
        ProgressFlat = 0f;
        ProgressMult = 1f;
        QualityFlat = 0f;
        QualityMult = 1f;
        StamMod = 1f;

        SetProgSame = false;
        SetProgZero = false;
        SetQualSame = false;
        SetQualZero = false;
    }



    public void ApplySpecialModifiers(ref float qual, ref float prog)
    {
        if (SetQualZero)
        {
            qual = 0f;
        }
        if (SetProgZero)
        {
            prog = 0f;
        }
        if (SetProgSame)
        {
            prog = qual;
        }
        if(SetQualSame)
        {
            qual = prog;
        }
    }

    public void ApplyThisTo(SkillModifier input)
    {
        input.ProgressFlat += this.ProgressFlat;
        input.ProgressMult += this.ProgressMult;
        input.QualityFlat += this.ProgressMult;
        input.QualityMult += this.QualityMult;
    }
}

