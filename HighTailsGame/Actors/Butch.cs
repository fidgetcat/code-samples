using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butch : PlayerActor
{


    public Locomotion WalkModule;
    private BasicModule BasicModule;
    private BasicAttacking AttackModule;


    public override void Initialize(int input, TacticsManager GM, Turnstile TM)
    {
        base.Initialize(input, GM, TM);
        UI = FindObjectOfType<TacticUI>();
        TurnManager.PlayerActors.Add(this);
        EnableTutorial = true;
    }

    protected override void BuildProfile()
    {
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Speed,
            Growth = 0,
            Value = 0,
            BaseValue = 10
        });
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Attack,
            Growth = 0,
            Value = 0,
            BaseValue = 20
        });
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Range,
            Growth = 0,
            Value = 0,
            BaseValue = 2
        });
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Movement,
            Growth = 0,
            Value = 0,
            BaseValue = 6
        });
        MyProfile.MyResources.Add(new ActorResource
        {
            Type = ActorResource.ResourceType.Health,
            Growth = 0,
            Value = 0,
            BaseValue = 90
        });
    }

    protected override void InitModules()
    {
        base.InitModules();
        var loc = new Locomotion(this);
        WalkModule = loc;
        var bas = new BasicModule(this);
        var atk = new BasicAttacking(this);

        AttackModule = atk;
        BasicModule = bas;
        MyModules.Add(atk);
        MyModules.Add(loc);
        MyModules.Add(bas);

        var com = (Comedy)MyModules.Find(x => x is Comedy);
        var agr = (Aggression)MyModules.Find(x => x is Aggression);
        var log = (Logic)MyModules.Find(x => x is Logic);

        //Skills.Add(new Skill("Doctor's Order", this, log.GetHealAct, 3, true, false, 0));
        // Skills.Add(new Skill("Theory of Halves", this, log.GetHalfDamageAct, 4, true, false, 4));
        //  Skills.Add(new Skill("Encouraging Words", this, log.GetDamageBuffAct, 4, true, false, 4));
        // Skills.Add(new Skill("Boring Explanation", this, log.GetSleeperAct, 3, false, true, 4));
        Skills.Add(new Skill("Emotional Vampire", this, agr.GetVampireAct, 0, true, false, 3));
        Skills.Add(new Skill("Rage", this, agr.GetMovebuff, 0, true, false, 4));
        Skills.Add(new Skill("Insult to Injury", this, agr.GetStunHit, 4, false, true, 5));

        Skills[0].Description = "For a few turns, heals user for half of the damage they deal. Hitting the red area increases effect duration.";
        Skills[1].Description = "Greatly increases the user's movement points for one turn. Hitting the red area will grant even more movement points.";
        Skills[2].Description = "Stuns the target, damaging them at the same time. Hitting the red area increases stun duration.";
    }

    public override void GetWalkAction(Vector2Int targ)
    {
        if (CurrentTurn != null)
        {
            CurrentTurn.SubmitAction(WalkModule.CreateWalkAct(targ));
        }
        else
        {
        }
    }

    public override void GetEndTurnAction()
    {
        if (CurrentTurn != null)
        {
            CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
        }
    }

    public override bool GetAttackAction(Vector2Int targ)
    {
        var list = GameManager.Level.ActorReferences[targ.x, targ.y];
        List<Actor> aah = new List<Actor>();
        foreach (GameObject thing in list)
        {
            aah.Add(thing.GetComponent<Actor>());
        }
        var ting = aah.Find(x => x.ThisType == UnitType.AI);
        if (ting != null)
        {
            if (!ting.CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Invuln))
            {
                CurrentTurn.SubmitAction(AttackModule.CreateAttackAct(ting));
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }



    protected override void InitStats()
    {
        base.InitStats();
    }

    public override void NotifyTurnBegin()
    {
        base.NotifyTurnBegin();

    }

    public override void NotifyTurnEnd()
    {
        base.NotifyTurnEnd();
        UI.PlayerMenu.SetActive(false);
        UI.EndTurnCleanup();
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    public override void NotifyActReady()
    {
        //reactivate UI
        if (CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Stun))
        {
            return;
        }
        else
        {
            TutorialFunc();
            UI.PlayerMenu.SetActive(true);
        }
    }

}
