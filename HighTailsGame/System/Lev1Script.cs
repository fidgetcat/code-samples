using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lev1Script : LevelScript
{

    public override void Init(Turnstile turnmanager)
    {
        base.Init(turnmanager);
        ObjectiveFlags.Add('x', "Defeat all the thugs!");

        var dl1 = new List<DialogueUnit>();
        dl1.Add(new DialogueUnit("Morgan", "This market by the Mayor was a good idea! Everything's affordable."));
        dl1.Add(new DialogueUnit("Butch", "Don't forget that you're putting honest people to work. "));
        dl1.Add(new DialogueUnit("Corgo", "Human. Don't look now, but... I think I found some familiar faces..."));
        dl1.Add(new DialogueUnit("Morgan", "Well how about we pay them a visit?"));
        dl1.Add(new DialogueUnit("Butch", "If it's trouble, I'll tag along."));
        dl1.Add(new DialogueUnit("Cortni", "Alright, everyone know the plan?"));
        dl1.Add(new DialogueUnit("Thugs", "Yes! We'll show these freaks they ain't welcome. THE MARKET IS A MISTAKE."));
        dl1.Add(new DialogueUnit("Kendrew", "We'll start with this rat. Looks like the one from that other night."));
        dl1.Add(new DialogueUnit("Doc", "Eeeep! I'm brittle!"));
        dl1.Add(new DialogueUnit("Morgan", "Oh! Well if it isn't our old friends from the back alley."));
        dl1.Add(new DialogueUnit("Fabio", "Oh no! It's that girl again!"));
        dl1.Add(new DialogueUnit("Morgan", "Perhaps we should discuss the next part of the plan, Fabio? Like stopping it. "));
        var cl1 = new List<char>();
        cl1.Add('j');
        LevelDialogue.Add(new DialogueSequence(dl1, 'b', cl1));

        var dl2 = new List<DialogueUnit>();
        dl2.Add(new DialogueUnit("Kendrew", "My lungs! I can't, not anymore. I can't laugh. It's too much!"));
        dl2.Add(new DialogueUnit("Morgan", "Tell me where you thugs hang out and I'll let you go."));
        dl2.Add(new DialogueUnit("Kendrew", "I can't! They'll...they'll TALK MY EAR OFF!"));
        dl2.Add(new DialogueUnit("Morgan", "If that's what you want. Butch, yell his ears off."));
        dl2.Add(new DialogueUnit("Doc", "Morgan stop! He's already scared. What if his condition worsens?"));
        dl2.Add(new DialogueUnit("Kendrew", "Oh thank you, ferret doctor. I'll tell you what I know. We were just distractions for the bigger plan."));
        dl2.Add(new DialogueUnit("Morgan", "A plan is it? What's this plan then?"));
        dl2.Add(new DialogueUnit("Kendrew", "Daddye plans to do something to the water fountain of the market."));
        dl2.Add(new DialogueUnit("Doc", "We have to stop them. This affects the ENTIRE market! HOW BARBARIC CAN HE GET!?"));
        dl2.Add(new DialogueUnit("Butch", "Yeah! If they do anything bad to it, it'll cripple a lot of us! Let's hustle!"));

        var cl2 = new List<char>();
        cl2.Add('Z');
        LevelDialogue.Add(new DialogueSequence(dl2, 'X', cl2));
    }

    public override void NoEnemiesLeft()
    {
        base.NoEnemiesLeft();
        SendFlag('X');
    }

    public override void SendFlag(char input)
    {
        base.SendFlag(input);
        if (input == 'a')
        {
            StartCoroutine(FadeIn());
        }
        else if (input == 'Z')
        {
            StartCoroutine(LevelOutro());
        }
        else if(input == 'j')
        {
            var doo = GameObject.FindGameObjectWithTag("mus");
            if(doo!=null)
            doo.GetComponent<MusicMan>().Play(doo.GetComponent<MusicMan>().Combat);
        }
    }

    public new void Start()
    {
        ShowTutPanel = false;
        ReferenceHolderScript refi = GameObject.FindGameObjectWithTag("refh").GetComponent<ReferenceHolderScript>();
        TutPanel = refi.PromptPanel;
        DialogueAvatar = refi.DialogueAvatar;
        Opacity = refi.BlackScreen;
        Oprahcity = refi.WinScreen;
        DialoguePanel = refi.DialoguePanel;
        NameText = refi.NameText;
        TutorialText = refi.TutorialText;
        DialogueText = refi.DialogueText;
    }


    void Update()
    {
        if (TutPanel != null)
        {
            if (!ShowTutPanel)
            {
                if (TutPanel.activeSelf)
                {
                    TutPanel.SetActive(false);
                }
            }
        }
    }

    public IEnumerator FadeIn()
    {
        var dude = TurnManager.ActorQueue.Find(x => x.Name == "Morgan");
        var morgan = (PlayerActor)dude;
        morgan.EnableTutorial = false;

        var dude2 = TurnManager.ActorQueue.Find(x => x.Name == "Doc");
        var doc = (Doc)dude2;
        if (doc != null)
        {
            doc.EnableTutorial = false;
            doc.TutorialSkillUsed = true;
        }
        var dude3 = TurnManager.ActorQueue.Find(x => x.Name == "Butch");
        var butch = (PlayerActor)dude3;
        if(butch!=null)
        butch.EnableTutorial = false;

        EnableCutsceneMode();
        TutPanel.SetActive(false);
        var cv = Opacity.GetComponent<CanvasRenderer>();
        while (cv.GetAlpha() >= 0.01f)
        {
            EnableCutsceneMode();
            cv.SetAlpha(cv.GetAlpha() - 0.8f * Time.deltaTime);
            yield return null;
        }
        Destroy(cv.gameObject);
        SendFlag('b');
    }

    public IEnumerator LevelOutro()
    {

        TutPanel.SetActive(false);
        var actlist = new List<Act>();
        Oprahcity.SetActive(true);
        var cv = Oprahcity.GetComponent<CanvasRenderer>();
        PauseMusic();
        TurnManager.ThisAudio.PlayOneShot(TurnManager.WinClip);
        cv.SetAlpha(0f);
        while (cv.GetAlpha() < 1f)
        {
            EnableCutsceneMode();
            cv.SetAlpha(cv.GetAlpha() + 0.63f * Time.deltaTime);
            yield return null;
        }
    }
}
