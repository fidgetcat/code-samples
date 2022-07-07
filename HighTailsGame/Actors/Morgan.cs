using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morgan : PlayerActor, ISummoner
{

    public Locomotion WalkModule;
    private BasicModule BasicModule;
    private BasicAttacking AttackModule;
    private Summoning SummoningModule;

    public GameObject ResistorPrefab;

    public List<Actor> MyMinions;

    public override void Initialize(int input, TacticsManager GM, Turnstile TM)
    {
        base.Initialize(input, GM, TM);
        MyMinions = new List<Actor>();
        UI = FindObjectOfType<TacticUI>();
        TurnManager.PlayerActors.Add(this);
    }

    public void ReceiveMinion(Actor actor)
    {
        MyMinions.Add(actor);
    }

    public void RemoveMinion(Actor actor)
    {
        if (MyMinions.Contains(actor))
        {
            MyMinions.Remove(actor);
        }
    }
    protected override void BuildProfile()
    {
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Speed,
            Growth = 1,
            Value = 1,
            BaseValue = 3
        });
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Attack,
            Growth = 0,
            Value = 0,
            BaseValue = 15
        });
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Range,
            Growth = 0,
            Value = 0,
            BaseValue = 4
        });
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Movement,
            Growth = 0,
            Value = 0,
            BaseValue = 4
        });
        MyProfile.MyResources.Add(new ActorResource
        {
            Type = ActorResource.ResourceType.Health,
            Growth = 0,
            Value = 0,
            BaseValue = 85
        });
    }

    protected override void InitModules()
    {
        base.InitModules(); 
        var loc = new Locomotion(this);
        WalkModule = loc;
        var bas = new BasicModule(this);
        var atk = new BasicAttacking(this);
        var sum = new Summoning(this);
        SummoningModule = sum;
        AttackModule = atk;
        BasicModule = bas;
        MyModules.Add(sum);
        MyModules.Add(atk);
        MyModules.Add(loc);
        MyModules.Add(bas);
        var com = (Comedy)MyModules.Find(x => x is Comedy);
        var agr = (Aggression)MyModules.Find(x => x is Aggression);
        var log = (Logic)MyModules.Find(x => x is Logic);


        Skills.Add(new Skill("Self Deprecation", this, com.GetSacrificeAct, 5, false, true, 3));
        Skills.Add(new Skill("Turn Around", this, com.GetTeleSwapAct, 4, false, true, 5));
         Skills.Add(new LaughingGas(this, com.GetLaughGasAct));

        Skills[0].Description = "Damages self and target enemy. Will not deal lethal damage to the user. Hitting the red area increases outgoing damage and decreases incoming damage.";
        Skills[1].Description = "Switches places with target enemy. Hitting the red area halves the skill's cooldown.";
        Skills[2].Description = "Stuns all enemies within a 2-tile vicinity of the user. Will not fire if there are no valid targets nearby. Hitting the red area increases effect duration.";
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
        foreach(GameObject thing in list)
        {
            aah.Add(thing.GetComponent<Actor>());
        }   
        var ting = aah.Find(x => x.ThisType == UnitType.AI);
        if(ting != null)
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

    public override void TutorialFunc()
    {
        if (EnableTutorial)
        {
            if (TurnManager.ActorQueue.Exists(x => x.Name == "Cortni"))
            {
                var tg = TurnManager.ActorQueue.Find(x => x.Name == "Cortni");
                if (AttackModule.CheckWithinRange(tg))
                {
                    TurnManager.Director.ReceiveTutorialText('b');
                }
                else
                {
                    TurnManager.Director.ReceiveTutorialText('a');
                }
            }
            else
            {
                TurnManager.Director.ReceiveTutorialText('d');
            }
        }
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




