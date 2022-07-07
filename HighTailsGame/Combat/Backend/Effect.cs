using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
public class Effect
{
    public enum EffectType { Special, Heal, Movement, Circuitry, Damage, EndTurn, Cut }
    public Act MyAct;
    public EffectType MyType;
    public List<Actor> Targets;
    public int Efficacy;

    public void ReceiveAct(Act act)
    {
        MyAct = act;
        SecondaryInit();
    }

    public virtual void SecondaryInit()
    {

    }

    public virtual IEnumerator ExecuteEffect()
    {
        ApplyEffect();
        yield return null;
    }

    public virtual void ApplyEffect()   
    {
        //
    }
}

public class PlayBuffEffect : Effect
{
    public PlayBuffEffect(Actor targ)
    {
        Targets = new List<Actor>();
        Targets.Add(targ);
    }
    public override IEnumerator ExecuteEffect()
    {
        var ting = GameObject.Instantiate(Targets[0].GameManager.Buff, Targets[0].transform.position, Quaternion.identity);
        ting.GetComponent<TimedDestroy>().DestroyMe(1.3f);
        yield return new WaitForSeconds(1.3f);
    }
}

public class PlayDebuffEffect : Effect
{
    public PlayDebuffEffect(Actor targ)
    {
        Targets = new List<Actor>();
        Targets.Add(targ);
    }
    public override IEnumerator ExecuteEffect()
    {
        var ting = GameObject.Instantiate(Targets[0].GameManager.Debuff, Targets[0].transform.position, Quaternion.identity);
        ting.GetComponent<TimedDestroy>().DestroyMe(1.3f);
        yield return new WaitForSeconds(1.3f);
    }
}

public class EndTurn : Effect
{
    public EndTurn(List<Actor> targets)
    {
        MyType = EffectType.EndTurn;
        Targets = targets;
        Efficacy = 1;
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        foreach (Actor target in Targets)
        {
            if (target.CurrentTurn != null)
            {
                target.CurrentTurn.Movecount = 0;
                target.CurrentTurn.Actcount = 0;
            }
        }
    }

    public override IEnumerator ExecuteEffect()
    {
        yield return null;
        yield return new WaitForSeconds(0.7f);
    }
}

public class CallDeath : Effect
{
    public CallDeath(Actor target)
    {
        Targets = new List<Actor>();
        Targets.Add(target);
        MyType = EffectType.Special;
    }

    public override IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(0.1f);
        Targets[0].Death();
    }
}

public class TakeDamage : Effect
{
    Actor Victim;

    public TakeDamage(Actor targ, int strength)
    {
        Victim = targ;
        Efficacy = strength;
    }

    public override void SecondaryInit()
    {

    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Victim.MyProfile.ModifyResource(ActorResource.ResourceType.Health, Efficacy);
    }

    public override IEnumerator ExecuteEffect()
    {
        Victim.TakeDamageWiggle();
        Victim.ShowDamageNumbers(Efficacy);
        yield return new WaitForSeconds(0.1f);
    }
}

public class HealDamage : Effect
{
    public HealDamage(List<Actor> targets, int eff)
    {
        Targets = targets;
        Efficacy = eff;
    }

    public override void SecondaryInit()
    {
        //Efficacy = MyAct.MyModule.MyActor.MyProfile.GetStat(ActorStat.StatType.Attack);
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Targets[0].MyProfile.ModifyResource(ActorResource.ResourceType.Health, -Efficacy);
        if (Targets[0].MyProfile.GetResource(ActorResource.ResourceType.Health) > Targets[0].MyProfile.GetResourceRef(ActorResource.ResourceType.Health))
        {
            Targets[0].MyProfile.SetResource(ActorResource.ResourceType.Health, Targets[0].MyProfile.GetResourceRef(ActorResource.ResourceType.Health));
        }
        MyAct.MyModule.MyActor.TutorialSkillUsed = true;
    }

    public override IEnumerator ExecuteEffect()
    {
        if (MyAct.MyModule.MyActor.ThisType == Actor.UnitType.AI)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }
        // MyAct.MyModule.MyActor.PlayDamageSound();
        //Targets[0].TakeDamageWiggle();
        Targets[0].ShowHealNumbers(Efficacy);
        yield return new WaitForSeconds(0.7f);
    }
}


