using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillTree{
    public string Name { get; set; }
    public string Treefab { get; set; }
    public Dictionary<string, Skill> SkillDictionary { get; set; }


    public SkillTree()
    {
        SkillDictionary = new Dictionary<string, Skill>();
    }

    public List<Skill> ReturnActiveSkills()
    {
        List<Skill> derpy = new List<Skill>();
        foreach(KeyValuePair<string,Skill> element in SkillDictionary)
        {
            if (element.Value.IsPassive == false && element.Value.IsZero() == false)
                derpy.Add(element.Value);
        }
        return derpy;
    }

    public List<Skill> ReturnPassiveSkills()
    {
        List<Skill> derpy = new List<Skill>();
        foreach (KeyValuePair<string, Skill> element in SkillDictionary)
        {
            if (element.Value.IsPassive == true && element.Value.IsZero() == false)
                derpy.Add(element.Value);
        }
        return derpy;
    }
}
