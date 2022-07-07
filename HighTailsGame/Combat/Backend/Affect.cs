    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affect  {
    public enum SubscriptionType { Action, State};
    public enum AffectType { Other, Buff, Lock, Invuln, Stun};
    public bool TurnStartSubscriber;
    public AffectType ThisAffectType;
    public SubscriptionType MyType;
    public Actor MyActor;

    public string AffectName = "";

    public int Duration;
    public bool IsTimedAffect = false;

    public virtual void Init()
    {
        if(MyType == SubscriptionType.Action)
        {
            Turnstile.OnActAccept += this.ActEventFired;
        }
        else
        {
            Turnstile.OnStateCheck += this.StateEventFired;
        }
        if (TurnStartSubscriber)
        {
            Turnstile.OnTurnStart += this.TurnStartFired;
        }
    }

    public virtual void TurnStartFired()
    {
        Decrement();
    }

    public virtual void Decrement()
    {

        if (IsTimedAffect)
        {
            if (MyActor.TurnManager.CurrentTurn.MyOwner == MyActor)
            {
                Duration--;
                if (Duration <= 0)
                {
                    MyActor.CurrentAffects.Remove(this);
                    Unsubscribe();
                }
            }
        }
    }

    public virtual void ActEventFired(Act check)
    {

    }

    public virtual void StateEventFired()
    {

    }

    public virtual void Unsubscribe()
    {
        if (MyType == SubscriptionType.Action)
        {
            Turnstile.OnActAccept -= this.ActEventFired;
        }
        else
        {
            Turnstile.OnStateCheck -= this.StateEventFired;
        }

        if (TurnStartSubscriber)
        {
            Turnstile.OnTurnStart -= this.TurnStartFired;
        }
    }



}

public class Invulnerability : Affect
{
    public Invulnerability(Actor myactor, int dur = 999, string name = "Invulnerability")
    {
        MyType = SubscriptionType.State;
        ThisAffectType = AffectType.Invuln;
        TurnStartSubscriber = true;
        MyActor = myactor;
        Duration = dur;
        IsTimedAffect = true;
        AffectName = name;
    }
}

public class HalfDamageBuff : Affect
{
    public HalfDamageBuff(Actor target, int dur = 999, string name = "Theory of Halves")
    {
        MyType = SubscriptionType.Action;
        ThisAffectType = AffectType.Buff;
        TurnStartSubscriber = true;
        MyActor = target;
        Duration = dur;
        IsTimedAffect = true;
        AffectName = name;
        Init();
    }

    public override void ActEventFired(Act check)
    {
        base.ActEventFired(check);
        if (check.MyModule != null)
        {
            if (check.Effects.Exists(x => x.MyType == Effect.EffectType.Damage) == true)
            {
                var lizt = check.Effects.FindAll(x => x.MyType == Effect.EffectType.Damage);
                if(lizt.Exists(x=>x.Targets.Contains(MyActor)))
                {
                    var leezt = check.Effects.FindAll(x => x.MyType == Effect.EffectType.Damage && x.Targets.Contains(MyActor));
                    foreach (Effect eff in leezt)
                    {
                        eff.Efficacy = eff.Efficacy / 2;
                    }
                }
            }
        }
    }

}

public class LeechBuff : Affect
{
    public LeechBuff(Actor myactor, int dur = 999, string name = "Emotional Vampire")
    {
        MyType = SubscriptionType.Action;
        ThisAffectType = AffectType.Buff;
        TurnStartSubscriber = true;
        MyActor = myactor;
        Duration = dur;
        IsTimedAffect = true;
        AffectName = name;
        Init();
    }

    public override void ActEventFired(Act check)
    {
        base.ActEventFired(check);
        if (check.MyModule != null)
        {
            if (check.MyModule.MyActor == MyActor && check.Effects.Exists(x => x.MyType == Effect.EffectType.Damage) == true)
            {
                var lizt = check.Effects.FindAll(x => x.MyType == Effect.EffectType.Damage);
                foreach (Effect eff in lizt)
                {

                    Actor trg = MyActor;

                    var list = new List<Effect>();
                    var dude = new List<Actor>();
                    dude.Add(trg);
                    list.Add(new HealDamage(dude, eff.Efficacy / 2));
                    var ak = new Act()
                    {
                        MyModule = this.MyActor.TargModule,
                        MyType = Act.ActType.Action,
                        Effects = list,
                        Targets = dude
                    };
                    list[0].ReceiveAct(ak);
                    MyActor.TurnManager.ReceiveAct(ak);
                }
            }
        }
    }
}

public class DoubleDamageBuff : Affect
{
    int Triggers;