public class FakeDamage : Effect
{
    public FakeDamage(List<Actor> targets, int effs = 0)
    {
        MyType = EffectType.Damage;
        Targets = targets;
        Efficacy = effs;
    }

    public override void SecondaryInit()
    {

    }

    public override void ApplyEffect()
    {

    }

    public override IEnumerator ExecuteEffect()
    {
        if (MyAct.MyModule.MyActor.ThisType == Actor.UnitType.AI)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }
        // MyAct.MyModule.MyActor.PlayDamageSound();
        Targets[0].TakeDamageWiggle();
        Targets[0].ShowDamageNumbers(Efficacy);
        yield return new WaitForSeconds(0.1f);
    }
}

public class DealDamage : Effect
{
    public DealDamage(List<Actor> targets, int effs = 0)
    {
        MyType = EffectType.Damage;
        Targets = targets;
        Efficacy = effs;
    }

    public override void SecondaryInit()
    {
        if (Efficacy == 0)
        {
            Efficacy = MyAct.MyModule.MyActor.MyProfile.GetStat(ActorStat.StatType.Attack);
        }
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        float factor = 1f;
        if(MyAct.MyModule.MyActor.ThisAffinity == Targets[0].ThisAffinity)
        {
            factor = 1f;
        }
        else
        {
            switch (MyAct.MyModule.MyActor.ThisAffinity)
            {
                case Actor.Affinity.Aggresion:
                    if (Targets[0].ThisAffinity == Actor.Affinity.Logic)
                        factor = 1.5f;
                    break;
                case Actor.Affinity.Comedy:
                    if (Targets[0].ThisAffinity == Actor.Affinity.Aggresion)
                        factor = 1.5f;
                    break;
                case Actor.Affinity.Logic:
                    if (Targets[0].ThisAffinity == Actor.Affinity.Comedy)
                        factor = 1.5f;
                    break;
                default:
                    break;
            }
        }
        Efficacy = Mathf.RoundToInt(Efficacy * factor);
        Targets[0].MyProfile.ModifyResource(ActorResource.ResourceType.Health, Efficacy);
    }

    public override IEnumerator ExecuteEffect()
    {
        if (MyAct.MyModule.MyActor.ThisType == Actor.UnitType.AI)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }
       // MyAct.MyModule.MyActor.PlayDamageSound();
        Targets[0].TakeDamageWiggle();
        Targets[0].ShowDamageNumbers(Efficacy);
        yield return new WaitForSeconds(0.1f);
    }
}

public class Wait : Effect
{
    public float Duration;

    public Wait(float duration)
    {
        Duration = duration;
    }

    public override IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(Duration);
    }

}


public class SpawnDeathBubble : Effect
{
    public Actor target;
    public SpawnDeathBubble(Actor targ)
    {
        target = targ;
        MyType = EffectType.Special;
    }

    public override IEnumerator ExecuteEffect()
    {
        GameObject thing = GameObject.Instantiate(target.TurnManager.MyBrain.RedSBUB, target.gameObject.transform);
        Vector3 orig = thing.GetComponent<RectTransform>().position;
        target.PlayAttackSound();
        float tmer = 0f;
        float otmer = 4f;
        while (tmer < 0.3f)
        {
            tmer += Time.deltaTime * 0.3f;
            otmer += Time.deltaTime * 70f;
            if (otmer >= 1f)
            {
                int negX;
                int negY;
                if (Random.Range(0f, 100f) < 50f)
                {
                    negX = -1;
                }
                else
                {
                    negX = 1;
                }
                if (Random.Range(0f, 100f) > 50f)
                {
                    negY = -1;
                }
                else
                {
                    negY = 1;
                }

                float xcoe = Random.Range(20f, 100f) * 0.01f * negX * 0.1f * 0.1f;
                float ycoe = Random.Range(20f, 100f) * 0.01f * negY * 0.1f * 0.1f;
                thing.GetComponent<RectTransform>().position = new Vector3(orig.x + xcoe, orig.y + ycoe, orig.z - xcoe);
                otmer = 0f;
                yield return null;
            }
            yield return null;
        }
        thing.GetComponent<RectTransform>().position = orig;
        thing.GetComponent<SbubTextHolder>().DestroyMe();
    }
}


