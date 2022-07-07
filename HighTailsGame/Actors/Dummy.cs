using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Actor
{
    public BasicModule BasicModule;

    public override void Initialize(int input, TacticsManager GM, Turnstile TM)
    {
        base.Initialize(input, GM, TM);
    }

    public override void NotifyActReady()
    {
        base.NotifyActReady();
        CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
    }

    public override void NotifyTurnBegin()
    {
        base.NotifyTurnBegin();
        NotifyActReady();
    }

    public override void NotifyTurnEnd()
    {
        base.NotifyTurnEnd();
    }

    protected override void BuildProfile()
    {
        base.BuildProfile();
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Speed,
            Growth = 0,
            Value = 0,
            BaseValue = 1
        });

        MyProfile.MyResources.Add(new ActorResource
        {
            Type = ActorResource.ResourceType.Health,
            Growth = 0,
            Value = 0,
            BaseValue = 9999
        });
    }

    protected override void InitModules()
    {
        base.InitModules();
        var bas = new BasicModule(this);
        BasicModule = bas;
        MyModules.Add(bas);
    }

    protected override void InitStats()
    {
        base.InitStats();
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();
    }
}
