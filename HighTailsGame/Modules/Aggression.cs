using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggression : Module
{
    public Aggression(Actor actor)
    {
        MyActor = actor;
        MyClass = ModuleClass.Active;
    }

    public Act GetVampireAct(Actor emptyparam, float factor)
    {

        Actor trg = MyActor;

        MyActor.CurrentAffects.Add(new LeechBuff(MyActor,Mathf.RoundToInt((2 * factor)+1)));
        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(trg);
        list.Add(new PlayBuffEffect(MyActor));
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
        ak.ActName = "Emotional Vampire";
        if (MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "Your rage only makes my conviction stronger!";
        }
        else
        {
            ak.Catchphrase = "I’m sorry to have to do this to you, but you have to learn some of these lessons yourself.";
        }
        return ak;
    }

    public Act GetMovebuff(Actor emptyparam, float factor)
    {
        Actor trg = MyActor;
        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(trg);
        list.Add(new PlayBuffEffect(MyActor));
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
        if (factor > 1.1f)
        {
            factor *= 0.75f;
        }
        MyActor.CurrentTurn.Movecount += Mathf.RoundToInt(MyActor.MyProfile.GetStatRef(ActorStat.StatType.Movement) * factor);
        MyActor.TurnMoveCount = MyActor.CurrentTurn.Movecount;
        ak.ActName = "Rage";
        if (MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "You’re the deer. I’m the headlights AND the car!";
        }
        else
        {
            ak.Catchphrase = "Stop! You’re making a mistake and I’m not going to let you!";
        }
        return ak;
    }

    public Act GetStunHit(Actor targ, float factor)
    {
        Actor trg = targ;
        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(trg);
        int dur = 1;
        if (factor >= 1.9f)
        {
            dur = 2;
        }
        trg.CurrentAffects.Add(new Stun(trg, dur + 1));
        list.Add(new DealDamage(dude, MyActor.MyProfile.GetStat(ActorStat.StatType.Attack)));
        list.Add(new PlayDebuffEffect(targ));
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
        ak.ActName = "Insult to Injury";
        if (MyActor.ThisType == Actor.UnitType.AI)
        {
            ak.Catchphrase = "You’d make a nice rug but I still wouldn’t buy you!";
        }
        else
        {
            ak.Catchphrase = "Swimming in blind anger is debilitating isn’t it?";
        }
        return ak;
    }
}