public class SpawnSpeechBubble : Effect
{
    public Actor target;
    public string Quote;
    public SpawnSpeechBubble(Actor targ, string qwt)
    {
        target = targ;
        MyType = EffectType.Special;
        Quote = qwt;
    }

    public override IEnumerator ExecuteEffect()
    {
        GameObject thing = GameObject.Instantiate(target.TurnManager.MyBrain.NormAllySbub, target.gameObject.transform);
        thing.GetComponent<SbubTextHolder>().InsultText.text = Quote;
        Vector3 orig = thing.GetComponent<RectTransform>().position;
        target.PlayAttackSound();
        var anc = target.GetComponentInChildren<AnimController>();
        anc.setAttacking();
        float tmer = 0f;
        float otmer = 4f;
        while (tmer < 0.2f)
        {
            tmer += Time.deltaTime * 0.3f;
            otmer += Time.deltaTime * 70f;
            if (otmer >= 1f)
            {
                int negX;
                int negY;
                if (Random.Range(0f, 100f) < 50f)
                {
                    negX = -1;
                }
                else
                {
                    negX = 1;
                }
                if (Random.Range(0f, 100f) > 50f)
                {
                    negY = -1;
                }
                else
                {
                    negY = 1;
                }

                float xcoe = Random.Range(20f, 100f) * 0.01f * negX * 0.1f * 0.1f;
                float ycoe = Random.Range(20f, 100f) * 0.01f * negY * 0.1f * 0.1f;
                thing.GetComponent<RectTransform>().position = new Vector3(orig.x + xcoe, orig.y + ycoe, orig.z - xcoe);
                otmer = 0f;
                yield return null;
            }
            yield return null;
        }
        thing.GetComponent<RectTransform>().position = orig;
        thing.GetComponent<SbubTextHolder>().DestroyMe();
        anc.setIdle();
    }
}

public class SpawnUnit : Effect
{
    public Vector2Int TargetLoc;
    public GameObject Unit;
    public TacticsManager Manager;
    public float WaitTime;

    public SpawnUnit(TacticsManager manager, GameObject unit, Vector2Int targ)
    {
        Manager = manager;
        TargetLoc = targ;
        Unit = unit;
        MyType = EffectType.Special;
        WaitTime = 0f;
    }

    public SpawnUnit(TacticsManager manager, GameObject unit, Vector2Int targ, float wait)
    {
        Manager = manager;
        TargetLoc = targ;
        Unit = unit;
        MyType = EffectType.Special;
        WaitTime = wait;
    }

    public override IEnumerator ExecuteEffect()
    {
        yield return new WaitForSeconds(WaitTime);
        Actor dude = GameObject.Instantiate(Unit, Manager.Level.ThisLevelReference.MyGrid[TargetLoc.x, TargetLoc.y].GetWalkablePoint(), Quaternion.identity).GetComponent<Actor>();
        Manager.InsertNewActor(dude, TargetLoc);
        dude.TakeDamageWiggle();
        dude.CurrentPosition = TargetLoc;
    }
}

public class CreateUnit : Effect
{
    public Vector2Int TargetLoc;
    public GameObject Unit;

    public CreateUnit(GameObject unit, Vector2Int targ)
    {
        TargetLoc = targ;
        Unit = unit;
        MyType = EffectType.Special;
    }

    public override IEnumerator ExecuteEffect()
    {
        if (MyAct.MyModule.MyActor.ThisType == Actor.UnitType.AI)
        {
            yield return new WaitForSeconds(0.1f);
        }
        IMinion dude = (IMinion)GameObject.Instantiate(Unit, Targets[0].GameManager.Level.ThisLevelReference.MyGrid[TargetLoc.x, TargetLoc.y].GetWalkablePoint(), Quaternion.identity).GetComponent<Actor>();
        Targets[0].PlayMagicEffect();
        dude.ReceiveMaster(Targets[0]);
        Actor guy = (Actor)dude;
        Targets[0].GameManager.InsertNewActor(guy, TargetLoc);
        guy.TakeDamageWiggle();
        guy.CurrentPosition = TargetLoc;
    }
}