    public DoubleDamageBuff(Actor myacter, int triggers = 1, string name = "Encouraging Words")
    {
        Triggers = triggers;
        MyType = SubscriptionType.Action;
        ThisAffectType = AffectType.Buff;
        MyActor = myacter;
        AffectName = name;
        Init();
    }

    public override void ActEventFired(Act check)
    {
        base.ActEventFired(check);
        if (check.MyModule != null)
        {
            if (check.MyModule.MyActor == MyActor && check.Effects.Exists(x => x.MyType == Effect.EffectType.Damage) == true)
            {
                var lizt = check.Effects.FindAll(x => x.MyType == Effect.EffectType.Damage);
                foreach (Effect eff in lizt)
                {
                    if(Triggers <= 0)
                    {
                        MyActor.CurrentAffects.Remove(this);
                        Unsubscribe();
                        return;
                    }
                    else
                    {
                        Triggers--;
                        eff.Efficacy = eff.Efficacy * 2;
                    }
                }
            }
        }
    }

}

public class Stun : Affect
{
    public Stun(Actor myacter, int dur = 999, string name = "Stun")
    {
        MyType = SubscriptionType.State;
        ThisAffectType = AffectType.Stun;
        MyActor = myacter;
        Duration = dur;
        TurnStartSubscriber = true;
        IsTimedAffect = true;
        AffectName = name;
        Init();
    }
}

public class Sleep : Affect
{
    public Sleep(Actor myacter, int dur = 999, string name = "Sleep")
    {
        MyType = SubscriptionType.Action;
        ThisAffectType = AffectType.Stun;
        MyActor = myacter;
        Duration = dur;
        TurnStartSubscriber = true;
        IsTimedAffect = true;
        AffectName = name;
        Init();
    }

    public override void ActEventFired(Act check)
    {
        base.ActEventFired(check);
        if (check.MyModule != null)
        {
            if (check.Effects.Exists(x => x.MyType == Effect.EffectType.Damage) == true)
            {
                var lizt = check.Effects.FindAll(x => x.MyType == Effect.EffectType.Damage);
                if (lizt.Exists(x => x.Targets.Contains(MyActor)))
                {
                    Unsubscribe();
                    MyActor.CurrentAffects.Remove(this);
                }
            }
        }
    }
}

public class ActionLock : Affect
{
    public ActionLock(Actor myactor)
    {
        MyType = SubscriptionType.State;
        ThisAffectType = AffectType.Lock;
        MyActor = myactor;
    }
}



public class CheckForDeath : Affect
{
    public int triggered;
    public CheckForDeath(Actor myactor)
    {
        MyActor = myactor;
        MyType = SubscriptionType.State;
        Init();
        triggered = 0;
    }

    public override void StateEventFired()
    {
        if (triggered == 0)
        {
            if (MyActor.MyProfile.GetResource(ActorResource.ResourceType.Health) <= 0)
            {
                triggered = 1;
                var list = new List<Effect>();
                var targ = new List<Actor>();
                targ.Add(MyActor);
                list.Add(new SpawnDeathBubble(MyActor));
                Locomotion lc = (Locomotion)MyActor.MyModules.Find(x => x is Locomotion);
                list.Add(new DeathWalk(targ,lc.DeathWalkToPoint(MyActor.TurnManager.ReturnRandomFleePoint())));
                list.Add(new CallDeath(MyActor));
                var ak = new Act
                {
                    MyModule = MyActor.MyModules.Find(x => x is BasicModule),
                    Targets = targ,
                    Effects = list,
                    MyType = Act.ActType.Action
                };
                list[0].ReceiveAct(ak);
                list[1].ReceiveAct(ak);
                list[2].ReceiveAct(ak);
                MyActor.TurnManager.CleanupQueue.Add(ak);
                MyActor.TurnManager.Morgue.Add(MyActor);
            }
        }
    }
}

public class HealthThresholdTrigger : Affect
{
    /*public TurretBoss.TriggerAct TriggerDel;
    public int Percentage;
    public bool triggered;

    public HealthThresholdTrigger(Actor myactor, int percentage, TurretBoss.TriggerAct trig)
    {
        triggered = false;
        MyActor = myactor;
        Percentage = percentage;
        MyType = SubscriptionType.State;
        TriggerDel = trig;
        Init();
    }

    public override void StateEventFired()
    {
        if (!triggered)
        {
            float current = (float)MyActor.MyProfile.GetResource(ActorResource.ResourceType.Health);
            float factor = (float)Percentage * 0.01f;
            float target = (float)MyActor.MyProfile.GetResourceRef(ActorResource.ResourceType.Health) * factor;
            if (current <= target)
            {
                triggered = true;
                TriggerDel();
            }
        }
    }*/
}

