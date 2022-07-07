using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comedy : Module
{
    public Comedy(Actor actor)
    {
        MyActor = actor;
        MyClass = ModuleClass.Active;
    }

    public Act GetTeleSwapAct(Actor target, float factor)
    {
        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(target);
        dude.Add(MyActor);
        list.Add(new Teleport(target, MyActor.CurrentPosition));
        list.Add(new Teleport(MyActor, target.CurrentPosition));
        var ak = new Act()
        {
            MyModule = this,
            MyType = Act.ActType.Action,
            Effects = list,
            Targets = dude
        };
        CriticalCheck(ak, factor);
        if (!ak.IsCriticalCutscene)
        {
            list.Insert(0, new SpawnSpeechBubble(MyActor, MyActor.ReturnInsult()));
            list[0].ReceiveAct(ak);
        }
        if (factor >= 1.9f)
        {
            var ting = MyActor.Skills.Find(x => x.Name == "Turn Around");
            ting.CurrentCooldown = Mathf.RoundToInt(ting.CurrentCooldown/2);
        }
        else if (factor >= 1.4f)
        {
            var ting = MyActor.Skills.Find(x => x.Name == "Turn Around");
            ting.CurrentCooldown--;
        }
        ak.ActName = "Turn Around";
        if(MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "Now that we’ve switched places, my friends will just go ahead and make a rug out of you.";
        }
        else
        {
            ak.Catchphrase = "Now that I’ve turned you around. Let’s see if I can’t turn your views around too!";
        }

        return ak;
    }

    public Act GetSacrificeAct(Actor target, float factor)
    {
        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(target);
        var dood = new List<Actor>();
        dood.Add(MyActor);
        list.Add(new DealDamage(dude,Mathf.RoundToInt(MyActor.MyProfile.GetStat(ActorStat.StatType.Attack)*factor)));
        if (Mathf.RoundToInt(10 / factor) >= dood[0].MyProfile.GetResource(ActorResource.ResourceType.Health))
        {
            list.Add(new FakeDamage(dood, Mathf.RoundToInt(10 / factor)));
        }
        else
        {
            list.Add(new DealDamage(dood, Mathf.RoundToInt(10 / factor)));
        }
        var ak = new Act()
        {
            MyModule = this,
            MyType = Act.ActType.Action,
            Effects = list,
            Targets = dude
        };
        list[0].ReceiveAct(ak);
        list[1].ReceiveAct(ak);
        CriticalCheck(ak, factor);
        if (!ak.IsCriticalCutscene)
        {
            list.Insert(0, new SpawnSpeechBubble(MyActor, MyActor.ReturnInsult()));
            list[0].ReceiveAct(ak);
        }
        ak.ActName = "Self Deprecation";
        if (MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "And I thought I was ugly. I would NOT want to be walking around covered in fur.";
        }
        else
        {
            ak.Catchphrase = "If only I was as good as you getting my point across to the other person.";
        }
        return ak;
    }

    public Act GetLaughGasAct(List<Vector2Int> area, float factor)
    {
        var list = new List<Effect>();
        var dude = new List<Actor>();
        var ak = new Act()
        {
            MyModule = this,
            MyType = Act.ActType.Action,
            Effects = list,
            Targets = dude
        };
        int counter = 0;
        CriticalCheck(ak, factor);
        int dur = 1;
        if (factor >= 1.9f)
        {
            dur = 2;
        }
        foreach (Vector2Int vec in area)
        {
            Actor trg = MyActor.GameManager.LevelActors.Find(x=>x.CurrentPosition == vec);
            if (trg != null)
            {
                if (trg.ThisType != MyActor.ThisType)
                {
                    trg.CurrentAffects.Add(new Stun(trg, dur + 1));

                    dude.Add(trg);
                    list.Add(new PlayDebuffEffect(trg));

                    list[counter].ReceiveAct(ak);
                    counter++;
                }
            }
        }
        ak.ActName = "Laughing Gas";
        if (MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "Oh I did hurt a few skunks for this trick. It was satisfying too!";
        }
        else
        {
            ak.Catchphrase = "Stick around for a finale! You’ll see how they contribute to society!";
        }
        if (!ak.IsCriticalCutscene)
        {
            list.Insert(0, new SpawnSpeechBubble(MyActor, MyActor.ReturnInsult()));
            list[0].ReceiveAct(ak);
        }
        return ak;
    }
}