public class Suicide : Effect
{
    public Suicide(Actor target)
    {
        Efficacy = 999;
        MyType = EffectType.Special;
        Targets = new List<Actor>();
        Targets.Add(target);
    }

    public override void ApplyEffect()
    {
        Targets[0].MyProfile.SetResource(ActorResource.ResourceType.Health, 0);
    }

    public override IEnumerator ExecuteEffect()
    {
        Targets[0].MyProfile.SetResource(ActorResource.ResourceType.Health, 0);
        Targets[0].ShowDamageNumbers(9999);
        Targets[0].PlaySuicideSound();
        yield return null;
    }
}

public class PermaVoltageBuff : Effect
{
    public PermaVoltageBuff(List<Actor> targets, int strength)
    {
        Efficacy = strength;
        MyType = EffectType.Special;
        Targets = targets;
    }

    public override void ApplyEffect()
    {
        foreach (Actor item in Targets)
        {
            item.MyProfile.ModifyStat(ActorStat.StatType.Voltage, -Efficacy);
        }
    }

    public override IEnumerator ExecuteEffect()
    {
        yield return null;
    }
}

public class Teleport : Effect
{
    Vector2Int targ;
    public Teleport(Actor target, Vector2Int loc)
    {
        Targets = new List<Actor>();
        Targets.Add(target);
        targ = loc;
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Targets[0].GameManager.Level.MoveActor(Targets[0], targ);
    }

    public override IEnumerator ExecuteEffect()
    {
        yield return null;
        Targets[0].transform.position = Targets[0].GameManager.Level.ThisLevelReference.MyGrid[targ.x, targ.y].GetWalkablePoint();
    }

}

public class DeathWalk : Effect
{
    public List<Vector2Int> TargetLoc;
    public int tarindex;
    public DeathWalk(List<Actor> targets, List<Vector2Int> loc)
    {
        MyType = EffectType.Movement;
        Targets = targets;
        TargetLoc = new List<Vector2Int>();
        foreach (Vector2Int item in loc)
        {
            TargetLoc.Add(new Vector2Int(item.x, item.y));
        }
        tarindex = TargetLoc.Count - 1;
        Efficacy = TargetLoc.Count - 1;
    }

    public override IEnumerator ExecuteEffect()
    {
        if (MyAct.MyModule.MyActor.ThisType == Actor.UnitType.AI)
        {
            yield return new WaitForSeconds(0.1f);
        }
        MyAct.MyModule.MyActor.FadeMe();
        float tym = 0f;
        var anc = MyAct.MyModule.MyActor.GetComponentInChildren<AnimController>();
        float stop = 0f;
        while (stop <= 1f)
        {
            stop += Time.deltaTime * 3f;
            while (TargetLoc.Count > 0)
            {
                Targets[0].TurnManager.CamTarget.transform.position = new Vector3(Targets[0].TurnManager.CurrentTurn.MyOwner.gameObject.transform.position.x, Targets[0].TurnManager.CamTarget.transform.position.y, Targets[0].TurnManager.CurrentTurn.MyOwner.gameObject.transform.position.z);
                Vector3 target = Targets[0].GameManager.Level.ThisLevelReference.MyGrid[TargetLoc.First().x, TargetLoc.First().y].GetWalkablePoint();
                Vector3 origin = Targets[0].gameObject.transform.position;
                Vector3 mag = (target - origin);
                mag.Normalize();
                if (mag.x > 0 || mag.z > 0)
                {
                    //run forwards
                    if (mag.x > 0)
                        anc.faceRight();


                    else
                        anc.faceLeft();



                    anc.runBack();
                }
                else
                {
                    if (mag.x < 0)
                        anc.faceLeft();
                    else
                        anc.faceRight();

                    anc.runFront();
                }
                while ((target - Targets[0].gameObject.transform.position).magnitude > 0.0f)
                {
                    Targets[0].TurnManager.CamTarget.transform.position = new Vector3(Targets[0].TurnManager.CurrentTurn.MyOwner.gameObject.transform.position.x, Targets[0].TurnManager.CamTarget.transform.position.y, Targets[0].TurnManager.CurrentTurn.MyOwner.gameObject.transform.position.z);
                    tym += Time.deltaTime * 2.3f;
                    Targets[0].gameObject.transform.position = Vector3.Lerp(origin, target, tym);
                    if ((target - origin).magnitude <= 0.1f)
                    {
                        Targets[0].gameObject.transform.position = target;
                    }
                    yield return null;
                }
                tym = 0f;
                TargetLoc.RemoveAt(0);
            }
            yield return null;
        }
        anc.setIdle();
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Targets[0].GameManager.Level.MoveActor(Targets[0], TargetLoc[tarindex]);
    }

}



