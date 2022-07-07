using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;

public class Actor : MonoBehaviour
{

    public enum UnitType { Organic, AI, Component, Object };
    public enum Affinity { Logic, Comedy, Aggresion, Nonsentient };
    public TacticsManager GameManager;
    public Turnstile TurnManager;
    public List<Module> MyModules;
    public ActorProfile MyProfile;
    public int CurrentLevel;
    public UnitType ThisType;
    public Affinity ThisAffinity;

    public Sprite Portrait;
    public Sprite Slider;
    public Sprite TurnCutOut;
    public TargetingModule TargModule;

    public bool TutorialSkillUsed;

    public SpriteRenderer CharacterSprite;
    public GameObject BuffEffect;
    public GameObject DebuffEffect;
    public GameObject SpecialEffect;

    public string Name;

    public bool IsObstruction;
    public Vector2Int CurrentPosition;
    public Turn CurrentTurn;
    public List<Turn> TurnHistory;
    public List<Affect> CurrentAffects;

    public AudioClip MagicSound;
    public AudioClip SpecialSound;
    public AudioClip BuffSound;
    public AudioClip DebuffSound;
    public AudioClip HurtSound;
    public AudioSource MyAudio;
    public AudioClip DeathSound;

    public AudioClip AttackSound;
    public AudioClip AttackSound2;
    public AudioClip AttackSound3;
    public List<Skill> Skills;

    public int TurnMoveCount;

    public StatusManager StatMan;
    public PrefabServer MyPrefabServer;
    public GameObject MyHpBar;

