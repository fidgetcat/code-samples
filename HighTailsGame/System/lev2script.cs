using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lev2script : LevelScript
{

    public override void Init(Turnstile turnmanager)
    {
        base.Init(turnmanager);
        ObjectiveFlags.Add('x', "Defeat all the thugs!");

        var dl1 = new List<DialogueUnit>();
        dl1.Add(new DialogueUnit("Doc", "Where is he...where is that madman!"));
        dl1.Add(new DialogueUnit("Morgan", "We won't get anywhere panicking. Calm down."));
        dl1.Add(new DialogueUnit("Butch", "I called the police. We just need to make sure he doesn't do anything."));
        dl1.Add(new DialogueUnit("Doc", "Good, the sooner we stop him and his barbaric methods, the better."));
        dl1.Add(new DialogueUnit("Butch", "Yeah, no one's done something like this before. He's crazy and it goes against the rules."));
        dl1.Add(new DialogueUnit("Morgan", "Let's take a deep breath, it'll---"));
        dl1.Add(new DialogueUnit("Butch", "You're not one of us, Morgan. You don't know how bad it is."));
        dl1.Add(new DialogueUnit("Butch", "...I'm sorry. I'm just on edge and someone's trying to fight like a caveman. With actions, not words."));
        dl1.Add(new DialogueUnit("Morgan", "It's fine. I'm no stranger to stress. Treat yourself later."));
        dl1.Add(new DialogueUnit("Corgo", "I believe I'm catching the scent of someone very racist about to do racist things...."));
        dl1.Add(new DialogueUnit("Morgan", "Well, after him, boy! We'll be right behind you!"));
        dl1.Add(new DialogueUnit("Doc", "There he is! STOP RIGHT THERE, SCUM!"));
        dl1.Add(new DialogueUnit("Daddye", "Wild animals. About time you caught my scent. The scent of an actual human. the scent of a Daddye."));
        dl1.Add(new DialogueUnit("Daddye", "You rabid beasts and your mayor are all weak. All mistakes."));
        dl1.Add(new DialogueUnit("Doc", "A weak man!? He's doing this for ALL the residents of this district!"));
        dl1.Add(new DialogueUnit("Daddye", "All? He should be selective."));
        dl1.Add(new DialogueUnit("Daddye", "We'll clean this district up for all of us humans."));
        dl1.Add(new DialogueUnit("Daddye", "When my boss gets his position back as head of this district, everyone's lives will be better!"));
        dl1.Add(new DialogueUnit("Butch", "That's crazy! listen to yourself. You'd poison humans too!"));
        dl1.Add(new DialogueUnit("Daddye", "A CANNIBAL like you calls me crazy? Come back when you stop serving your neighbors on a plate."));
        dl1.Add(new DialogueUnit("Kendrew", "But what if they're right, Boss? What if we're the bad guys here?"));
        dl1.Add(new DialogueUnit("Daddye", "Hush, you! I will not have backtalk you hear? Now help me enlighten these fools."));
        var cl1 = new List<char>();
        cl1.Add('j');
        LevelDialogue.Add(new DialogueSequence(dl1, 'b', cl1));

        var dl2 = new List<DialogueUnit>();
        dl2.Add(new DialogueUnit("Daddye", "How could I lose!? Has society's immune system failed itself!?"));
        dl2.Add(new DialogueUnit("Doc", "No, diversity makes us strong."));
        dl2.Add(new DialogueUnit("Daddye", "Bah! Nonsense and lies! Purity is the only true strength!"));
        dl2.Add(new DialogueUnit("Cops", "I bet the court of law beats purity any day."));
        dl2.Add(new DialogueUnit("Doc", "It's the police! What a relief."));
        dl2.Add(new DialogueUnit("Daddye", "I could have gotten away with this if it weren't for you meddling freaks and your mangy mutt!"));
        dl2.Add(new DialogueUnit("Corgo", "I'll have you know I'm a purebred import and not some stray mutt. I cost more than your bail!"));
        dl2.Add(new DialogueUnit("Cops", "We'll take it from here. good job handling the situation."));
        dl2.Add(new DialogueUnit("Daddye", "My boss will set me free and I'll be back at my job before tomorrow!"));
        dl2.Add(new DialogueUnit("Cops", "Your boss? He can do that? He sounds powerful. What's his name?"));
        dl2.Add(new DialogueUnit("Daddye", "I uhh...it's..."));
        dl2.Add(new DialogueUnit("Cops", "Thanks for helping us get to the root of this. Sadly, we can't let you go."));
        dl2.Add(new DialogueUnit("Butch", "What a nuisance. There's no benefit in what he's trying to do."));
        dl2.Add(new DialogueUnit("Morgan", "Yeah, they don't seem to realize that humans and furries live in the same district."));
        dl2.Add(new DialogueUnit("Butch", "Not everyone wants it. I should get going."));
        dl2.Add(new DialogueUnit("Doc", "I'll stay here and make sure nothing's wrong. If even a drop got in there we may just have to shut the supply for the day."));
        dl2.Add(new DialogueUnit("Morgan", "You can do it, doc. I need to go back home too and get some rest for myself. "));
        dl2.Add(new DialogueUnit("Doc", "Rest well, neighbor. Morgan was it?"));
        dl2.Add(new DialogueUnit("Morgan", "Yeah! Good luck, doc."));

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
        else if (input == 'j')
        {
            var doo = GameObject.FindGameObjectWithTag("mus");
            if (doo != null)
            {
                doo.GetComponent<MusicMan>().Play(doo.GetComponent<MusicMan>().Combat);
            }
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
        if (!ShowTutPanel)
        {
            if (TutPanel.activeSelf)
            {
                TutPanel.SetActive(false);
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
        doc.EnableTutorial = false;
        doc.TutorialSkillUsed = true;

        var dude3 = TurnManager.ActorQueue.Find(x => x.Name == "Butch");
        var butch = (PlayerActor)dude3;
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
        PauseMusic();
        TurnManager.ThisAudio.PlayOneShot(TurnManager.WinClip);
        var cv = Oprahcity.GetComponent<CanvasRenderer>();
        cv.SetAlpha(0f);
        while (cv.GetAlpha() < 1f)
        {
            EnableCutsceneMode();
            cv.SetAlpha(cv.GetAlpha() + 0.63f * Time.deltaTime);
            yield return null;
        }
    }
}
