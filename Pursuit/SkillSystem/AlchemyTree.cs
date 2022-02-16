using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AlchemyTree : SkillTree
{
    public AlchemyTree()
    {
        Treefab = "AlchemySkills";
        Name = "Alchemy";
        SkillDictionary.Add("Fortified Synthesis", new FortifiedSynthesis());
        SkillDictionary.Add("Rash Fortify", new RashFortify());
        SkillDictionary.Add("Careful Synthesis", new CarefulSynthesis());
        SkillDictionary.Add("Focused Mind", new FocusedMind());
        SkillDictionary.Add("Fortified", new Fortified());
        SkillDictionary.Add("Swift Touch", new SwiftTouch());
        SkillDictionary.Add("Synthesized", new Synthesized());
        SkillDictionary["Fortified Synthesis"].LevelUp();
    }
}
