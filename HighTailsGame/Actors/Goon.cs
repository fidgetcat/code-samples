using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Goon : Actor
{
    public Actor MyMaster;
    public Locomotion WalkModule;
    public BasicAttacking AttackModule;
    public BasicModule BasicModule;
    public AudioClip Suicide;
    public bool stifle = false;

    public int atk = 20;
    public int HP = 75;
    public int mov = 4;
    public int spd = 2;
    public int range = 4;

    public int atkcount;

    public override void Initialize(int input, TacticsManager GM, Turnstile TM)
    {
        base.Initialize(input, GM, TM);
        atkcount = 0;
    }

    public override void PlaySuicideSound()
    {
        MyAudio.PlayOneShot(Suicide);
    }

    public override void NotifyActReady()
    {
        Actor targg = null;

        if (TurnManager.ActorQueue.Exists(x => x.Name == "Doc") && TurnManager.ActorQueue.Find(x => x.Name == "Doc").CheckAdjacentPathing() == true)
        {
            targg = TurnManager.ActorQueue.Find(x => x.Name == "Doc");
        }
        else if (TurnManager.ActorQueue.Exists(x => x.Name == "Butch") && TurnManager.ActorQueue.Find(x => x.Name == "Butch").CheckAdjacentPathing() == true)
        {
            targg = TurnManager.ActorQueue.Find(x => x.Name == "Butch");
        }
        else if (TurnManager.ActorQueue.Exists(x => x.Name == "Morgan") && TurnManager.ActorQueue.Find(x => x.Name == "Morgan").CheckAdjacentPathing() == true)
        {
            targg = TurnManager.ActorQueue.Find(x => x.Name == "Morgan");
        }

        if (targg == null)
        {
            CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
            return;
        }

        if (!CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Lock))
        {
            if (Skills.Exists(x => x.CurrentCooldown <= 0) && CurrentTurn.Actcount > 0)
            {
                float factor = 1f;
                if (Random.Range(0f, 100f) >= 65f)
                {
                    factor = 2f;
                }
                Skill tskill = Skills.Find(x => x.CurrentCooldown <= 0);
                if (tskill.MyTargetType == Skill.TargetType.Area)
                {
                    var tilez = tskill.GetTargetingData(CurrentPosition).Tiles;
                    if (tilez.Exists(x => x == targg.CurrentPosition))
                    {
                        tskill.UseSkill(tskill.GetTargetingData(CurrentPosition).Tiles, factor);
                    }
                    else if (CurrentTurn.Movecount > 0)
                    {
                        if (CheckAdjacentPathing())
                        {
                            CurrentTurn.SubmitAction(WalkModule.CreateWalkAct((targg)));
                        }
                        else
                        {
                            CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
                        }
                    }
                    else
                    {
                        CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
                    }
                }
                else if (tskill.AllyOnly)
                {
                    if (tskill.ActorDel != null)
                    {
                        tskill.UseSkill(this, factor);
                    }
                    else
                    {
                        tskill.UseSkill(CurrentPosition, factor);
                    }
                }
                else if (tskill.EnemyOnly)
                {
                    var tilez = GameManager.GetValidTiles(CurrentPosition, tskill.Range, false, true);
                    if (tilez.Exists(x => x == targg.CurrentPosition))
                    {
                        if (tskill.ActorDel != null)
                        {
                            tskill.UseSkill(targg, factor);
                        }
                        else
                        {
                            tskill.UseSkill(targg.CurrentPosition, factor);
                        }
                    }
                    else if (CurrentTurn.Movecount > 0)
                    {
                        if (CheckAdjacentPathing())
                        {
                            CurrentTurn.SubmitAction(WalkModule.CreateWalkAct((targg)));
                        }
                        else
                        {
                            CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
                        }
                    }
                    else
                    {
                        CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
                    }
                }
            }
            else if (AttackModule.CheckWithinRange(targg) && CurrentTurn.Actcount > 0)
            {
                CurrentTurn.SubmitAction(AttackModule.CreateAttackAct(targg));
            }
            else if (CurrentTurn.Movecount > 0)
            {
                if (CheckAdjacentPathing())
                {
                    if (!AttackModule.CheckWithinRange(targg))
                        CurrentTurn.SubmitAction(WalkModule.CreateWalkAct((targg)));
                    else
                        CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
                }
                else
                {
                    CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
                }
            }
            else if (CurrentTurn.Actcount <= 0)
            {
                CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
            }
            else
            {
                CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
            }
        }
        else
        {
            CurrentTurn.SubmitAction(BasicModule.CreateEndTurnAct());
        }
    }

    public override void NotifyTurnBegin()
    {
        base.NotifyTurnBegin();
        if (!CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Stun))
        {
            NotifyActReady();
        }
    }

    public override void NotifyTurnEnd()
    {
        base.NotifyTurnEnd();
    }

    /*  public void OnDeath()
      {
          ISummoner sum = (ISummoner)MyMaster;
          sum.RemoveMinion(this);
      } */

    public override void Death()
    {
        //OnDeath();
        foreach (Affect aff in CurrentAffects)
        {
            aff.Unsubscribe();
        }
        base.Death();
    }

    /*    public void ReceiveMaster(Actor actor)
        {
            MyMaster = actor;
            ISummoner sum = (ISummoner)MyMaster;
            sum.ReceiveMinion(this);
        } */

    protected override void BuildProfile()
    {
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Speed,
            Growth = 0,
            Value = 0,
            BaseValue = spd
        });
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Attack,
            Growth = 0,
            Value = 0,
            BaseValue = atk
        });
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Range,
            Growth = 0,
            Value = 0,
            BaseValue = range
        });
        MyProfile.MyStats.Add(new ActorStat
        {
            Type = ActorStat.StatType.Movement,
            Growth = 0,
            Value = 0,
            BaseValue = mov
        });
        MyProfile.MyResources.Add(new ActorResource
        {
            Type = ActorResource.ResourceType.Health,
            Growth = 0,
            Value = 0,
            BaseValue = HP
        });
    }

    protected override void InitModules()
    {
        base.InitModules();
        var bas = new BasicModule(this);
        var atk = new BasicAttacking(this);
        var loc = new Locomotion(this);
        MyModules.Add(bas);
        MyModules.Add(atk);
        MyModules.Add(loc);
        BasicModule = bas;
        AttackModule = atk;
        WalkModule = loc;
        var com = (Comedy)MyModules.Find(x => x is Comedy);
        var agr = (Aggression)MyModules.Find(x => x is Aggression);
        var log = (Logic)MyModules.Find(x => x is Logic);

        if (!stifle)
        {
            switch (ThisAffinity)
            {
                case Affinity.Aggresion:
                    Skills.Add(new Skill("Emotional Vampire", this, agr.GetVampireAct, 0, true, false, 3));
                    Skills.Add(new Skill("Rage", this, agr.GetMovebuff, 0, true, false, 4));
                    Skills.Add(new Skill("Insult to Injury", this, agr.GetStunHit, 4, false, true, 5));
                    break;
                case Affinity.Comedy:
                    Skills.Add(new Skill("Self Deprecation", this, com.GetSacrificeAct, 5, false, true, 3));
                    Skills.Add(new Skill("Turn Around", this, com.GetTeleSwapAct, 4, false, true, 5));
                    Skills.Add(new LaughingGas(this, com.GetLaughGasAct));
                    break;
                case Affinity.Logic:
                    Skills.Add(new Skill("Theory of Halves", this, log.GetHalfDamageAct, 4, true, false, 4));
                    Skills.Add(new Skill("Encouraging Words", this, log.GetDamageBuffAct, 4, true, false, 4));
                    Skills.Add(new Skill("Boring Explanation", this, log.GetSleeperAct, 3, false, true, 2));
                    break;
                default:
                    break;
            }
        }
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