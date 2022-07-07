using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
    public List<DialogueSequence> LevelDialogue;
    public Dictionary<char, string> ObjectiveFlags;
    public List<char> TriggerFlags;
    public Text DialogueText;
    public Text TutorialText;
    public Text NameText;
    public GameObject DialoguePanel;
    public TacticsHUD HUD;
    public TacticUI UI;
    public Turnstile TurnManager;
    public GameObject Opacity;
    public GameObject Oprahcity;


    public PortraitServer PServer;

    public bool ShowTutPanel = true;

    public Image DialogueAvatar;

    public bool FirstTutorial = true;

    public delegate void ScriptDel();
    public ScriptDel SceneDelegate;

    public GameObject TutPanel;

    public DialogueSequence CurrentSequence;
    public int index;

    public void Start()
    {
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

    //override this
    public virtual void Init(Turnstile turnmanager)
    {
        PServer = GameObject.FindGameObjectWithTag("Finish").GetComponent<PortraitServer>();
        ObjectiveFlags = new Dictionary<char, string>();
        TurnManager = turnmanager;
        HUD = GetComponent<TacticsHUD>();
        UI = GetComponent<TacticUI>();
        index = 0;
        LevelDialogue = new List<DialogueSequence>();
        TriggerFlags = new List<char>();
    }

    public virtual void ReceiveTriggerFlag(char input)
    {
        TriggerFlags.Add(input);
        TurnManager.UpdateObjectives();
        TurnManager.CheckGameEnd();
    }

    //override this too
    public virtual void SendFlag(char input)
    {
        if (TriggerFlags.Contains(input))
        {
            EnableCutsceneMode();
        }
        else if (LevelDialogue.Exists(x => x.TriggerFlag == input))
        {
            EnableCutsceneMode();
        }
        CheckDialogueFlags(input);
    }

    public virtual void NoEnemiesLeft()
    {

    }

    public void PauseMusic()
    {
        var doo = GameObject.FindGameObjectWithTag("mus").GetComponent<MusicMan>();
        doo.Paws();
    }


    public virtual void ReceiveTutorialText(char input)
    {
        if (TutPanel != null)
        {
            if (TutPanel.activeSelf != true && FirstTutorial == true)
            {
                TutPanel.SetActive(true);
            }

            if (TurnManager.CurrentTurn.MyOwner.Name == "Doc" || FirstTutorial == true)
            {
                switch (input)
                {
                    case 'a':
                        TutorialText.text = "After pressing MOVE, remember that you can only move to any highlighted area, human.";
                        break;
                    case 'b':
                        TutorialText.text = "Click on that handy attack button and pick a valid target, human. There's a massive arrow for a reason.";
                        break;
                    case 'c':
                        TutorialText.text = "Great job! That sent her reeling and out of here! Your WIT-based attacks did a number on her AGGRESSIVE nature!";
                        break;
                    case 'd':
                        TutorialText.text = "Good, now just click on the battery and I will conveniently move it out of your way! Out of sight even!";
                        break;
                    case 'e':
                        TutorialText.text = "Corgo: Access your Skills from the Skill button.";
                        break;
                    case 'f':
                        TutorialText.text = "Corgo: Then select \"Doctor's Order\".";
                        break;
                    case 'g':
                        TutorialText.text = "Corgo: Target the human and activate your skill. You have two seconds to comply.";
                        break;
                    case 'h':
                        TutorialText.text = "Corgo: Outstanding. Remember that some skills do not require targets.";
                        break;
                    case 'j':
                        TutorialText.text = "Corgo: Do you detect the bar on the screen? You must press right click when the blue arrow hits the red area for maximum effect.";
                        break;
                    default:
                        TutorialText.text = "";
                        break;
                }
            }
            else if (TutPanel.activeSelf == true)
            {
                TutPanel.SetActive(false);
            }
        }
    }

    public virtual void CheckDialogueFlags(char input)
    {
        if (LevelDialogue.Exists(x => x.TriggerFlag == input))
        {
            TutPanel.SetActive(false);
            DialoguePanel.SetActive(true);
            CurrentSequence = LevelDialogue.Find(x => x.TriggerFlag == input);
            index = 0;
            DialogueText.text = CurrentSequence.DialogueUnits[index].Dialogue;
            NameText.text = CurrentSequence.DialogueUnits[index].Speaker;
            DialogueAvatar.sprite = PServer.GetPortrait(CurrentSequence.DialogueUnits[index].Speaker);
        }
    }

    public void EnableCutsceneMode()
    {
        TurnManager.MCamera.IsBusy = true;
        UI.CutsceneMode = true;
        HUD.CutsceneMode = true;
        UI.PlayerMenu.SetActive(false);
        HUD.MouseOverPanel.SetActive(false);
        TurnManager.ObjectivePanel.SetActive(false);
        TurnManager.TurnOrderPanel.SetActive(false);
        GetComponent<TacticsHUD>().RPSPanel.SetActive(false);
    }

    public void DisableCutsceneMode()
    {
        TurnManager.MCamera.IsBusy = false;
        UI.PlayerMenu.SetActive(true);
        UI.CutsceneMode = false;
        HUD.CutsceneMode = false;
        TurnManager.ObjectivePanel.SetActive(true);
        TurnManager.TurnOrderPanel.SetActive(true);
        TurnManager.UpdateObjectives();
        TurnManager.UpdateTurnOrder();
    }

    public virtual void AdvanceDialogue()
    {
        //  GameObject.FindGameObjectWithTag("sfxx").GetComponent<SFXDude>().PlaySelect();
        if (CurrentSequence != null)
        {
            index++;
            if (index >= CurrentSequence.DialogueUnits.Count)
            {
                index = 0;
                DialoguePanel.SetActive(false);
                TutPanel.SetActive(true);

                if (CurrentSequence.EndFlags.Count > 0)
                {
                    //EnableCutsceneMode();
                    foreach (char c in CurrentSequence.EndFlags)
                    {
                        SendFlag(c);
                        if (ObjectiveFlags.ContainsKey(c))
                            ReceiveTriggerFlag(c);
                    }
                }
                else
                {
                    TurnManager.UI.PlayerMenu.SetActive(true);

                }
                DisableCutsceneMode();
                CurrentSequence = null;
            }
            else
            {
                TutPanel.SetActive(false);
                DialogueText.text = CurrentSequence.DialogueUnits[index].Dialogue;
                NameText.text = CurrentSequence.DialogueUnits[index].Speaker;
                // Actor guy = TurnManager.MyBrain.LevelActors.Find(x => x.Name == CurrentSequence.DialogueUnits[index].Speaker);
                DialogueAvatar.sprite = PServer.GetPortrait(CurrentSequence.DialogueUnits[index].Speaker);

            }
        }
        else
        {
        }
    }

    void Update()
    {
        if (DialoguePanel.activeSelf == true)
        {
            if (TurnManager.UI.PlayerMenu.activeSelf)
            {
                TurnManager.UI.DisableUI();
            }
        }


    }
}

public class DialogueSequence
{
    public List<DialogueUnit> DialogueUnits;
    public char TriggerFlag;
    public List<char> EndFlags;

    public DialogueSequence(List<DialogueUnit> dialogue, char trigger, List<char> end)
    {
        DialogueUnits = dialogue;
        TriggerFlag = trigger;
        EndFlags = end;
    }

}

public class DialogueUnit
{
    public string Speaker;
    public string Dialogue;

    public DialogueUnit(string speaker, string dialogue)
    {
        Speaker = speaker;
        Dialogue = dialogue;
    }
}

