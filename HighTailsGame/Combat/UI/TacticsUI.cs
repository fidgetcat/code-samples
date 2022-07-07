using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TacticsUI : MonoBehaviour
{
    public GameObject TileHighlight;
    public GameObject CutHighlight;
    public TacticsManager GM;
    public List<GameObject> temptiles;
    public List<GameObject> RangeTiles;
    public GameObject LastTarget;
    public Text moveinfotext;
    public List<GameObject> CutTiles;
    public List<GameObject> AtkTiles;

    public bool SelectMove;
    public bool SelectCut;
    public bool SelectAtk;
    public bool SelectInject;
    public bool CutsceneMode;

    public GameObject PlayerMenu;
    public GameObject SkillMenu;
    public AudioSource MyAudio;
    public AudioClip Error;


    public Morgan CurrentActor;

    Vector2Int CheckMouseHover(RaycastHit input)
    {
        if (input.collider.gameObject.CompareTag("block"))
        {
            return new Vector2Int((int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.x, (int)input.collider.gameObject.GetComponent<TilePosHolder>().pos.y);
        }
        else
            return new Vector2Int(999,999);
    }

    public void OpenSkillWindow()
    {
        SkillMenu.SetActive(true);
    }

    public void CloseSkillWindow()
    {
        SkillMenu.SetActive(false);
    }

    void HighlightTiles(List<Vector2Int> input)
    {
        RangeTiles = new List<GameObject>();
        temptiles = new List<GameObject>();
        for(int i = 0; i < input.Count; i++)
        {
          var ting =  new Vector2(input[i].x, input[i].y);
            //
            //
            if (GM.Level.ThisLevelReference.MyGrid[input[i].x, input[i].y].Blocks!=null)
            {
                temptiles.Add(Instantiate(TileHighlight, GM.Level.ThisLevelReference.MyGrid[input[i].x, input[i].y].GetTopBlockTransform().position, Quaternion.identity));
                RangeTiles.Add(GM.Level.ThisLevelReference.MyGrid[input[i].x, input[i].y].Blocks[0]);
            }
        }
        
    }

    public void CancelMoveMenu()
    {
        ClearMoveTiles();
        DisableCutHighlight();
        SelectMove = false;
        PlayerMenu.SetActive(false);
    }

    public void EndTurn()
    {
        CloseSkillWindow();
        PlayerMenu.SetActive(false);
        CurrentActor.GetEndTurnAction();
    }

    public void EndTurnCleanup()
    {
        CancelMoveMenu();
        SelectCut = false;
        SelectAtk = false;
        SelectInject = false;
    }

    public void SendMoveToActor(Vector2Int input)
    {
        CurrentActor.GetWalkAction(input);
        CancelMoveMenu();
    }


    public void ShowMoveRange()
    {
        if (SelectMove != true && SelectCut !=true && CurrentActor.TurnMoveCount > 0 && SelectInject !=true && SelectAtk !=true)
        {
            //
            HighlightTiles(GM.GetPlayerWalkTiles(CurrentActor.CurrentPosition, CurrentActor.TurnMoveCount));
            //HighlightTiles(GM.MoveRangeSearch(CurrentActor.CurrentPosition, GM.GetValidTiles(CurrentActor.CurrentPosition, CurrentActor.MyProfile.GetStat(ActorStat.StatType.Movement), false, false),CurrentActor.MyProfile.GetStat(ActorStat.StatType.Movement)));
            SelectMove = true;
        }
    }

    public void ShowInjectRange()
    {
        if (CurrentActor.CurrentTurn.Actcount > 0)
        {
            if (SelectMove != true && SelectCut != true && SelectInject != true && SelectAtk != true)
            {
                HighlightTiles(GM.GetValidTiles(CurrentActor.CurrentPosition, 1, true, true));
                SelectInject = true;
            }
        }
    }

    public void ShowAtkRange()
    {
        if (CurrentActor.CurrentTurn.Actcount > 0)
        {
            if (SelectMove != true && SelectCut != true && SelectInject != true && SelectAtk != true)
            {
                HighlightTiles(GM.GetValidTiles(CurrentActor.CurrentPosition, CurrentActor.MyProfile.GetStat(ActorStat.StatType.Range), true, true));
                SelectAtk = true;
            }
        }
    }

    public void ShowCutRange()
    {
        if (CurrentActor.CurrentTurn.Actcount > 0)
        {
            if (SelectMove != true && SelectCut != true && SelectInject != true && SelectAtk != true)
            {
                HighlightTiles(GM.GetValidTiles(CurrentActor.CurrentPosition, 1, true, true));
                SelectCut = true;
            }
        }
    }


    public bool SendAtkToActor(Vector2Int dir)
    {
        var list = GM.Level.ActorReferences[dir.x, dir.y];
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
            PlayError();
            return false;
        }
    }


    public void DisableCutHighlight()
    {
        if (CutTiles.Count > 0)
        {
            foreach(GameObject thing in CutTiles)
            {
                Destroy(thing);
            }
            CutTiles = new List<GameObject>();
        }
    }

    public void ClearMoveTiles()
    {
        foreach(GameObject reference in temptiles)
        {
            Destroy(reference);
        }
        SelectMove = false;
        SelectCut = false;
        SelectInject = false;
        SelectAtk = false;
    }

    public void PlayError()
    {
        MyAudio.PlayOneShot(Error);
    }

    void Start()
    {
        MyAudio = GetComponent<AudioSource>();
        SelectMove = false;
        SelectCut = false;
        SelectAtk = false;
        SelectInject = false;
        GM = GetComponent<TacticsManager>();
        temptiles = new List<GameObject>();
        RangeTiles = new List<GameObject>();
        CutTiles = new List<GameObject>();
        AtkTiles = new List<GameObject>();
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

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (SelectMove)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.CompareTag("block") && RangeTiles.Contains(hit.collider.gameObject))
                        {
                            //
                        }
                    }
                }
            }

            else if (SelectAtk)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        var basedir = new Vector2Int((int)hit.collider.gameObject.GetComponent<TilePosHolder>().pos.x, (int)hit.collider.gameObject.GetComponent<TilePosHolder>().pos.y);
                        List<TilePosHolder> lis = new List<TilePosHolder>();
                        foreach(GameObject thing in RangeTiles)
                        {
                            lis.Add(thing.GetComponent<TilePosHolder>());
                        }

                        //
                        if (lis.Exists(x => x.pos == basedir))
                        {
                            if (SendAtkToActor(basedir))
                            {
                                DisableCutHighlight();
                                SelectMove = false;
                                SelectCut = false;
                                SelectAtk = false;
                                SelectInject = false;
                                PlayerMenu.SetActive(false);
                                CancelMoveMenu();
                            }
                            else
                            {
                                //play error sound
                            }
                        }
                        else
                        {
                            PlayError();
                        }
                    }
                }
            }
            else
            {

            }
        }
    }
}
