using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelScript : LevelScript
{
    public enum GoonType { Logic, Aggression, Comedy };
    public List<GoonType> GoonList;

    public List<Vector2Int> SpawnPoints;


    public GameObject LogicGoon;
    public GameObject AggressionGoon;
    public GameObject ComedyGoon;


    public override void SendFlag(char input)
    {
        base.SendFlag(input);
        if (input == 'a')
        {
            InitTestLevel();
        }
    }

    public override void Init(Turnstile turnmanager)
    {
        base.Init(turnmanager);
        FirstTutorial = false;
        if (TutPanel != null)
            TutPanel.SetActive(false);
    }


    public void InitTestLevel()
    {
        var morgan = (Morgan)TurnManager.PlayerActors.Find(x => x is Morgan);
        var doc = (Doc)TurnManager.PlayerActors.Find(x => x is Doc);
        if (morgan != null)
        {
            morgan.EnableTutorial = false;
            morgan.TutorialSkillUsed = true;
        }
        if (doc != null)
        {
            doc.EnableTutorial = false;
            doc.TutorialSkillUsed = true;
        }
        SpawnPoints = new List<Vector2Int>();
        foreach (TileStack thing in TurnManager.MyBrain.Level.ThisLevel.MyGrid)
        {
            if (thing.IsSpawn)
                SpawnPoints.Add(new Vector2Int(thing.X, thing.Y));

        }
        var actlist = new List<Act>();
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            if (i >= GoonList.Count)
                break;
            else
            {
                switch (GoonList[i])
                {
                    case GoonType.Aggression:
                        var list = new List<Effect>();
                        var tarlist = new List<Actor>();
                        list.Add(new SpawnUnit(TurnManager.MyBrain, AggressionGoon, SpawnPoints[i]));
                        var ak = new Act()
                        {
                            MyType = Act.ActType.Action,
                            Effects = list,
                            MyModule = null
                        };
                        list[0].ReceiveAct(ak);
                        actlist.Add(ak);
                        break;
                    case GoonType.Comedy:
                        var list2 = new List<Effect>();
                        var tarlist2 = new List<Actor>();
                        list2.Add(new SpawnUnit(TurnManager.MyBrain, ComedyGoon, SpawnPoints[i]));
                        var ak2 = new Act()
                        {
                            MyType = Act.ActType.Action,
                            Effects = list2,
                            MyModule = null
                        };
                        list2[0].ReceiveAct(ak2);
                        actlist.Add(ak2);
                        break;
                    case GoonType.Logic:
                        var list3 = new List<Effect>();
                        var tarlist3 = new List<Actor>();
                        list3.Add(new SpawnUnit(TurnManager.MyBrain, LogicGoon, SpawnPoints[i]));
                        var ak3 = new Act()
                        {
                            MyType = Act.ActType.Action,
                            Effects = list3,
                            MyModule = null
                        };
                        list3[0].ReceiveAct(ak3);
                        actlist.Add(ak3);
                        break;
                }
            }
        }
        TurnManager.ReceiveSceneAct(actlist);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