    public bool IsObstructingTile(Vector2Int coords)
    {
        if (!IsObstruction)
        {
            return false;
        }
        else if (CurrentPosition == coords && IsObstruction == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void PlayAttackSound()
    {
        float rand = UnityEngine.Random.Range(0, 100);

        if (rand >= 0f && rand <= 30f)
        {
            MyAudio.PlayOneShot(AttackSound);
        }
        else if (rand >= 30f && rand <= 60f)
        {
            MyAudio.PlayOneShot(AttackSound2);
        }
        else
        {
            MyAudio.PlayOneShot(AttackSound3);
        }
    }

    public virtual void PlayCritSound()
    {
        MyAudio.PlayOneShot(AttackSound3);
    }

    public IEnumerator FadeOut()
    {
        var cv = this.GetComponentInChildren<SpriteRenderer>();
        while (cv.color.a >= 0.01f)
        {
            Color col = cv.color;
            col.a = cv.color.a - (0.8f * Time.deltaTime);
            cv.color = col;
            yield return null;
        }
    }

    public virtual void FadeMe()
    {
        StartCoroutine(FadeOut());
    }

    public virtual string ReturnInsult()
    {
        float rand = UnityEngine.Random.Range(0, 100);
        if (ThisType == UnitType.AI)
        {
            if (ThisAffinity == Affinity.Aggresion)
            {
                if (rand >= 0f && rand <= 30f)
                {
                    return "You leg humping fart sniffer!";
                }
                else if (rand >= 30f && rand <= 60f)
                {
                    return "You butt licking flea herder!";
                }
                else
                {
                    return "You turd throwing animal!";
                }
            }
            else if (ThisAffinity == Affinity.Logic)
            {
                if (rand >= 0f && rand <= 30f)
                {
                    return "A brainless bag of fur!";
                }
                else if (rand >= 30f && rand <= 60f)
                {
                    return "Untrained mutt!";
                }
                else
                {
                    return "Backwards uneducated cattle!";
                }
            }
            else if (ThisAffinity == Affinity.Comedy)
            {
                if (rand >= 0f && rand <= 30f)
                {
                    return "Your mother is my handbag!";
                }
                else if (rand >= 30f && rand <= 60f)
                {
                    return "Poop coffee producer!";
                }
                else
                {
                    return "Keep rolling in the dirt for me!";
                }
            }
        }
        else
        {
            if (ThisAffinity == Affinity.Aggresion)
            {
                if (rand >= 0f && rand <= 30f)
                {
                    return "I know you’re angry but you do not want that to define you! Trust me!";
                }
                else if (rand >= 30f && rand <= 60f)
                {
                    return "I’ve been in that pit of anger like you! Let go, you’ll feel better.";
                }
                else
                {
                    return "Stop this! You wouldn’t want it happening to you, don’t you?";
                }
            }
            else if (ThisAffinity == Affinity.Logic)
            {
                if (rand >= 0f && rand <= 30f)
                {
                    return "We work as hard anyone else in society.";
                }
                else if (rand >= 30f && rand <= 60f)
                {
                    return "We can all be so much stronger if we embrace this diversity!";
                }
                else
                {
                    return "Imagine the potential of growth if you just give us a chance.";
                }
            }
            else if (ThisAffinity == Affinity.Comedy)
            {
                if (rand >= 0f && rand <= 30f)
                {
                    return "I see your point, but do you really think you could call your siblings that?";
                }
                else if (rand >= 30f && rand <= 60f)
                {
                    return "Think long and hard about those words, they could be used against you too!";
                }
                else
                {
                    return "Even you have to admit that’s doing more harm than good.";
                }
            }
        }
        return "You smell like butts!";
    }

    public bool CheckAdjacentPathing()
    {
        bool n = true;
        bool s = true;
        bool e = true;
        bool w = true;
        if (TurnManager.ActorQueue.Find(x => x.CurrentPosition == new Vector2Int(CurrentPosition.x + 1, CurrentPosition.y)))
        {
            e = false;
        }
        if (TurnManager.ActorQueue.Find(x => x.CurrentPosition == new Vector2Int(CurrentPosition.x - 1, CurrentPosition.y)))
        {
            w = false;
        }
        if (TurnManager.ActorQueue.Find(x => x.CurrentPosition == new Vector2Int(CurrentPosition.x, CurrentPosition.y + 1)))
        {
            s = false;
        }
        if (TurnManager.ActorQueue.Find(x => x.CurrentPosition == new Vector2Int(CurrentPosition.x, CurrentPosition.y - 1)))
        {
            n = false;
        }

        if (GameManager.Level.ThisLevel.Exists(new Vector2Int(CurrentPosition.x + 1, CurrentPosition.y)) == true)
        {
            if (GameManager.Level.ThisLevel.MyGrid[CurrentPosition.x + 1, CurrentPosition.y].IsObstruction)
            {
                e = false;
            }
        }
        else
        {
            e = false;
        }
        if (GameManager.Level.ThisLevel.Exists(new Vector2Int(CurrentPosition.x - 1, CurrentPosition.y)) == true)
        {
            if (GameManager.Level.ThisLevel.MyGrid[CurrentPosition.x - 1, CurrentPosition.y].IsObstruction == true)
            {
                w = false;
            }
        }
        else
        {
            w = false;
        }
        if (GameManager.Level.ThisLevel.Exists(new Vector2Int(CurrentPosition.x, CurrentPosition.y + 1)) == true)
        {
            if (GameManager.Level.ThisLevel.MyGrid[CurrentPosition.x, CurrentPosition.y + 1].IsObstruction == true)
            {
                s = false;
            }
        }
        else
        {
            s = false;
        }
        if (GameManager.Level.ThisLevel.Exists(new Vector2Int(CurrentPosition.x, CurrentPosition.y - 1)) == true)
        {
            if (GameManager.Level.ThisLevel.MyGrid[CurrentPosition.x, CurrentPosition.y - 1].IsObstruction == true)
            {
                n = false;
            }
        }
        else
        {
            n = false;
        }

        if (e == false)
        {
            if (s == false)
            {
                if (w == false)
                {
                    if (n == false)
                    {
                        return false;
                    }
                }
            }
        }


        return true;
    }

    public virtual void PlayBuff()
    {
        MyAudio.PlayOneShot(BuffSound);
        var thing = Instantiate(BuffEffect, this.transform.position, Quaternion.identity);
    }

    public virtual void PlayDebuff()
    {
        MyAudio.PlayOneShot(DebuffSound);
        var thing = Instantiate(DebuffEffect, this.transform.position, Quaternion.identity);
    }

    public virtual void PowerDown()
    {

    }

    public virtual void PowerUp()
    {

    }

    public virtual void TakeDamageWiggle()
    {
        PlayDamageSound();
        StartCoroutine(WiggleEffect());
    }

    public virtual void Death()
    {
        PlayDeathSound();
        foreach (Affect item in CurrentAffects)
        {
            item.Unsubscribe();
        }
        GameManager.RemoveActor(this);
    }

    public virtual IEnumerator WiggleEffect()
    {
        int counter = 5;
        while (counter > 0)
        {
            CharacterSprite.color = Color.clear;
            yield return new WaitForSeconds(0.07f);
            CharacterSprite.color = Color.white;

            yield return new WaitForSeconds(0.07f);
            counter--;
        }
    }

    public virtual void ShowDamageNumbers(int numbers)
    {
        var thing = Instantiate(GameManager.DamageText, this.transform.position, Quaternion.identity);
        thing.GetComponent<ScrollingTextBehavior>().SetText(numbers);
        MyHpBar.SetActive(true);
        StartCoroutine(MyHpBar.GetComponent<HealthbarScript>().UpdateHealth((float)((float)MyProfile.GetResource(ActorResource.ResourceType.Health) / (float)MyProfile.GetResourceRef(ActorResource.ResourceType.Health))));
    }

    public virtual void ShowHealNumbers(int numbers)
    {
        var thing = Instantiate(GameManager.HealText, this.transform.position, Quaternion.identity);
        thing.GetComponent<ScrollingTextBehavior>().SetText(numbers);
        MyHpBar.SetActive(true);
        StartCoroutine(MyHpBar.GetComponent<HealthbarScript>().UpdateHealth((float)((float)MyProfile.GetResource(ActorResource.ResourceType.Health) / (float)MyProfile.GetResourceRef(ActorResource.ResourceType.Health))));
    }

    public virtual void ShowStunText()
    {
        var thing = Instantiate(GameManager.StunText, this.transform.position, Quaternion.identity);
    }

    public virtual void PlayDamageSound()
    {
        MyAudio.PlayOneShot(HurtSound);
    }

    public virtual void PlayDeathSound()
    {

    }

    public virtual void PlaySuicideSound()
    {

    }

    public virtual void Initialize(int input, TacticsManager GM, Turnstile TM)
    {
        TutorialSkillUsed = false;
        MyAudio = GetComponent<AudioSource>();
        TurnManager = TM;
        GameManager = GM;
        CurrentLevel = input;
        CurrentAffects = new List<Affect>();
        MyProfile = new ActorProfile();
        MyModules = new List<Module>();
        Skills = new List<Skill>();
        TurnHistory = new List<Turn>();
        BuildProfile();
        InitStats();
        InitModules();
        var anim = GetComponentInChildren<AnimController>();
        if (anim != null)
        {
            anim.setIdle();
        }
        MyPrefabServer = GameObject.FindGameObjectWithTag("prefserv").GetComponent<PrefabServer>();
        MyHpBar = Instantiate(MyPrefabServer.HPBar, this.gameObject.transform);
        MyHpBar.SetActive(false);

        StatMan = this.gameObject.AddComponent<StatusManager>();
        StatMan.Init(this);
    }

    protected virtual void BuildProfile()
    {

    }

    public virtual void PlayMagicEffect()
    {
        MyAudio.PlayOneShot(MagicSound);
    }

    public virtual void PlaySkillEffect(Vector3 loc)
    {
        Instantiate(SpecialEffect, loc, Quaternion.identity);
        MyAudio.PlayOneShot(SpecialSound);
    }

    public virtual void NotifyTurnEnd()
    {
        CurrentTurn = null;
    }

    public virtual void NotifyTurnBegin()
    {
        if (MyProfile.HasStat(ActorStat.StatType.Movement))
            TurnMoveCount = MyProfile.GetStat(ActorStat.StatType.Movement);

        if (Skills.Count > 0)
        {
            foreach (Skill item in Skills)
            {
                if (item.CurrentCooldown > 0)
                {
                    item.CurrentCooldown--;
                }
            }
        }

        StatMan.UpdateStatuses();

        if (CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Stun))
        {
            if (TurnManager.CurrentTurn.MyOwner == this)
            {
                if (TurnManager.CurrentTurn.MyOwner.ThisType == UnitType.Organic)
                {
                    TurnManager.UI.DisableUI();
                }
                ShowStunText();
                var bas = (BasicModule)MyModules.Find(x => x is BasicModule);
                CurrentTurn.SubmitAction(bas.CreateEndTurnAct());
                return;
            }
        }
    }

    public virtual void NotifyActReady()
    {

    }

    protected virtual void InitStats()
    {
        MyProfile.Init(CurrentLevel);
    }

    protected virtual void InitModules()
    {
        var tar = new TargetingModule(this);
        var log = new Logic(this);
        var agr = new Aggression(this);
        var com = new Comedy(this);
        MyModules.Add(tar);
        MyModules.Add(log);
        MyModules.Add(agr);
        MyModules.Add(com);
        TargModule = tar;
    }

    protected virtual void Unsubscribe()
    {
        if (CurrentAffects != null)
        {
            if (CurrentAffects.Count > 0)
            {
                foreach (Affect aff in CurrentAffects)
                {
                    aff.Unsubscribe();
                }
            }
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        Unsubscribe();
    }
}

public class PlayerActor : Actor
{
    public TacticUI UI;
    public bool EnableTutorial;
    public virtual void GetEndTurnAction()
    {

    }

    public virtual void TutorialFunc()
    {

    }


    public virtual void GetWalkAction(Vector2Int targ)
    {

    }

    public virtual bool GetAttackAction(Vector2Int targ)
    {
        return true;
    }
    public override void Death()
    {
        base.Death();
    }

    public override void Initialize(int input, TacticsManager GM, Turnstile TM)
    {
        base.Initialize(input, GM, TM);
        EnableTutorial = true;
    }

    public override void NotifyActReady()
    {
        base.NotifyActReady();
    }

    public override void NotifyTurnBegin()
    {
        base.NotifyTurnBegin();
        TutorialFunc();
        UI.CurrentActor = this;
        UI.PlayerMenu.SetActive(true);
    }

    public override void NotifyTurnEnd()
    {
        base.NotifyTurnEnd();
    }

    public override void PlayBuff()
    {
        base.PlayBuff();
    }

    public override void PlayDamageSound()
    {
        base.PlayDamageSound();
    }

    public override void PlayDeathSound()
    {
        base.PlayDeathSound();
    }

    public override void PlayDebuff()
    {
        base.PlayDebuff();
    }

    public override void PlayMagicEffect()
    {
        base.PlayMagicEffect();
    }

    public override void PlaySkillEffect(Vector3 loc)
    {
        base.PlaySkillEffect(loc);
    }

    public override void PlaySuicideSound()
    {
        base.PlaySuicideSound();
    }

    public override void PowerDown()
    {
        base.PowerDown();
    }

    public override void PowerUp()
    {
        base.PowerUp();
    }

    public override void ShowDamageNumbers(int numbers)
    {
        base.ShowDamageNumbers(numbers);
    }

    public override void TakeDamageWiggle()
    {
        base.TakeDamageWiggle();
    }

    public override IEnumerator WiggleEffect()
    {
        return base.WiggleEffect();
    }

    protected override void BuildProfile()
    {
        base.BuildProfile();
    }

    protected override void InitModules()
    {
        base.InitModules();
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

public class ActorProfile
{
    public int CurrentLevel;
    public List<ActorResource> MyResources;
    public List<ActorStat> MyStats;
    public List<ActorStat> BaseStatReference;
    public List<ActorStat> CurrentStats;
    public List<ActorResource> BaseResourceReference;
    public List<ActorResource> CurrentResources;


    public void Init(int input)
    {
        CurrentLevel = input;
        foreach (ActorStat item in MyStats)
        {
            CurrentStats.Add(new ActorStat
            {
                BaseValue = item.BaseValue,
                Type = item.Type,
                Growth = item.Growth,
                Value = item.BaseValue + (item.Growth * CurrentLevel)
            });
            BaseStatReference.Add(new ActorStat
            {
                BaseValue = item.BaseValue,
                Type = item.Type,
                Growth = item.Growth,
                Value = item.BaseValue + (item.Growth * CurrentLevel)
            });
        }
        foreach (ActorResource item in MyResources)
        {
            CurrentResources.Add(new ActorResource
            {
                BaseValue = item.BaseValue,
                Type = item.Type,
                Growth = item.Growth,
                Value = item.BaseValue + (item.Growth * CurrentLevel)
            });
            BaseResourceReference.Add(new ActorResource
            {
                BaseValue = item.BaseValue,
                Type = item.Type,
                Growth = item.Growth,
                Value = item.BaseValue + (item.Growth * CurrentLevel)
            });
        }

    }

    public ActorProfile()
    {
        MyResources = new List<ActorResource>();
        MyStats = new List<ActorStat>();
        BaseStatReference = new List<ActorStat>();
        BaseResourceReference = new List<ActorResource>();
        CurrentStats = new List<ActorStat>();
        CurrentResources = new List<ActorResource>();
    }

    public bool HasStat(ActorStat.StatType input)
    {
        if (CurrentStats.Exists(x => x.Type == input))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasResource(ActorResource.ResourceType input)
    {
        if (CurrentResources.Exists(x => x.Type == input))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ModifyResource(ActorResource.ResourceType type, int value)
    {
        if (HasResource(type))
            CurrentResources.Find(x => x.Type == type).Value -= value;
        else
    }

    public void SetResource(ActorResource.ResourceType type, int value)
    {
        if (HasResource(type))
            CurrentResources.Find(x => x.Type == type).Value = value;
        else
    }

    public void SetStat(ActorStat.StatType type, int value)
    {
        if (HasStat(type))
            CurrentStats.Find(x => x.Type == type).Value = value;
        else
    }

    public void ModifyStat(ActorStat.StatType type, int value)
    {
        if (HasStat(type))
            CurrentStats.Find(x => x.Type == type).Value -= value;
        else
    }

    public int GetResourceRef(ActorResource.ResourceType input)
    {
        if (HasResource(input))
            return BaseResourceReference.Find(x => x.Type == input).Value;
        else
        {
            return 0;
        }
    }

    public int GetResource(ActorResource.ResourceType input)
    {
        if (HasResource(input))
            return CurrentResources.Find(x => x.Type == input).Value;
        else
        {
            return 0;
        }
    }

    public int GetStat(ActorStat.StatType input)
    {
        if (HasStat(input))
            return CurrentStats.Find(x => x.Type == input).Value;
        else
        {
            return 0;
        }
    }

    public int GetStatRef(ActorStat.StatType input)
    {
        if (HasStat(input))
            return BaseStatReference.Find(x => x.Type == input).Value;
        else
        {
            return 0;
        }
    }
}

public class ActorResource : IComparable<ActorResource>
{
    public enum ResourceType { Health, Wattage };
    public ResourceType Type;
    public int Growth;
    public int BaseValue;
    public int Value;

    public int CompareTo(ActorResource input)
    {
        if (input == null) return 1;

        else
            return this.Value.CompareTo(input.Value);
    }
}

public class ActorStat : IComparable<ActorStat>
{
    public enum StatType { Speed, Movement, Attack, Range, Resistance, Voltage, ResistanceFactor };
    public StatType Type;
    public int Growth;
    public int BaseValue;
    public int Value;

    public int CompareTo(ActorStat input)
    {
        if (input == null) return 1;

        else
            return this.Value.CompareTo(input.Value);
    }
}
