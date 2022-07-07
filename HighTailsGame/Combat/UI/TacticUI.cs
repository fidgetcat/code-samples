using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TacticUI : MonoBehaviour
{
    public enum SelectionMode { Movement, Attack, Skill, Special };
    public SelectionMode CurrentMode;
    public bool InSelection;
    public bool CutsceneMode;

    public TacticsManager GameManager;
    public SliderMinigame SliderManager;

    public Vector2Int curvec;
    public int CurrentSkill;
    public List<GameObject> temptiles;
    public List<GameObject> RangeTiles;
    public List<GameObject> Areatiles;
    public GameObject TileHighlight; //higlight prefab
    public GameObject AttackHighlight; //higlight prefab
    public GameObject AllyHighlight; //higlight prefab

    public GameObject AllyTargetHighlight;
    public GameObject TargetHighlight; // target prefab
    public GameObject atkWOWHighlight;
    public Vector2Int CurrentPoint;
    public List<Vector2Int> CurrentArea;
    public Actor CurrentTargetActor;
    public List<Vector2Int> CurrentTargetArea;

    public Text moveinfotext;
    public GameObject PlayerMenu;
    public GameObject SkillMenu;
    public AudioSource MyAudio;
    public AudioClip Error;
    public AudioClip Select;
    public AudioClip Action;

    public GameObject SP1;
    public GameObject SP2;
    public GameObject SP3;
    public GameObject SP4;
    public Text ST1;
    public Text ST2;
    public Text ST3;
    public Text ST4;

    public PlayerActor CurrentActor;
    public GameObject ErrorPanel;
    public GameObject SkillDescPanel;

    public void ReadyAction(string mode)
    {
        if (!GetComponent<Turnstile>().IsBusy)
        {
            MyAudio.PlayOneShot(Select);
            CloseSkillWindow();
            if (CurrentActor.Name != "Doc")
            {
                if (!CurrentActor.CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Lock))
                {
                    CloseSkillWindow();
                    InSelection = true;
                    int range = 0;
                    switch (mode)
                    {
                        case "move":
                            CurrentMode = SelectionMode.Movement;
                            range = CurrentActor.TurnMoveCount;
                            HighlightTiles(GameManager.GetPlayerWalkTiles(CurrentActor.CurrentPosition, CurrentActor.TurnMoveCount));
                            break;
                        case "attack":
                             if (CurrentActor.CurrentTurn.Actcount > 0)
                              {
                            CurrentMode = SelectionMode.Attack;
                            range = CurrentActor.MyProfile.GetStat(ActorStat.StatType.Range);
                            HighlightTiles(GameManager.GetValidTiles(CurrentActor.CurrentPosition, CurrentActor.MyProfile.GetStat(ActorStat.StatType.Range), true, true));
                             }
                              else
                               {
                                 MyAudio.PlayOneShot(Error);
                              }
                            break;
                        case "skill":
                               if (CurrentActor.CurrentTurn.Actcount > 0)
                               {
                            CurrentMode = SelectionMode.Skill;
                            range = CurrentActor.Skills[CurrentSkill].Range;
                            if (range > 0)
                                HighlightTiles(GameManager.GetValidTiles(CurrentActor.CurrentPosition, range, true, true));
                            else if (CurrentActor.Skills[CurrentSkill].MyTargetType == Skill.TargetType.Area)
                            {
                                if (CurrentActor.Skills[CurrentSkill].VerifyTargetArea(CurrentActor.Skills[CurrentSkill].ProvideTargetArea(CurrentActor.CurrentPosition)))
                                {
                                    CurrentArea = CurrentActor.Skills[CurrentSkill].ProvideTargetArea(CurrentActor.CurrentPosition);
                                    BeginMinigame();
                                }
                                else
                                {
                                    MyAudio.PlayOneShot(Error);
                                }
                            }
                            else
                            {
                                CurrentPoint = CurrentActor.CurrentPosition;
                                BeginMinigame();
                            }
                            if (CurrentActor.EnableTutorial == true && CurrentActor.Name == "Doc")
                            {
                                GameManager.TurnManager.Director.ReceiveTutorialText('g');
                            }
                              }
                               else
                              {
                                  MyAudio.PlayOneShot(Error);
                              }
                            break;
                        default:
                            CurrentMode = SelectionMode.Special;
                            break;
                    }
                }
                else
                {
                    MyAudio.PlayOneShot(Error);
                }
            }
            else
            {
                int range = 0;
                switch (mode)
                {
                    case "move":
                        if (!CurrentActor.CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Lock))
                        {
                            CloseSkillWindow();
                            InSelection = true;
                            CurrentMode = SelectionMode.Movement;
                            range = CurrentActor.TurnMoveCount;
                            HighlightTiles(GameManager.GetPlayerWalkTiles(CurrentActor.CurrentPosition, CurrentActor.TurnMoveCount));
                        }
                        break;
                    case "attack":
                        if (!CurrentActor.CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Lock))
                        {
                            if (CurrentActor.CurrentTurn.Actcount > 0)
                            {
                                CloseSkillWindow();
                                InSelection = true;
                                CurrentMode = SelectionMode.Attack;
                                range = CurrentActor.MyProfile.GetStat(ActorStat.StatType.Range);
                                HighlightTiles(GameManager.GetValidTiles(CurrentActor.CurrentPosition, CurrentActor.MyProfile.GetStat(ActorStat.StatType.Range), true, true));
                            }
                            else
                            {
                                MyAudio.PlayOneShot(Error);
                            }
                        }
                        break;
                    case "skill":
                        if (CurrentActor.CurrentTurn.Actcount > 0)
                        {
                            CloseSkillWindow();
                            InSelection = true;
                            CurrentMode = SelectionMode.Skill;
                            range = CurrentActor.Skills[CurrentSkill].Range;
                            HighlightTiles(GameManager.GetValidTiles(CurrentActor.CurrentPosition, range, true, true));
                            if (CurrentActor.EnableTutorial == true && CurrentActor.Name == "Doc")
                            {
                                GameManager.TurnManager.Director.ReceiveTutorialText('g');
                            }
                        }
                        else
                        {
                            MyAudio.PlayOneShot(Error);
                        }
                        break;
                    default:
                        CurrentMode = SelectionMode.Special;
                        break;
                }
            }
        }
    }

    public void PrimeSkill(int input)
    {
        CurrentSkill = input - 1;
        ReadyAction("skill");
        CloseSkillWindow();
    }

    public void CancelAction()
    {
        ClearTiles();
        InSelection = false;
        CloseSkillWindow();
    }

    public void HighlightTiles(List<Vector2Int> input)
    {
        ClearTiles();
        GameObject tiles = null;
        if(CurrentMode == SelectionMode.Attack)
        {
            tiles = AttackHighlight;
        }
        else if (CurrentMode == SelectionMode.Movement)
        {
            tiles = TileHighlight;
        }
        else if(CurrentMode == SelectionMode.Skill)
        {
            if (CurrentActor.Skills[CurrentSkill].AllyOnly)
            {
                tiles = AllyHighlight;
            }
            else
            {
                tiles = AttackHighlight;
            }
        }
        else
        {
            tiles = TileHighlight;
        }
        for (int i = 0; i < input.Count; i++)
        {
            var ting = new Vector2(input[i].x, input[i].y);
            if (GameManager.Level.ThisLevelReference.MyGrid[input[i].x, input[i].y].Blocks != null)
            {
                temptiles.Add(Instantiate(tiles, GameManager.Level.ThisLevelReference.MyGrid[input[i].x, input[i].y].GetTopBlockTransform().position, Quaternion.identity));
                RangeTiles.Add(GameManager.Level.ThisLevelReference.MyGrid[input[i].x, input[i].y].Blocks[0]);
            }
        }
    }

    public void HighlightArea(List<Vector2Int> input, Vector2Int origin)
    {
        GameObject tyls = null;
        if (CurrentMode == SelectionMode.Attack)
        {
            tyls = atkWOWHighlight;
        }
        else if (CurrentMode == SelectionMode.Movement)
        {
            tyls = TargetHighlight;
        }
        else if (CurrentMode == SelectionMode.Skill)
        {
            if (CurrentActor.Skills[CurrentSkill].AllyOnly)
            {
                tyls = AllyTargetHighlight;
            }
            else
            {
                tyls = atkWOWHighlight;
            }
        }
        List<TilePosHolder> lis = new List<TilePosHolder>();
        foreach (GameObject thing in RangeTiles)
        {
            lis.Add(thing.GetComponent<TilePosHolder>());
        }
        if (lis.Exists(x => x.pos == origin))
        {
            if (Areatiles.Count > 0)
            {
                foreach (GameObject tile in Areatiles)
                {
                    Destroy(tile);
                }
                Areatiles = new List<GameObject>();
            }
            foreach (Vector2Int item in input)
            {
                Areatiles.Add(Instantiate(tyls, GameManager.Level.ThisLevelReference.MyGrid[item.x, item.y].GetTopBlockTransform().position, Quaternion.identity));
            }
        }
        else
        {
            if (Areatiles.Count > 0)
            {
                foreach (GameObject tile in Areatiles)
                {
                    Destroy(tile);
                }
                Areatiles = new List<GameObject>();
            }
        }
    }

    public void ClearTiles()
    {
        if (temptiles.Count > 0)
        {
            foreach (GameObject reference in temptiles)
            {
                Destroy(reference);
            }
            temptiles = new List<GameObject>();
        }
        if (Areatiles.Count > 0)
        {
            foreach (GameObject reference in Areatiles)
            {
                Destroy(reference);
            }
            Areatiles = new List<GameObject>();
        }
        RangeTiles = new List<GameObject>();
    }

    public void EndTurn()
    {
        if (!GetComponent<Turnstile>().IsBusy)
        {
            if (!CurrentActor.CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Stun))
            {
                if (!CurrentActor.CurrentAffects.Exists(x => x.ThisAffectType == Affect.AffectType.Lock))
                {
                    CurrentActor.GetEndTurnAction();
                    EndTurnCleanup();
                }
                else
                {
                    MyAudio.PlayOneShot(Error);
                }
            }
            else
            {
                MyAudio.PlayOneShot(Error);
            }
        }
    }

    public void EndTurnCleanup()
    {
        CurrentActor = null;
        CancelAction();
        CloseSkillWindow();
        PlayerMenu.SetActive(false);
        ClearTiles();
        InSelection = false;
    }

    public bool SendAtkToActor(Vector2Int dir)
    {
        var list = GameManager.Level.ActorReferences[dir.x, dir.y];
        List<Actor> alist = new List<Actor>();
        foreach (GameObject thing in list)
        {
            alist.Add(thing.GetComponent<Actor>());
        }
        if (alist.Any(z => z.ThisType == Actor.UnitType.AI))
        {
            return CurrentActor.GetAttackAction(dir);
        }
        else
        {
            return false;
        }
    }


    Vector2Int CheckMouseHover(RaycastHit input)
    {
        if (input.collider.gameObject.CompareTag("block"))
        {
            return new Vector2Int((int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.x, (int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.y);
        }
        else
            return new Vector2Int(999, 999);
    }

    public void MouseOverTarget()
    {

    }

    public void SendMoveToActor(Vector2Int input)
    {
        CurrentActor.GetWalkAction(input);
    }

    public void AttemptTargetSubmission(RaycastHit input)
    {
        switch (CurrentMode)
        {
            case SelectionMode.Attack:
                var basedir = new Vector2Int((int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.x, (int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.y);
                List<TilePosHolder> lis = new List<TilePosHolder>();
                foreach (GameObject thing in RangeTiles)
                {
                    lis.Add(thing.GetComponent<TilePosHolder>());
                }
                if (lis.Exists(x => x.pos == basedir))
                {
                    if (SendAtkToActor(basedir))
                    {
                        DisableUI();
                        MyAudio.PlayOneShot(Action);
                    }
                    else
                    {
                        MyAudio.PlayOneShot(Error);
                    }
                }
                break;
            case SelectionMode.Movement:
                if (RangeTiles.Contains(input.collider.gameObject))
                {
                    MyAudio.PlayOneShot(Action);
                    SendMoveToActor(new Vector2Int((int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.x, (int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.y));
                    DisableUI();
                }
                else
                {
                    MyAudio.PlayOneShot(Error);
                }
                break;
            case SelectionMode.Skill:
                var curskil = CurrentActor.Skills[CurrentSkill];
                if (curskil.MyTargetType == Skill.TargetType.Point || curskil.MyTargetType == Skill.TargetType.Actor)
                {
                    var baseloc = new Vector2Int((int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.x, (int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.y);
                    List<TilePosHolder> list = new List<TilePosHolder>();
                    foreach (GameObject thing in RangeTiles)
                    {
                        list.Add(thing.GetComponent<TilePosHolder>());
                    }
                    if (list.Exists(x => x.pos == baseloc))
                    {
                        if (curskil.AllyOnly)
                        {
                            if (GameManager.Level.ActorReferences[baseloc.x, baseloc.y].Count > 0)
                            {
                                if (GameManager.Level.ActorReferences[baseloc.x, baseloc.y][0].GetComponent<Actor>().ThisType == Actor.UnitType.Organic)
                                {
                                    MyAudio.PlayOneShot(Action);
                                    CurrentPoint = baseloc;
                                    BeginMinigame();
                                }
                            }
                            else
                            {
                                MyAudio.PlayOneShot(Error);
                            }
                        }
                        else if (curskil.EnemyOnly)
                        {
                            if (GameManager.Level.ActorReferences[baseloc.x, baseloc.y].Count > 0)
                            {
                                if (GameManager.Level.ActorReferences[baseloc.x, baseloc.y][0].GetComponent<Actor>().ThisType == Actor.UnitType.AI)
                                {
                                    MyAudio.PlayOneShot(Action);
                                    CurrentPoint = baseloc;
                                    BeginMinigame();
                                }
                            }
                            else
                            {
                                MyAudio.PlayOneShot(Error);
                            }
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    var baseloc = new Vector2Int((int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.x, (int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.y);
                    if (curskil.VerifyTargetArea(curskil.ProvideTargetArea(baseloc)))
                    {
                        CurrentArea = curskil.ProvideTargetArea(baseloc);
                        MyAudio.PlayOneShot(Action);
                        BeginMinigame();
                    }
                    else
                    {
                        MyAudio.PlayOneShot(Error);
                    }
                }
                break;
            default:
                break;
        }
    }

    public void BeginMinigame()
    {
        DisableUI();
        SliderManager.SpawnSlider();
        if (CurrentActor.EnableTutorial == true && CurrentActor.Name == "Doc")
        {
            GameManager.TurnManager.Director.ReceiveTutorialText('h');
            StartCoroutine(ChangeTextAfterDelay());
        }

    }

    public IEnumerator ChangeTextAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        GameManager.TurnManager.Director.ReceiveTutorialText('j');
    }

    public void ExecuteSkillWithFactor(float factor)
    {
        switch (CurrentActor.Skills[CurrentSkill].MyTargetType)
        {
            case Skill.TargetType.Actor:
                var dude = GameManager.LevelActors.Find(x => x.CurrentPosition == CurrentPoint);
                CurrentActor.Skills[CurrentSkill].UseSkill(dude, factor);
                break;
            case Skill.TargetType.Point:
                CurrentActor.Skills[CurrentSkill].UseSkill(CurrentPoint, factor);
                break;
            case Skill.TargetType.Area:
                CurrentActor.Skills[CurrentSkill].UseSkill(CurrentArea, factor);
                break;
        }
    }

    public void DisableUI()
    {
        ClearTiles();
        InSelection = false;
        CloseSkillWindow();
        PlayerMenu.SetActive(false);
    }

    void Update()
    {
        if (CurrentActor != null && CutsceneMode == false)
        {
            if (CurrentActor.CurrentTurn != null)
            {
                if (PlayerMenu.activeSelf)
                {
                    moveinfotext.text = "Moves: " + CurrentActor.CurrentTurn.Movecount.ToString() + " /// " + "Actions: " + CurrentActor.CurrentTurn.Actcount.ToString();
                }
            }
        }

        if (InSelection == true && EventSystem.current.IsPointerOverGameObject() == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                curvec = CheckMouseHover(hit);
                if (curvec.x < 999 && curvec.y < 999)
                {
                    if (CurrentMode == SelectionMode.Skill)
                    {
                        if (CurrentActor.Skills[CurrentSkill].Range > 0)
                        {
                            if (CurrentActor.Skills[CurrentSkill].MyTargetType == Skill.TargetType.Actor || CurrentActor.Skills[CurrentSkill].MyTargetType == Skill.TargetType.Point)
                            {
                                var tt = new List<Vector2Int>();
                                tt.Add(curvec);
                                HighlightArea(tt, curvec);
                            }
                            else
                            {
                                HighlightArea(CurrentActor.Skills[CurrentSkill].ProvideTargetArea(curvec), curvec);
                            }
                        }
                    }
                    else
                    {
                        var tt = new List<Vector2Int>();
                        tt.Add(curvec);
                        HighlightArea(tt, curvec);
                    }
                    MouseOverTarget();
                    if (Input.GetMouseButtonDown(0))
                    {
                        AttemptTargetSubmission(hit);
                    }
                }
                else
                {
                    if (Areatiles.Count > 0)
                    {
                        foreach (GameObject reference in Areatiles)
                        {
                            Destroy(reference);
                        }
                        Areatiles = new List<GameObject>();
                    }
                }
            }
            else
            {
                if (Areatiles.Count > 0)
                {
                    foreach (GameObject reference in Areatiles)
                    {
                        Destroy(reference);
                    }
                    Areatiles = new List<GameObject>();
                }
            }
        }
        else if (InSelection == false)
        {
            ClearTiles();
        }
    }








    void Start()
    {
        CurrentArea = new List<Vector2Int>();
        curvec = new Vector2Int();
        InSelection = false;
        Areatiles = new List<GameObject>();
        MyAudio = GetComponent<AudioSource>();
        temptiles = new List<GameObject>();
        RangeTiles = new List<GameObject>();
        SliderManager = GetComponent<SliderMinigame>();
        GameManager = GetComponent<TacticsManager>();
    }



    public void SkillTest()
    {
        SliderManager.SpawnSlider();
    }


    public void SkillSetup()
    {
        if (SkillMenu.activeSelf == true)
        {
            if (CurrentActor.Skills.Count > 0)
            {
                switch (CurrentActor.Skills.Count)
                {
                    case 1:
                        SP1.SetActive(true);
                        SP2.SetActive(false);
                        SP3.SetActive(false);
                        SP4.SetActive(false);
                        ST1.gameObject.SetActive(true);
                        ST2.gameObject.SetActive(false);
                        ST3.gameObject.SetActive(false);
                        ST4.gameObject.SetActive(false);
                        break;
                    case 2:
                        SP1.SetActive(true);
                        SP2.SetActive(true);
                        SP3.SetActive(false);
                        SP4.SetActive(false);
                        ST1.gameObject.SetActive(true);
                        ST2.gameObject.SetActive(true);
                        ST3.gameObject.SetActive(false);
                        ST4.gameObject.SetActive(false);
                        break;
                    case 3:
                        SP1.SetActive(true);
                        SP2.SetActive(true);
                        SP3.SetActive(true);
                        SP4.SetActive(false);
                        ST1.gameObject.SetActive(true);
                        ST2.gameObject.SetActive(true);
                        ST3.gameObject.SetActive(true);
                        ST4.gameObject.SetActive(false);
                        break;
                    case 4:
                        SP1.SetActive(true);
                        SP2.SetActive(true);
                        SP3.SetActive(true);
                        SP4.SetActive(true);
                        ST1.gameObject.SetActive(true);
                        ST2.gameObject.SetActive(true);
                        ST3.gameObject.SetActive(true);
                        ST4.gameObject.SetActive(true);
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < CurrentActor.Skills.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            SP1.GetComponentInChildren<Text>().text = CurrentActor.Skills[i].Name;
                            if (CurrentActor.Skills[i].CurrentCooldown > 0)
                            {
                                SP1.SetActive(false);
                                ST1.text = CurrentActor.Skills[i].CurrentCooldown.ToString() + " turns";
                            }
                            else
                            {
                                SP1.SetActive(true);
                                ST1.gameObject.SetActive(false);
                            }
                            break;
                        case 1:
                            SP2.GetComponentInChildren<Text>().text = CurrentActor.Skills[i].Name;
                            if (CurrentActor.Skills[i].CurrentCooldown > 0)
                            {
                                SP2.SetActive(false);
                                ST2.text = CurrentActor.Skills[i].CurrentCooldown.ToString() + " turns";
                            }
                            else
                            {
                                SP2.SetActive(true);
                                ST2.gameObject.SetActive(false);
                            }
                            break;
                        case 2:
                            SP3.GetComponentInChildren<Text>().text = CurrentActor.Skills[i].Name;
                            if (CurrentActor.Skills[i].CurrentCooldown > 0)
                            {
                                SP3.SetActive(false);
                                ST3.text = CurrentActor.Skills[i].CurrentCooldown.ToString() + " turns";
                            }
                            else
                            {
                                SP3.SetActive(true);
                                ST3.gameObject.SetActive(false);
                            }
                            break;
                        case 3:
                            SP4.GetComponentInChildren<Text>().text = CurrentActor.Skills[i].Name;
                            if (CurrentActor.Skills[i].CurrentCooldown > 0)
                            {
                                SP4.SetActive(false);
                                ST4.text = CurrentActor.Skills[i].CurrentCooldown.ToString() + " turns";
                            }
                            else
                            {
                                SP4.SetActive(true);
                                ST4.gameObject.SetActive(false);
                            }
                            break;
                        default:
                            SP1.GetComponentInChildren<Text>().text = CurrentActor.Skills[i].Name;
                            if (CurrentActor.Skills[i].CurrentCooldown > 0)
                            {
                                SP1.SetActive(false);
                                ST1.text = CurrentActor.Skills[i].CurrentCooldown.ToString() + " turns";
                            }
                            else
                            {
                                SP1.SetActive(true);
                                ST1.gameObject.SetActive(false);
                            }
                            break;
                    }
                }
            }
            else
            {
                SP1.SetActive(false);
                SP2.SetActive(false);
                SP3.SetActive(false);
                SP4.SetActive(false);
            }
        }
    }


    public void OpenSkillWindow()
    {
        if (!GetComponent<Turnstile>().IsBusy)
        {
            if (GetComponent<Turnstile>().SkillsAvailable)
            {
                MyAudio.PlayOneShot(Select);
                SkillMenu.SetActive(true);
                SkillSetup();
                if (CurrentActor.EnableTutorial == true && CurrentActor.Name == "Doc")
                {
                    GameManager.TurnManager.Director.ReceiveTutorialText('f');
                }
            }
            else
            {
                if (!ErrorPanel.activeSelf)
                {
                    ErrorPanel.SetActive(true);
                    StartCoroutine(HideErrorPanel());
                }
            }
        }
    }

    public IEnumerator HideErrorPanel()
    {
        yield return new WaitForSeconds(2f);
        ErrorPanel.SetActive(false);
    }

    public void CloseSkillWindow()
    {
        SkillMenu.SetActive(false);
        SkillDescPanel.SetActive(false);
    }

}
