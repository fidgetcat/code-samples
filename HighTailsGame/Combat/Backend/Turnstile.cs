using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Turnstile : MonoBehaviour
{
    public enum LevelObjective { Circuit, Slay, Resistor };
    public List<LevelObjective> ObjectiveList;
    public TacticsManager MyBrain;
    public List<Turn> TurnHistory;

    public List<Act> ActQueue;
    public List<Act> SceneActQueue;
    public List<Act> StateActQueue;
    public List<Act> CleanupQueue;
    public List<Actor> ActorQueue;
    public Turn CurrentTurn;

    public AudioSource ThisAudio;
    public AudioClip WinClip;

    public LevelScript Director;
    public LevelScript.ScriptDel ScDelegate;

    public GameObject SelectionHighlight;

    public GameObject CurrentHighlight;

    public List<Actor> PlayerActors;
    public List<Actor> Morgue;

    public GameObject WinPanel;
    public Text ObjectiveText;

    public CameraController MCamera;
    public GameObject LevelCanvas;

    public string LoadScene;

    public delegate void AcceptAct(Act input);
    public static event AcceptAct OnActAccept;

    public delegate void CheckStates();
    public static event CheckStates OnStateCheck;

    public delegate void TurnStartCheck();
    public static event TurnStartCheck OnTurnStart;

    public float turndelay;

    public List<GameObject> CutinCleanup;
    public GameObject TopCut;
    public GameObject BotCut;

    public GameObject SliderPrefab;
    public float cutspeed;
    public float slidespeed;
    public float skidspeed;
    public GameObject SliderAnchorLeft;
    public GameObject SliderAnchorRight;

    public GameObject TopBubble;
    public GameObject BotBubble;

    public GameObject SkillTextPanel;
    public GameObject SkillText;

    public GameObject TurnOrderPanel;
    public GameObject ParentTurnCutPrefab;
    public GameObject ChildTurnCutPrefab;

    public List<GameObject> TurnOrderList;

    public GameObject ObjectivePanel;

    public TacticUI UI;

    public GameObject LoseScreen;

    public GameObject CamTarget;

    public bool SkillsAvailable;
    public bool IsBusy;

    public List<Vector2Int> FleePoints;
    void Start()
    {
        FleePoints = new List<Vector2Int>();
        CamTarget = GameObject.FindGameObjectWithTag("Player");
        MCamera = CamTarget.GetComponent<CameraController>();
        TurnOrderList = new List<GameObject>();
        SkillsAvailable = true;
        CutinCleanup = new List<GameObject>();
        ThisAudio = GetComponent<AudioSource>();
        Director = GetComponent<LevelScript>();
        Morgue = new List<Actor>();
        MyBrain = GetComponent<TacticsManager>();
        ActorQueue = new List<Actor>();
        TurnHistory = new List<Turn>();
        ActQueue = new List<Act>();
        SceneActQueue = new List<Act>();
        StateActQueue = new List<Act>();
        UI = GetComponent<TacticUI>();
    }

    public void PreInit()
    {
        PlayerActors = new List<Actor>();
    }

    public void UpdateTurnOrder()
    {
        foreach(GameObject obj in TurnOrderList)
        {
            Destroy(obj);
        }
        TurnOrderList = new List<GameObject>();
        if (ActorQueue.Count > 0)
        {
            if (ActorQueue.Count == 1)
            {
                RectTransform rct = Instantiate(ParentTurnCutPrefab, TurnOrderPanel.GetComponent<RectTransform>()).GetComponent<RectTransform>();
                rct.gameObject.GetComponent<Image>().sprite = ActorQueue[0].TurnCutOut;
                TurnOrderList.Add(rct.gameObject);
            }
            else if (ActorQueue.Count == 2)
            {
                RectTransform rct = Instantiate(ParentTurnCutPrefab, TurnOrderPanel.GetComponent<RectTransform>()).GetComponent<RectTransform>();
                rct.gameObject.GetComponent<Image>().sprite = CurrentTurn.MyOwner.TurnCutOut;
                TurnOrderList.Add(rct.gameObject);

                RectTransform rct2 = Instantiate(ChildTurnCutPrefab, TurnOrderPanel.GetComponent<RectTransform>()).GetComponent<RectTransform>();
                rct2.gameObject.GetComponent<Image>().sprite = ActorQueue[ActorQueue.Count-1].TurnCutOut;
                TurnOrderList.Add(rct2.gameObject);
            }
            else
            {
                float lastY = 0f;
                int unitcount = ActorQueue.Count;
                if (unitcount > 4)
                    unitcount = 4;
                unitcount--;

                RectTransform rct = Instantiate(ParentTurnCutPrefab, TurnOrderPanel.GetComponent<RectTransform>()).GetComponent<RectTransform>();
                rct.gameObject.GetComponent<Image>().sprite = CurrentTurn.MyOwner.TurnCutOut;
                TurnOrderList.Add(rct.gameObject);

                RectTransform rct2 = Instantiate(ChildTurnCutPrefab, TurnOrderPanel.GetComponent<RectTransform>()).GetComponent<RectTransform>();
                rct2.gameObject.GetComponent<Image>().sprite = ActorQueue[ActorQueue.Count - 1].TurnCutOut;
                TurnOrderList.Add(rct2.gameObject);
                lastY = rct2.localPosition.y;

                for(int i = ActorQueue.Count-2; unitcount>0; i--)
                {
                    unitcount--;
                    RectTransform rct3 = Instantiate(ChildTurnCutPrefab, TurnOrderPanel.GetComponent<RectTransform>()).GetComponent<RectTransform>();
                    rct3.localPosition = new Vector3(rct3.localPosition.x, lastY - 70.38f, rct3.localPosition.z);
                    rct3.gameObject.GetComponent<Image>().sprite = ActorQueue[i].TurnCutOut;
                    TurnOrderList.Add(rct3.gameObject);
                    lastY = rct3.localPosition.y;
                }
            }
        }
    }

    public void Initialize(List<Actor> inputs)
    {
        CleanupQueue = new List<Act>();
        var leest = new List<Actor>();
        foreach (Actor actor in inputs)
        {
            if (actor.MyProfile.HasStat(ActorStat.StatType.Speed))
            {
                leest.Add(actor);
            }
        }

        ActorQueue = new List<Actor>(leest);

        ActorQueue.Sort(delegate (Actor x, Actor y)
        {
            if (x.MyProfile == null && y.MyProfile == null) return 0;
            else if (x.MyProfile == null) return -1;
            else if (y.MyProfile == null) return 1;
            else return x.MyProfile.MyStats.Find(f => f.Type == ActorStat.StatType.Speed).CompareTo(y.MyProfile.MyStats.Find(k => k.Type == ActorStat.StatType.Speed));
        });
        ActorQueue.Sort(delegate (Actor x, Actor y)
        {
            if (x.MyProfile == null && y.MyProfile == null) return 0;
            else if (x.MyProfile == null) return -1;
            else if (y.MyProfile == null) return 1;
            else return x.MyProfile.MyStats.Find(f => f.Type == ActorStat.StatType.Speed).CompareTo(y.MyProfile.MyStats.Find(k => k.Type == ActorStat.StatType.Speed));
        });
        for (int i = 0; i < ActorQueue.Count; i++)
        {
            //
        }
    }

    public IEnumerator PlayCutIn(Act inact)
    {
        var topcut = Instantiate(TopCut, TopCut.GetComponent<RectTransform>());
        var botcut = Instantiate(BotCut, BotCut.GetComponent<RectTransform>());
        var leftslider = Instantiate(SliderPrefab, SliderAnchorLeft.transform);
        var rightslider = Instantiate(SliderPrefab, SliderAnchorRight.transform);
        rightslider.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);


        RectTransform leftrans = leftslider.GetComponent<RectTransform>();
        RectTransform rightrans = rightslider.GetComponent<RectTransform>();
        rightrans.localPosition = new Vector3(-leftrans.localPosition.x, rightrans.localPosition.y, rightrans.localPosition.z);
        var teee = 0f;

        Vector3 leforigtrans = new Vector3();
        Vector3 ritorigtrans = new Vector3();
        Vector3 leftartrans = new Vector3();
        Vector3 rittartrans = new Vector3();

        if (inact.MyModule.MyActor.ThisType == Actor.UnitType.Organic)
        {
            float fac1 = 0f;
            if(inact.MyModule.MyActor.Name == "Morgan")
            {
                fac1 = 120f;
            }
            else if (inact.MyModule.MyActor.Name == "Butch")
            {
                fac1 = -70f;
            }
            else if (inact.MyModule.MyActor.Name == "Daddye")
            {
                fac1 = 70f;
            }
            leftslider.GetComponent<Image>().sprite = inact.MyModule.MyActor.Slider;
            leftrans = leftslider.GetComponent<RectTransform>();
            leforigtrans = leftrans.localPosition;
            leftartrans = new Vector3(286.5f+fac1, leforigtrans.y, leforigtrans.z);
            if (inact.Targets.Count == 1 && inact.Targets.Exists(x => x == inact.MyModule.MyActor) == false)
            {
                float fac2 = 0f;
                foreach (Effect eff in inact.Effects)
                {
                    if (eff.Targets != null)
                    {
                        if (eff.Targets.Exists(x => x.Name == "Morgan"))
                        {
                            fac2 = 120f;
                        }
                        else if (eff.Targets.Exists(x => x.Name == "Daddye"))
                        {
                            fac2 = 150f;
                        }
                        else if (eff.Targets.Exists(x => x.Name == "Butch"))
                        {
                            fac2 = -70f;
                        }
                    }
                }
                rightslider.GetComponent<Image>().sprite = inact.Targets[0].Slider;
                rightrans = rightslider.GetComponent<RectTransform>();
                ritorigtrans = rightrans.localPosition;
                rittartrans = new Vector3(-286.5f-fac2, ritorigtrans.y, ritorigtrans.z);
            }
            else
            {
                Destroy(rightslider);
            }
        }
        else
        {
            float fac1 = 0f;
            if (inact.MyModule.MyActor.Name == "Morgan")
            {
                fac1 = 120f;
            }
            else if(inact.MyModule.MyActor.Name == "Daddye")
            {
                fac1 = 150f;
            }
            else if (inact.MyModule.MyActor.Name == "Butch")
            {
                fac1 = -70f;
            }
            rightslider.GetComponent<Image>().sprite = inact.MyModule.MyActor.Slider;
            rightrans = rightslider.GetComponent<RectTransform>();
            ritorigtrans = rightrans.localPosition;
            rittartrans = new Vector3(-286.5f-fac1, ritorigtrans.y, ritorigtrans.z);
            if (inact.Targets.Count == 1 && inact.Targets.Exists(x => x == inact.MyModule.MyActor) == false)
            {
                float fac2 = 0f;
                foreach(Effect eff in inact.Effects)
                {
                    if (eff.Targets != null)
                    {
                        if(eff.Targets.Exists(x=> x.Name == "Morgan"))
                        {
                            fac2 = 120f;
                        }
                        else if (eff.Targets.Exists(x => x.Name == "Daddye"))
                        {
                            fac2 = 1500f;
                        }
                        else if (eff.Targets.Exists(x => x.Name == "Butch"))
                        {
                            fac2 = -70f;
                        }
                    }
                }
                leftslider.GetComponent<Image>().sprite = inact.Targets[0].Slider;
                leftrans = leftslider.GetComponent<RectTransform>();
                leforigtrans = leftrans.localPosition;
                leftartrans = new Vector3(286.5f+fac2, leforigtrans.y, leforigtrans.z);
            }
            else
            {
                Destroy(leftslider);
            }
        }
        var toptrans = topcut.GetComponent<RectTransform>();
        var botrans = botcut.GetComponent<RectTransform>();
        var tee = 0f;

        var toporigtrans = toptrans.localPosition;
        var botorigtrans = botrans.localPosition;
        var toptartrans = new Vector3(toporigtrans.x, -410.1f, toporigtrans.z);
        var bottartrans = new Vector3(botorigtrans.x, 410.8f, botorigtrans.z);
        while (tee < 1f)
        {
            tee += cutspeed * Time.deltaTime;
            if (tee > 1f)
                tee = 1f;
            toptrans.localPosition = Vector3.Lerp(toporigtrans, toptartrans, tee);
            botrans.localPosition = Vector3.Lerp(botorigtrans, bottartrans, tee);
            yield return null;
        }
        // botrans.localPosition = bottartrans;
        //toptrans.localPosition = toptartrans;
        while (teee < 1f)
        {
            teee += slidespeed * Time.deltaTime;
            if (teee > 1f)
                teee = 1f;
            if (leftslider != null)
            {
                leftrans.localPosition = Vector3.Lerp(leforigtrans, leftartrans, teee);
            }
            if (rightslider != null)
            {
                rightrans.localPosition = Vector3.Lerp(ritorigtrans, rittartrans, teee);
            }
            yield return null;
        }

        GameObject holder;

        if (inact.MyModule.MyActor.ThisType == Actor.UnitType.Organic)
        {
            GameObject dood = Instantiate(TopBubble, LevelCanvas.transform);
            dood.GetComponent<SbubTextHolder>().InsultText.text = inact.Catchphrase;
            inact.MyModule.MyActor.PlayCritSound();
            holder = dood;
            Vector3 orig = dood.GetComponent<RectTransform>().position;
            float tmer = 0f;
            float otmer = 4f;
            while (tmer < 0.55f)
            {
                tmer += Time.deltaTime * 0.3f;
                otmer += Time.deltaTime * 70f;
                if (leftslider != null)
                {
                    leftrans.localPosition = new Vector3(leftrans.localPosition.x + (skidspeed * Time.deltaTime), leftrans.localPosition.y, leftrans.localPosition.z);
                }
                if (rightslider != null)
                {
                    rightrans.localPosition = new Vector3(rightrans.localPosition.x - (skidspeed * Time.deltaTime), rightrans.localPosition.y, rightrans.localPosition.z);
                }
                if (otmer >= 1f)
                {
                    int negX;
                    int negY;
                    if (Random.Range(0f, 100f) < 50f)
                    {
                        negX = -7;
                    }
                    else
                    {
                        negX = 7;
                    }
                    if (Random.Range(0f, 100f) > 50f)
                    {
                        negY = -7;
                    }
                    else
                    {
                        negY = 7;
                    }

                    float xcoe = Random.Range(20f, 100f) * 0.01f * negX * 0.1f;
                    float ycoe = Random.Range(20f, 100f) * 0.01f * negY * 0.1f;
                    dood.GetComponent<RectTransform>().position = new Vector3(orig.x + xcoe, orig.y + ycoe, orig.z - xcoe);
                    otmer = 0f;
                    yield return null;
                }
                yield return null;
            }
            dood.GetComponent<RectTransform>().position = orig;
        }
        else
        {
            GameObject dood = Instantiate(BotBubble, LevelCanvas.transform);
            holder = dood;
            dood.GetComponent<SbubTextHolder>().InsultText.text = inact.Catchphrase;
            inact.MyModule.MyActor.PlayCritSound();
            Vector3 orig = dood.GetComponent<RectTransform>().position;
            float tmer = 0f;
            float otmer = 4f;
            while (tmer < 0.55f)
            {
                tmer += Time.deltaTime * 0.3f;
                otmer += Time.deltaTime * 70f;
                if (leftslider != null)
                {
                    leftrans.localPosition = new Vector3(leftrans.localPosition.x + (skidspeed * Time.deltaTime), leftrans.localPosition.y, leftrans.localPosition.z);
                }
                if (rightslider != null)
                {
                    rightrans.localPosition = new Vector3(rightrans.localPosition.x - (skidspeed * Time.deltaTime), rightrans.localPosition.y, rightrans.localPosition.z);
                }
                if (otmer >= 1f)
                {
                    int negX;
                    int negY;
                    if (Random.Range(0f, 100f) < 50f)
                    {
                        negX = -7;
                    }
                    else
                    {
                        negX = 7;
                    }
                    if (Random.Range(0f, 100f) > 50f)
                    {
                        negY = -7;
                    }
                    else
                    {
                        negY = 7;
                    }

                    float xcoe = Random.Range(20f, 100f) * 0.01f * negX * 0.1f;
                    float ycoe = Random.Range(20f, 100f) * 0.01f * negY * 0.1f;
                    dood.GetComponent<RectTransform>().position = new Vector3(orig.x + xcoe, orig.y + ycoe, orig.z - xcoe);
                    otmer = 0f;
                    yield return null;
                }
                yield return null;
            }
            dood.GetComponent<RectTransform>().position = orig;
        }
        Destroy(holder);
        Destroy(topcut);
        Destroy(botcut);
        if (leftslider != null)
            Destroy(leftslider);
        if (rightslider != null)
            Destroy(rightslider);
    }

    public void ReceiveAct(Act input)
    {
        ActQueue.Add(input);
        if (OnActAccept != null)
        {
            OnActAccept(input);
        }
    }

    public void ReceiveSceneAct(List<Act> input)
    {
        UI.DisableUI();
        foreach (Act item in input)
        {
            SceneActQueue.Add(item);
        }
        for (int i = 0; i < SceneActQueue.Count; i++)
        {
            for (int x = 0; x < SceneActQueue[i].Effects.Count; x++)
            {
                SceneActQueue[i].Effects[x].ApplyEffect();
            }
        }
        StartCoroutine(ProcessSceneActs());
    }

    public void ReceiveSceneAct(Act input)
    {
        SceneActQueue.Add(input);
        for (int i = 0; i < SceneActQueue.Count; i++)
        {
            for (int x = 0; x < SceneActQueue[i].Effects.Count; x++)
            {
                SceneActQueue[i].Effects[x].ApplyEffect();
            }
        }
        StartCoroutine(ProcessSceneActs());
    }

    public void ReceiveStateAct(Act input)
    {
        StateActQueue.Add(input);
    }

    public void ReceiveCleanupAct(Act input)
    {
        CleanupQueue.Add(input);
    }

    public IEnumerator ProcessActQueues()
    {
        if (ActQueue.Count > 0 && ActQueue != null)
        {
            for (int x = 0; 0 < ActQueue.Count; x++)
            {
                for (int y = 0; 0 < ActQueue[x].Effects.Count; y++)
                {
                    Coroutine z = StartCoroutine(ActQueue[x].Effects[y].ExecuteEffect());
                    yield return z;
                }
            }
        }
        else
        {
            ActQueue = new List<Act>();
        }

        if (StateActQueue.Count > 0 && ActQueue != null)
        {
            for (int x = 0; 0 < StateActQueue.Count; x++)
            {
                for (int y = 0; 0 < StateActQueue[x].Effects.Count; y++)
                {
                    Coroutine z = StartCoroutine(StateActQueue[x].Effects[y].ExecuteEffect());
                    yield return z;
                }
            }
        }
        else
        {
            StateActQueue = new List<Act>();
        }
    }

    public IEnumerator ProcessSceneActs()
    {
        UI.DisableUI();
        IsBusy = true;
        if (SceneActQueue.Count > 0 && SceneActQueue != null)
        {
            for (int x = 0; x < SceneActQueue.Count; x++)
            {
                for (int y = 0; y < SceneActQueue[x].Effects.Count; y++)
                {
                    Coroutine z = StartCoroutine(SceneActQueue[x].Effects[y].ExecuteEffect());
                    yield return z;
                }
            }
            SceneActQueue = new List<Act>();
        }
        else
        {
            SceneActQueue = new List<Act>();
        }

        if (ScDelegate != null)
        {
            ScDelegate();
            ScDelegate = null;
        }
        IsBusy = false;
        UI.PlayerMenu.SetActive(true);
    }

    public IEnumerator ProcessActQueuesEnd()
    {
        MCamera.IsBusy = true;
        if (ActQueue.Count > 0 && ActQueue != null)
        {
            for (int x = 0; x < ActQueue.Count; x++)
            {
                CamTarget.transform.position = new Vector3(CurrentTurn.MyOwner.gameObject.transform.position.x, CamTarget.transform.position.y, CurrentTurn.MyOwner.gameObject.transform.position.z);
                SkillTextPanel.SetActive(true);
                SkillText.GetComponent<Text>().text = ActQueue[x].ActName;
                if (ActQueue[x].IsCriticalCutscene)
                {
                    Coroutine aa = StartCoroutine(PlayCutIn(ActQueue[x]));
                    CamTarget.transform.position = new Vector3(CurrentTurn.MyOwner.gameObject.transform.position.x, CamTarget.transform.position.y, CurrentTurn.MyOwner.gameObject.transform.position.z);
                    yield return aa;
                }
                for (int y = 0; y < ActQueue[x].Effects.Count; y++)
                {
                    Coroutine z = StartCoroutine(ActQueue[x].Effects[y].ExecuteEffect());
                    CamTarget.transform.position = new Vector3(CurrentTurn.MyOwner.gameObject.transform.position.x, CamTarget.transform.position.y, CurrentTurn.MyOwner.gameObject.transform.position.z);
                    yield return z;
                }
                SkillTextPanel.SetActive(false);
            }
            SkillTextPanel.SetActive(false);
        }
        else
        {
            ActQueue = new List<Act>();
        }
        ActQueue = new List<Act>();

        if (StateActQueue.Count > 0 && StateActQueue != null)
        {
            for (int x = 0; x < StateActQueue.Count; x++)
            {
                for (int y = 0; y < StateActQueue[x].Effects.Count; y++)
                {
                    Coroutine z = StartCoroutine(StateActQueue[x].Effects[y].ExecuteEffect());
                    yield return z;
                }
            }
            StateActQueue = new List<Act>();
        }
        else
        {
            StateActQueue = new List<Act>();
        }

        if (CleanupQueue.Count > 0 && CleanupQueue != null)
        {
            for (int x = 0; x < CleanupQueue.Count; x++)
            {
                if (CleanupQueue[x].Effects.Count > 0)
                {
                    //
                    for (int y = 0; y < CleanupQueue[x].Effects.Count; y++)
                    {
                        Coroutine z = StartCoroutine(CleanupQueue[x].Effects[y].ExecuteEffect());
                        yield return z;
                    }
                }
            }
        }
        else
        {
            CleanupQueue = new List<Act>();
        }
        /*   if(Morgue!=null && Morgue.Count > 0)
           {
               foreach(Actor item in Morgue)
               {
                   item.Death();
               }
           }  */
           foreach(Actor guy in ActorQueue)
        {
            guy.StatMan.UpdateStatuses();
        }
        UpdateObjectives();
        yield return new WaitForSeconds(turndelay);
        MCamera.IsBusy = false;
        EndAction();
    }

    void EndGame()
    {
        ThisAudio.PlayOneShot(WinClip);
        WinPanel.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(LoadScene, LoadSceneMode.Single);
    }

    void EndAction()
    {
        StateActQueue = new List<Act>();
        ActQueue = new List<Act>();
        CleanupQueue = new List<Act>();
        //
        foreach (Actor thing in ActorQueue)
        {
        }
        if (ActorQueue.Exists(x => x.ThisType == Actor.UnitType.AI) == false && ActorQueue.Exists(x => x.ThisType == Actor.UnitType.Organic) == true)
        {
            Director.NoEnemiesLeft();
        }
        else if(ActorQueue.Exists(x => x.ThisType == Actor.UnitType.Organic) == false)
        {
            LoseScreen.SetActive(true);
        }

        if (CheckObjectiveCompletion())
        {
            EndGame();
        }
        else if (CurrentTurn.Movecount <= 0 && CurrentTurn.Actcount <= 0)
        {
            EndTurn();
        }
        else
        {
            CurrentTurn.MyOwner.NotifyActReady();
        }
    }

    public void CheckGameEnd()
    {
        if (CheckObjectiveCompletion())
        {
            EndGame();
        }
    }

    bool CheckObjectiveCompletion()
    {
        if (Director.TutPanel != null)
            return Director.ObjectiveFlags.Count == Director.TriggerFlags.Count;
        else
            return false;
    }

    public void UpdateObjectives()
    {
        if (ObjectiveText.gameObject.activeSelf)
        {
            string str = "Objectives:\n";
            foreach (KeyValuePair<char, string> item in Director.ObjectiveFlags)
            {
                if (item.Value.Length > 3)
                {
                    string moop = " [ ]";
                    if (Director.TriggerFlags.Contains(item.Key))
                        moop = " [x]";
                    str += item.Value + moop + "\n";
                }
            }
            ObjectiveText.text = str;
        }
    }

    void EndTurn()
    {
        CurrentTurn.MyOwner.NotifyTurnEnd();
        BeginNextTurn();
    }

    public void ReceiveTurnAct(Act input)
    {
        ActQueue = new List<Act>();
        StateActQueue = new List<Act>();
        ReceiveAct(input);
        for (int i = 0; i < ActQueue.Count; i++)
        {
            for (int x = 0; x < ActQueue[i].Effects.Count; x++)
            {
                ActQueue[i].Effects[x].ApplyEffect();
            }
        }
        StateCheckCall();
        StartCoroutine(ProcessActQueuesEnd());
    }

    void StateCheckCall() //don't call this without purging the queue
    {
        if (OnStateCheck != null)
            OnStateCheck();
        for (int i = 0; i < StateActQueue.Count; i++)
        {
            for (int x = 0; x < StateActQueue[i].Effects.Count; x++)
            {
                StateActQueue[i].Effects[x].ApplyEffect();
            }
        }
    }

    public Vector2Int ReturnRandomFleePoint()
    {
        var ting = Random.Range(0f, 100f);
        if (ting >= 0f && ting <= 30f)
        {
            return FleePoints[0];
        }
        else if (ting >= 30f && ting <= 60f)
        {
            return FleePoints[1];
        }
        else
        {
            return FleePoints[2];
        }
    }

    public void BeginNextTurn()
    {
        if (CurrentTurn != null)
        {
            TurnHistory.Add(CurrentTurn);
            CurrentTurn.MyOwner.NotifyTurnEnd();
        }
        var temp = ActorQueue[ActorQueue.Count - 1];
        ActorQueue.RemoveAt(ActorQueue.Count - 1);
        //
        CurrentTurn = new Turn(temp, this);
        ActorQueue.Insert(0, temp);
        CurrentTurn.MyOwner.CurrentTurn = CurrentTurn;
        if (OnTurnStart != null)
            OnTurnStart();
        CurrentTurn.MyOwner.NotifyTurnBegin();
        CamTarget.transform.position = new Vector3(CurrentTurn.MyOwner.gameObject.transform.position.x, CamTarget.transform.position.y, CurrentTurn.MyOwner.gameObject.transform.position.z);
        UpdateTurnOrder();
        if (CurrentHighlight != null)
            Destroy(CurrentHighlight);
        CurrentHighlight = GameObject.Instantiate(SelectionHighlight, CurrentTurn.MyOwner.transform);
    }

    public void BeginGame()
    {
        //apply all passive affects
        List<Actor> PassiveUsers = MyBrain.LevelActors.FindAll(x => x.MyModules.Exists(y => y.MyClass == Module.ModuleClass.Passive || y.MyClass == Module.ModuleClass.Mixed));
        foreach (Actor item in PassiveUsers)
        {
            List<Module> temp = item.MyModules.FindAll(x => x.MyClass == Module.ModuleClass.Passive || x.MyClass == Module.ModuleClass.Mixed);
            foreach (Module mod in temp)
            {
                mod.InitApply();
            }
        }
        foreach (TileStack thing in MyBrain.Level.ThisLevel.MyGrid)
        {
            if (thing.IsSpawn)
                FleePoints.Add(new Vector2Int(thing.X, thing.Y));

        }
        StateCheckCall();
        StartCoroutine(ProcessActQueues());
        Director.Init(this);
        UpdateObjectives();
        Director.SendFlag('a');
        BeginNextTurn();
        if (Director.TutPanel != null)
            Director.TutPanel.SetActive(false);

    }

    public void RemoveActor(Actor actor)
    {
        ActorQueue.Remove(actor);
        if (CurrentTurn.MyOwner == actor)
        {
            Destroy(actor.gameObject);
            BeginNextTurn();
        }
        if (actor != null)
        {
            Destroy(actor.gameObject);
        }
        UpdateTurnOrder();
    }

    public void AddNewActor(Actor actor)
    {
        if (actor.MyProfile.HasStat(ActorStat.StatType.Speed))
            ActorQueue.Insert(1, actor);

        List<Module> temp = actor.MyModules.FindAll(x => x.MyClass == Module.ModuleClass.Passive || x.MyClass == Module.ModuleClass.Mixed);
        if (temp.Count > 0)
        {
            foreach (Module mod in temp)
            {
                mod.InitApply();
            }
        }
        StateCheckCall();
        UpdateTurnOrder();
        //todo update turn queue visual
    }

    void Update()
    {
        if (UI.CutsceneMode)
        {
            CamTarget.transform.position = new Vector3(CurrentTurn.MyOwner.gameObject.transform.position.x, CamTarget.transform.position.y, CurrentTurn.MyOwner.gameObject.transform.position.z);
        }
    }

}
