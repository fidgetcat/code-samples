using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn  {
    public Actor MyOwner;
    public Turnstile MyManager;
    public List<Act> ActHistory;

    public int Movecount;
    public int Actcount;

    public void SubmitAction(Act input)
    {
        if(input.MyType == Act.ActType.Movement)
        {
            var eff = input.Effects.Find(x => x.MyType == Effect.EffectType.Movement);
            if (eff != null)
            {
                Movecount -= eff.Efficacy;
                MyOwner.TurnMoveCount = Movecount;
            }
            else
            {
                Movecount = 0;
            }
        }
        else
        {
            Actcount--;
        }
        ActHistory.Add(input);
        MyManager.ReceiveTurnAct(input);
    }

    public Turn(Actor owner, Turnstile manager)
    {
        ActHistory = new List<Act>();
        MyManager = manager;
        MyOwner = owner;
        if (MyOwner.MyProfile.HasStat(ActorStat.StatType.Movement))
        {
            Movecount = MyOwner.MyProfile.GetStat(ActorStat.StatType.Movement);
        }
        else
        {
            Movecount = 0;
        }
        Actcount = 1;
    }

}