public class Walk : Effect
{
    public List<Vector2Int> TargetLoc;
    public int tarindex;
    public Walk(List<Actor> targets, List<Vector2Int> loc)
    {
        MyType = EffectType.Movement;
        Targets = targets;
        TargetLoc = new List<Vector2Int>();
        foreach (Vector2Int item in loc)
        {
            TargetLoc.Add(new Vector2Int(item.x, item.y));
        }
        tarindex = TargetLoc.Count - 1;
        Efficacy = TargetLoc.Count - 1;
    }

    public override IEnumerator ExecuteEffect()
    {
        if (MyAct.MyModule.MyActor.ThisType == Actor.UnitType.AI)
        {
            yield return new WaitForSeconds(0.1f);
        }
        float tym = 0f;
        var anc = MyAct.MyModule.MyActor.GetComponentInChildren<AnimController>();
        while (TargetLoc.Count > 0)
        {
            Targets[0].TurnManager.CamTarget.transform.position = new Vector3(Targets[0].TurnManager.CurrentTurn.MyOwner.gameObject.transform.position.x, Targets[0].TurnManager.CamTarget.transform.position.y, Targets[0].TurnManager.CurrentTurn.MyOwner.gameObject.transform.position.z);
            Vector3 target = Targets[0].GameManager.Level.ThisLevelReference.MyGrid[TargetLoc.First().x, TargetLoc.First().y].GetWalkablePoint();
            Vector3 origin = Targets[0].gameObject.transform.position;
            Vector3 mag = (target - origin);
            mag.Normalize();
            if (mag.x > 0 || mag.z > 0)
            {
                //run forwards
                if (mag.x > 0)
                    anc.faceRight();


                else
                    anc.faceLeft();



                anc.runBack();
            }
            else
            {
                if (mag.x < 0)
                    anc.faceLeft();
                else
                    anc.faceRight();

                anc.runFront();
            }
            while ((target - Targets[0].gameObject.transform.position).magnitude > 0.0f)
            {
                Targets[0].TurnManager.CamTarget.transform.position = new Vector3(Targets[0].TurnManager.CurrentTurn.MyOwner.gameObject.transform.position.x, Targets[0].TurnManager.CamTarget.transform.position.y, Targets[0].TurnManager.CurrentTurn.MyOwner.gameObject.transform.position.z);
                tym += Time.deltaTime * 2.3f;
                Targets[0].gameObject.transform.position = Vector3.Lerp(origin, target, tym);
                if ((target - origin).magnitude <= 0.1f)
                {
                    Targets[0].gameObject.transform.position = target;
                }
                yield return null;
            }
            tym = 0f;
            TargetLoc.RemoveAt(0);
        }
        anc.setIdle();
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Targets[0].GameManager.Level.MoveActor(Targets[0], TargetLoc[tarindex]);
    }

}

public class Cut : Effect
{
    Actor MyActor;
    public Cut(Actor myActor, List<Actor> targets)
    {
        this.MyActor = myActor;
        this.Targets = targets;
        MyType = EffectType.Cut;
    }

    public override IEnumerator ExecuteEffect()
    {
        MyActor.PlaySkillEffect((Targets[0].transform.position + Targets[1].transform.position) / 2);
        yield return null;
    }
}
