using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lev0Script : LevelScript {
    public GameObject FakeWinScreen;
    public GameObject ArrivalBanner;

    public bool TransitionBlocker;
    public GameObject Doc;
    public GameObject ThugG;
    public GameObject ThugF;
    public GameObject ThugC;

    public GameObject FightScreen;
    public GameObject FightForRealScreen;

    public override void Init(Turnstile turnmanager)
    {
        base.Init(turnmanager);
        TransitionBlocker = true;
        ObjectiveFlags.Add('z', "Get that battery!");
        ObjectiveFlags.Add('x', "Defeat all the thugs!");


        var dl1 = new List<DialogueUnit>();
        dl1.Add(new DialogueUnit("Morgan", "We're done! It's working! It's--"));
        dl1.Add(new DialogueUnit("Morgan", "NO!"));
        dl1.Add(new DialogueUnit("Corgo", "Please Human, calm yourself. It was just a power fluctuation. I believe the cause of it is outside."));
        dl1.Add(new DialogueUnit("Morgan", "Outside? Oh, they look nasty. We better stop them then!"));
        dl1.Add(new DialogueUnit("Corgo", "I don't think opening the window and interfering is...wise."));
        var cl1 = new List<char>();
        cl1.Add('b');
        LevelDialogue.Add(new DialogueSequence(dl1, 'a', cl1));

        var dl2 = new List<DialogueUnit>();
        dl2.Add(new DialogueUnit("Morgan", "Hey! What are you doing with the generator?"));
        dl2.Add(new DialogueUnit("Cortni", "We're teaching those freaks at the hospital that medicine ain't for them. of course, this apartment block is a sweet bonus."));
        dl2.Add(new DialogueUnit("Fabio", "Yeah, they're trash for a reason, little girl. Now buzz off!"));
        dl2.Add(new DialogueUnit("Morgan", "But can't you see that's a bad thing to do?"));
        dl2.Add(new DialogueUnit("Cortni", "Can't you see this is absolutely none of your business? Buzz. Off."));
        dl2.Add(new DialogueUnit("Morgan", "I'm not going to stand by and just let you do this!"));   
        var cl2 = new List<char>();
        cl2.Add('X');
        LevelDialogue.Add(new DialogueSequence(dl2, 'd', cl2));

        var dl3 = new List<DialogueUnit>();
        dl3.Add(new DialogueUnit("Fabio", "Hey! That's not how this is supposed to go! You gimme that battery now, girl."));
        dl3.Add(new DialogueUnit("Morgan", "I'm only letting go of it to put it back on the generator!"));
        dl3.Add(new DialogueUnit("Fabio", "I gave you a warning, girl. I'm not repeating myself!"));
        var cl3 = new List<char>();
        cl3.Add('k');
        LevelDialogue.Add(new DialogueSequence(dl3, 'j', cl3));

        var dl4 = new List<DialogueUnit>();
        dl4.Add(new DialogueUnit("Morgan", "It's horrible that someone would try something like this! They ought to be ashamed!"));
        dl4.Add(new DialogueUnit("Corgo", "We put the battery back, that's what matters. I'll play a victory tune. That's fitting!"));
        var cl4 = new List<char>();
        cl4.Add('y');
        LevelDialogue.Add(new DialogueSequence(dl4, 'q', cl4));

        var dl5 = new List<DialogueUnit>();
        dl5.Add(new DialogueUnit("Johnthony", "Hey, that's a nice tune you got there. Did I miss a party?"));
        dl5.Add(new DialogueUnit("Fabio", "Oh no, it's Johnthony!"));
        dl5.Add(new DialogueUnit("Johnthony", "That's right, Fabio. I'll deal with you two failures later. Gotta play with this girl first."));
        var cl5 = new List<char>();
        cl5.Add('B');
        LevelDialogue.Add(new DialogueSequence(dl5, 'A', cl5));

        var dl6 = new List<DialogueUnit>();
        dl6.Add(new DialogueUnit("Corgo", "Ah, I see it's the ringleader of these two?"));
        dl6.Add(new DialogueUnit("Morgan", "You should tell your friends that what they're doing is wrong."));
        dl6.Add(new DialogueUnit("Johnthony", "I'm sorry, I don't think I ordered a stand up comedian. Good joke though."));
        dl6.Add(new DialogueUnit("Kendrew", "Hey! Look what I found, Johnthony! One of those freaks playing nurse in the hospital."));
        var cl6 = new List<char>();
        cl6.Add('D');
        LevelDialogue.Add(new DialogueSequence(dl6, 'C', cl6));

        var dl7 = new List<DialogueUnit>();
        dl7.Add(new DialogueUnit("Doc", "Let me go! I am a LICENSED MEDICAL PRACTITIONER!"));
        dl7.Add(new DialogueUnit("Johnthony", "You're a failure is what you are, rat. Kendrew, how about you argue the merits of being human to it."));
        var cl7 = new List<char>();
        cl7.Add('E');
        LevelDialogue.Add(new DialogueSequence(dl7, 'U', cl7));

        var dl8 = new List<DialogueUnit>();
        dl8.Add(new DialogueUnit("Johnthony", "Oh, what's this? Girl's a freak lover is she? Makes sense when she's living with squatters."));
        dl8.Add(new DialogueUnit("Corgo", "Human! Be careful!"));
        dl8.Add(new DialogueUnit("Doc", "Thank you! Oh, are you alright? Here, let me just take a look..."));
        dl8.Add(new DialogueUnit("Corgo", "Watch out! They look like they mean business!"));
        var cl8 = new List<char>();
        cl8.Add('G');
        LevelDialogue.Add(new DialogueSequence(dl8, 'F', cl8));

        var dl9 = new List<DialogueUnit>();
        dl9.Add(new DialogueUnit("Corgo", "Doctor, please mend the human right now."));
        var cl9 = new List<char>();
        cl9.Add('V');
        LevelDialogue.Add(new DialogueSequence(dl9, 'H', cl9));

        var dl10 = new List<DialogueUnit>();
        dl10.Add(new DialogueUnit("Corgo", "Did you hit the red area? You should have. You're a rodent doctor."));
        dl10.Add(new DialogueUnit("Corgo", "Landing on the yellow area delivers normal potency."));
        dl10.Add(new DialogueUnit("Corgo", "Landing on the white area delivers minimal effect."));
        dl10.Add(new DialogueUnit("Corgo", "To review: it's preferable if you tap on the red area. Yellow area is normal potency. White is minimal effect."));
        dl10.Add(new DialogueUnit("Morgan", "Yeah I'm feeling better. Thanks a lot. Just the kind of encouragement I needed."));
        dl10.Add(new DialogueUnit("Doc", "Absolutely no problem, miss. Now let's go and lea--"));
        dl10.Add(new DialogueUnit("Johnthony", "Oi! Who said you could just talk and ignore us. Get 'em, boys!"));
        var cl10 = new List<char>();
        cl10.Add('J');
        LevelDialogue.Add(new DialogueSequence(dl10, 'I', cl10));

        var dl11 = new List<DialogueUnit>();
        dl11.Add(new DialogueUnit("Doc", "Well that was...unpleasant."));
        dl11.Add(new DialogueUnit("Morgan", "I may have found it a little entertaining. It feels good to knock them down a few pegs after all. "));
        dl11.Add(new DialogueUnit("Doc", "That wasn't necessary. We could have avoided this. Words can do damage too."));
        dl11.Add(new DialogueUnit("Morgan", "Doesn't hurt as much as getting punched, honestly."));
        dl11.Add(new DialogueUnit("Doc", "Wish I could share your outlook, neighbor. It's just that this does the kind of damage that's on a delayed trigger."));
        dl11.Add(new DialogueUnit("Morgan", "Neighbor. My name's Morgan, a little easier to use yeah? "));
        dl11.Add(new DialogueUnit("Doc", "And mine is Doctor Ferret. Just call me doc. As I was saying. The damage those words do comes at the worst moments."));
        dl11.Add(new DialogueUnit("Morgan", "I suppose you're right, I did feel a little guilty yelling all that. But what choice do we have? Let them push us over?"));
        dl11.Add(new DialogueUnit("Doc", "That's not what I'm saying, but we could at least endeavor to be more careful with our words. We probably won't even see the damage happen ourselves."));
        dl11.Add(new DialogueUnit("Morgan", "So you're telling me I need to tiptoe around them? That's asking a lot out of anyone, especially in a heated argument."));
        dl11.Add(new DialogueUnit("Doc", "That doesn't excuse the fact we need to be careful. We need to be the better people out of this. They will come around eventually."));
        dl11.Add(new DialogueUnit("Doc", "It's getting late, there's hoodlums around. We can resume this debate another time."));
        var cl11 = new List<char>();
        cl11.Add('R');
        LevelDialogue.Add(new DialogueSequence(dl11, 'Q', cl11));

       /* var dl12 = new List<DialogueUnit>();
        dl12.Add(new DialogueUnit("Corgo", "Did you hit the red area? You should have. You're a rodent doctor."));
        dl12.Add(new DialogueUnit("Corgo", "Landing on the yellow area delivers normal potency."));
        dl12.Add(new DialogueUnit("Corgo", "Landing on the white area delivers minimal effect."));
        dl12.Add(new DialogueUnit("Corgo", "To Review: it's preferable if you tap on the red area. Yellow area is normal potency. White is minimal effect."));
        dl12.Add(new DialogueUnit("Morgan", "That's the stuff. I'm feeling better already. How'd you do it, neighbor?"));
        dl12.Add(new DialogueUnit("Doc", "Preparation. My binder has four different sections because that's apparently all it can hold--"));
        dl12.Add(new DialogueUnit("Johnthony", "Oi! Who said you could just talk and ignore us. Get 'em boys!"));
        var cl12 = new List<char>();
        cl12.Add('T');
        LevelDialogue.Add(new DialogueSequence(dl12, 'S', cl12));*/
    }

    public override void NoEnemiesLeft()
    {
        if (!TransitionBlocker)
        {
            base.NoEnemiesLeft();
            SendFlag('Q');
        }
    }

    public override void SendFlag(char input)
    {
        base.SendFlag(input);
        if (input == 'B')
        {
            SpawnOtherDudes();
        }
        else if (input == 'D')
        {
            SpawnDoc();
        }
        else if (input == 'E')
        {
            var doo = GameObject.FindGameObjectWithTag("mus").GetComponent<MusicMan>();
            doo.Play(doo.Combat);
            MorgDash();
        }
        else if (input == 'X')
        {
            var doo = GameObject.FindGameObjectWithTag("mus").GetComponent<MusicMan>();
            doo.Play(doo.Combat);
        }
        else if(input == 'F')
        {
           // DocDash();
        }
        else if (input == 'G')
        {
            ShowFakePanel();
        }
        else if (input == 'J')
        {
            var dude = TurnManager.ActorQueue.Find(x => x.Name == "Doc");
            var morgan = (PlayerActor)dude;
            morgan.EnableTutorial = false;
            morgan.CurrentAffects = new List<Affect>();
            ShowRealPanel();
            TransitionBlocker = false;
        }
        else if (input == 'R')
        {
            StartCoroutine(LevelOutro());
        }
        else if (input == 'V')
        {
            var dude = TurnManager.ActorQueue.Find(x => x.Name == "Doc");
            var morgan = (PlayerActor)dude;
            dude.CurrentAffects.Add(new ActionLock(dude));
            morgan.NotifyActReady();
            TutPanel.SetActive(true);
            FacilitateTut();
        }
        else if (input == 'b')
        {
            StartCoroutine(FadeIn());
        }
        else if (input == 'c')
        {
            MoveMorgan();
        }
        else if (input == 'i')
        {
            if (TurnManager.ActorQueue.Exists(x => x.Name == "Morgan"))
            {
                var dude = TurnManager.ActorQueue.Find(x => x.Name == "Morgan");
                dude.CurrentAffects.Add(new ActionLock(dude));
                var morgan = (PlayerActor)dude;
                morgan.EnableTutorial = false;
                FirstTutorial = false;
            }
        }
        else if(input == 'j')
        {
            ReceiveTriggerFlag('z');
        }
        else if (input == 'k')
        {
            TutPanel.SetActive(false);
            ArrivalBanner.SetActive(true);
            ArrivalBanner.GetComponent<TimedDestroy>().DestroyMe(1.7f);
            TutorialText.text = "";
            if (TurnManager.ActorQueue.Exists(x => x.Name == "Fabio"))
            {
                var dude = TurnManager.ActorQueue.Find(x => x.Name == "Fabio");
                dude.CurrentAffects = new List<Affect>();
            }
            if (TurnManager.ActorQueue.Exists(x => x.Name == "Morgan"))
            {
                var dude = TurnManager.ActorQueue.Find(x => x.Name == "Morgan");
                dude.CurrentAffects = new List<Affect>();
            }
            if(TurnManager.CurrentTurn.MyOwner.Name == "Morgan")
            {
                TurnManager.CurrentTurn.MyOwner.TurnMoveCount = TurnManager.CurrentTurn.MyOwner.MyProfile.GetStat(ActorStat.StatType.Movement);
                TurnManager.CurrentTurn.Movecount = TurnManager.CurrentTurn.MyOwner.MyProfile.GetStat(ActorStat.StatType.Movement);
                TurnManager.CurrentTurn.Actcount = 1;
            }
            DisableCutsceneMode();
            TutPanel.SetActive(false);
        }
        else if(input == 'y')
        {
            PauseMusic();
            TurnManager.ThisAudio.PlayOneShot(TurnManager.WinClip);
            FakeWinScreen.SetActive(true);
        }
        else if (input == 'D')
        {
        }
        else if(input == 'A')
        {
            ThugCEntrance();
        }
    }

    public IEnumerator LevelOutro()
    {

        TutPanel.SetActive(false);
        var actlist = new List<Act>();
        var morgan = (Morgan)TurnManager.PlayerActors.Find(x => x is Morgan);
        actlist.Add(morgan.WalkModule.ForceWalkAct(new Vector2Int(5, 8)));
        TurnManager.ReceiveSceneAct(actlist);
        Oprahcity.SetActive(true);
        var cv = Oprahcity.GetComponent<CanvasRenderer>();
        cv.SetAlpha(0f);
        PauseMusic();
        TurnManager.ThisAudio.PlayOneShot(TurnManager.WinClip);
        while (cv.GetAlpha() < 1f)
        {
            EnableCutsceneMode();
            cv.SetAlpha(cv.GetAlpha() + 0.63f * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {

        EnableCutsceneMode();
        TutPanel.SetActive(false);
        var cv = Opacity.GetComponent<CanvasRenderer>();
        if(TurnManager.ActorQueue.Exists(x=>x.Name == "Fabio"))
        {
            var dude = TurnManager.ActorQueue.Find(x => x.Name == "Fabio");
            dude.CurrentAffects.Add(new Invulnerability(dude));
            dude.CurrentAffects.Add(new ActionLock(dude));
        }
        while (cv.GetAlpha() >= 0.01f)
        {

            EnableCutsceneMode();
            cv.SetAlpha(cv.GetAlpha() - 0.8f * Time.deltaTime);
            yield return null;
        }
        Destroy(cv.gameObject);
        SendFlag('c');
    }

    public void MoveMorgan()
    {
        TutPanel.SetActive(false);
        TurnManager.SkillsAvailable = false;
        SceneDelegate = AfterMove;
        TurnManager.ScDelegate = SceneDelegate;
        var morgan = (Morgan)TurnManager.PlayerActors.Find(x => x is Morgan);
        TurnManager.ReceiveSceneAct(morgan.WalkModule.ForceWalkAct(new Vector2Int(morgan.CurrentPosition.x, morgan.CurrentPosition.y - 4)));
    }

    public void FacilitateTut()
    {
        SceneDelegate = FaciDel;
        TurnManager.ScDelegate = SceneDelegate;
        var actlist = new List<Act>();
        var list3 = new List<Effect>();
        var tarlist3 = new List<Actor>();
        list3.Add(new Wait(1.3f));
        var ak3 = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list3,
            MyModule = null
        };
        list3[0].ReceiveAct(ak3);
        actlist.Add(ak3);
        TurnManager.ReceiveSceneAct(actlist);
    }

    public void SpawnDoc()
    {
        TutPanel.SetActive(false);
        SceneDelegate = AfterDocSpawn;
        TurnManager.ScDelegate = SceneDelegate;
        var actlist = new List<Act>();
        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        list.Add(new SpawnUnit(TurnManager.MyBrain, Doc, new Vector2Int(5, 1)));
        var ak = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list,
            MyModule = null
        };
        list[0].ReceiveAct(ak);
        actlist.Add(ak);

        var list3 = new List<Effect>();
        var tarlist3 = new List<Actor>();
        list3.Add(new Wait(1.3f));
        var ak3 = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list3,
            MyModule = null
        };
        list3[0].ReceiveAct(ak3);
        actlist.Add(ak3);
        TurnManager.ReceiveSceneAct(actlist);
    }

    public void MorgDash()
    {
        var doc = (Doc)TurnManager.PlayerActors.Find(x => x is Doc);
        doc.Skills.RemoveAt(3);
        doc.Skills.RemoveAt(2);
        doc.Skills.RemoveAt(1);
        TurnManager.SkillsAvailable = true;
        TutPanel.SetActive(false);
        SceneDelegate = AfterMorg;
        TurnManager.ScDelegate = SceneDelegate;
        var actlist = new List<Act>();
        var morgan = (Morgan)TurnManager.PlayerActors.Find(x => x is Morgan);
        actlist.Add(morgan.WalkModule.ForceWalkAct(new Vector2Int(5, 3)));

        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        list.Add(new TakeDamage(morgan,12));
        var ak = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list,
            MyModule = null
        };
        list[0].ReceiveAct(ak);
        actlist.Add(ak);
        TurnManager.ReceiveSceneAct(actlist);
    }

    public void ShowRealPanel()
    {
        TutPanel.SetActive(false);
        StartCoroutine(FlashScreen(FightForRealScreen));
    }

    public void ShowFakePanel()
    {
        TutPanel.SetActive(false);
        SceneDelegate = AfterPanelFlash;
        TurnManager.ScDelegate = SceneDelegate;
        StartCoroutine(FlashScreen(FightScreen));
        var actlist = new List<Act>();
        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        list.Add(new Wait(1.4f));
        var ak = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list,
            MyModule = null
        };
        list[0].ReceiveAct(ak);
        actlist.Add(ak);
        TurnManager.ReceiveSceneAct(actlist);
    }

    public void SpawnOtherDudes()
    {
        TurnManager.UI.DisableUI();
        TutPanel.SetActive(false);
        SceneDelegate = AfterSpawn;
        TurnManager.ScDelegate = SceneDelegate;
        TurnManager.UI.DisableUI();
        var actlist = new List<Act>();
        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        list.Add(new SpawnUnit(TurnManager.MyBrain, ThugF, new Vector2Int(7, 2)));
        var ak = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list,
            MyModule = null
        };
        list[0].ReceiveAct(ak);
        actlist.Add(ak);
        TurnManager.UI.DisableUI();
        var list2 = new List<Effect>();
        var tarlist2 = new List<Actor>();
        list2.Add(new SpawnUnit(TurnManager.MyBrain, ThugG, new Vector2Int(6, 2)));
        var ak2 = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list2,
            MyModule = null
        };
        list2[0].ReceiveAct(ak2);
        actlist.Add(ak2);
        TurnManager.UI.DisableUI();
        var list3 = new List<Effect>();
        var tarlist3 = new List<Actor>();
        list3.Add(new Wait(1.3f));
        var ak3 = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list3,
            MyModule = null
        };
        list3[0].ReceiveAct(ak3);
        actlist.Add(ak3);
        TurnManager.ReceiveSceneAct(actlist);
        TurnManager.UI.DisableUI();
    }

    public void DocDash()
    {
        DialoguePanel.SetActive(false);
        TutPanel.SetActive(false);
        var actlist = new List<Act>();
        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        list.Add(new Wait(0.3f));
        var ak = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list,
            MyModule = null
        };
        list[0].ReceiveAct(ak);
        actlist.Add(ak);
        var doc = (Doc)TurnManager.PlayerActors.Find(x => x is Doc);
        actlist.Add(doc.WalkModule.ForceWalkAct(new Vector2Int(6, 3)));
        TurnManager.ReceiveSceneAct(actlist);
    }



    public void ThugCEntrance()
    {
        SceneDelegate = EnableUIDel;
        TurnManager.ScDelegate = EnableUIDel;
        DialoguePanel.SetActive(false);
        TutPanel.SetActive(false);
        var actlist = new List<Act>();
        var morgan = (Morgan)TurnManager.PlayerActors.Find(x => x is Morgan);
        actlist.Add(morgan.WalkModule.ForceWalkAct(new Vector2Int(5, 7)));

        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        list.Add(new Wait(0.1f));
        var ak = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list,
            MyModule = null
        };
        list[0].ReceiveAct(ak);
        actlist.Add(ak);

        var list2 = new List<Effect>();
        var tarlist2 = new List<Actor>();
        list2.Add(new SpawnUnit(TurnManager.MyBrain,ThugC,new Vector2Int(5,2)));
        var ak2 = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list2,
            MyModule = null
        };
        list2[0].ReceiveAct(ak2);
        actlist.Add(ak2);

        TurnManager.ReceiveSceneAct(actlist);
    }

    public IEnumerator FlashScreen(GameObject targ)
    {
        targ.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        targ.SetActive(false);

    }

    public void AfterMove()
    {
        SendFlag('d');
    }

    public void AfterSpawn()
    {
        SendFlag('C');
    }

    public void AfterPanelFlash()
    {

        var temp = TurnManager.ActorQueue.Find(x => x.Name == "Doc");
        TurnManager.ActorQueue.Remove(temp);
        TurnManager.ActorQueue.Add(temp);
        TurnManager.BeginNextTurn();
        SendFlag('H');
    }//

    public void FaciDel()
    {
        StartCoroutine(TutorialShits());
    }

    public IEnumerator TutorialShits()
    {
        yield return new WaitForSeconds(3f);
        var dude = TurnManager.ActorQueue.Find(x => x.Name == "Doc");
        var morgan = (PlayerActor)dude;
        TutPanel.SetActive(true);
        morgan.NotifyActReady();
    }

    public void AfterDocSpawn()
    {
        SendFlag('U');
    }

    public void AfterMorg()
    {
        SendFlag('F');
    }

    public void EnableUIDel()
    {
        DialoguePanel.SetActive(true);
    }

    public void SecondUIDel()
    {
        DialoguePanel.SetActive(true);
    }

    public void ButtonScript()
    {
        GameObject.FindGameObjectWithTag("sfxx").GetComponent<SFXDude>().PlaySelect();
        FakeWinScreen.SetActive(false);
        SendFlag('A');
    }

}
