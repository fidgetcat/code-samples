using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : Module
{
    public Logic(Actor myactor)
    {
        MyActor = myactor;
        MyClass = ModuleClass.Active;
    }

    public Act GetHealAct(Vector2Int target, float factor)
    {
        int final = Mathf.RoundToInt(factor * 40f);

        Actor trg = MyActor.GameManager.Level.ActorReferences[target.x, target.y][0].GetComponent<Actor>();

        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(trg);
        list.Add(new HealDamage(dude, final));
        var ak = new Act()
        {
            MyModule = this,
            MyType = Act.ActType.Action,
            Effects = list,
            Targets = dude
        };
        CriticalCheck(ak, factor);
        list[0].ReceiveAct(ak);
        if (!ak.IsCriticalCutscene)
        {
            list.Insert(0, new SpawnSpeechBubble(MyActor, MyActor.ReturnInsult()));
            list[0].ReceiveAct(ak);
        }
        ak.ActName = "Doctor's Order";
        if (MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "You can keep it up! You’ll get through to them!";
        }
        else
        {
            ak.Catchphrase = "You can keep it up! You’ll get through to them!";
        }
        return ak;
    }

    public Act GetDamageBuffAct(Actor target, float factor)
    {
        Actor trg = target;
        target.CurrentAffects.Add(new DoubleDamageBuff(trg,Mathf.RoundToInt(1*factor)));
        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(trg);
        list.Add(new PlayBuffEffect(trg));
        var ak = new Act()
        {
            MyModule = this,
            MyType = Act.ActType.Action,
            Effects = list,
            Targets = dude
        };
        CriticalCheck(ak, factor);
        list[0].ReceiveAct(ak);
        if (!ak.IsCriticalCutscene)
        {
            list.Insert(0, new SpawnSpeechBubble(MyActor, MyActor.ReturnInsult()));
            list[0].ReceiveAct(ak);
        }
        ak.ActName = "Encouraging Words";
        if (MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "Anything you say will scar them. You have my permission to do just that.";
        }
        else
        {
            ak.Catchphrase = "I think this idea makes the best counterpoint for that statement!";
        }

        if (ak.IsCriticalCutscene)
        {
            List<Actor> dood = new List<Actor>();
            dood.Add(target);
            list.Add(new HealDamage(dood, 30));
            list.Find(x => x is HealDamage).ReceiveAct(ak);
        }

        return ak;
    }

    public Act GetSleeperAct(Actor target, float factor)
    {
        Actor trg = target;
        target.CurrentAffects.Add(new Sleep(target, Mathf.RoundToInt(2 * factor) + 1));
        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(trg);
        list.Add(new PlayDebuffEffect(trg));
        var ak = new Act()
        {
            MyModule = this,
            MyType = Act.ActType.Action,
            Effects = list,
            Targets = dude
        };
        CriticalCheck(ak, factor);
        list[0].ReceiveAct(ak);
        if (!ak.IsCriticalCutscene)
        {
            list.Insert(0, new SpawnSpeechBubble(MyActor, MyActor.ReturnInsult()));
            list[0].ReceiveAct(ak);
        }
        ak.ActName = "Boring Explanation";
        if (MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "Sit down, fool. I’ll enlighten you on the finer points of how to properly serve a human being.";
        }
        else
        {
            ak.Catchphrase = "How about we sit down and just have a good, long talk about how we can help each other?";
        }
        return ak;
    }

    public Act GetHalfDamageAct(Actor target, float factor)
    {
        Actor trg = target;
        int facc = 0;
        if (factor >= 1.9f)
            facc = 1;
        target.CurrentAffects.Add(new HalfDamageBuff(target,Mathf.RoundToInt(2*factor)+1 - facc));
        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(trg);
        list.Add(new PlayBuffEffect(trg));
        var ak = new Act()
        {
            MyModule = this,
            MyType = Act.ActType.Action,
            Effects = list,
            Targets = dude
        };
        CriticalCheck(ak, factor);
        list[0].ReceiveAct(ak);
        if (!ak.IsCriticalCutscene)
        {
            list.Insert(0, new SpawnSpeechBubble(MyActor, MyActor.ReturnInsult()));
            list[0].ReceiveAct(ak);
        }
        ak.ActName = "Theory of Halves";
        if (MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "Look at how soft and furry they are. Their arguments are just as harmless and just as useless as they are.";
        }
        else
        {
            ak.Catchphrase = "Just remember that they’re going to be wrong in the end.";
        }
        return ak;
    }
}
