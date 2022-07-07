using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class LevelBuilder : MonoBehaviour {
    public TileGrid ThisLevel;
    public TileGridReference ThisLevelReference;
    public GameObject LoadPanel;

    public List<TilePosHolder> TilePositions;

    public List<GameObject>[,] ActorReferences;
    public List<GameObject>[,] OrnamentReferences;

    public GameObject CamPivot;
    public GameObject LoadTextObject;
    public TacticsManager GM;
    public string levname;


    private void BuildLevel()
    {
        var xzero = ThisLevel.size / 2;
        var yzero = xzero;
        ThisLevelReference = new TileGridReference();
        ThisLevelReference.MyGrid = new TileStackReference[ThisLevel.size, ThisLevel.size];
        ThisLevelReference.size = ThisLevel.size;
        ActorReferences = new List<GameObject>[ThisLevel.size, ThisLevel.size];
        OrnamentReferences = new List<GameObject>[ThisLevel.size, ThisLevel.size];
        for (int y = 0; y < ThisLevel.size; y++)
        {
            for (int x = 0; x < ThisLevel.size; x++)
            {
                float newpos = 0f;
                float previouspos = 0f;
                var tile = ThisLevel.MyGrid[x, y];
                float totalheight = 0f;
                for (int i = 0; i < tile.MyStack.Count; i++)
                {
                    if (i == 0)
                    {
                        ThisLevelReference.MyGrid[x, y] = new TileStackReference
                        {
                            Blocks = new List<GameObject>(),
                            pos = new Vector2(tile.X, tile.Y)
                        };

                        if (tile.MyStack[i].Height <= 0)
                        {
                            tile.MyStack[i].Height = 0f;
                            newpos = 0f;
                        }
                        else
                        {
                            newpos = (tile.MyStack[i].Height - 1) / 2;
                        }
                        previouspos = newpos;
                    }
                    else
                    {
                        newpos = (tile.MyStack[i - 1].Height / 2) + (tile.MyStack[i].Height / 2) + previouspos;
                        previouspos = newpos;
                    }
                    var cube = GameObject.Instantiate((Resources.Load("Tiles/" + tile.MyStack[i].TileName, typeof(GameObject))) as GameObject, new Vector3(yzero - y, newpos, xzero - x), Quaternion.identity);
                    cube.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y * tile.MyStack[i].Height, cube.transform.localScale.z);
                    cube.GetComponent<TilePosHolder>().pos = new Vector2(tile.X, tile.Y);
                    cube.GetComponent<TilePosHolder>().StackCount = i;
                    totalheight += tile.MyStack[i].Height;
                    ThisLevelReference.MyGrid[x, y].Blocks.Add(cube);
                }

                if (ActorReferences[x, y] == null)
                {
                    ActorReferences[x, y] = new List<GameObject>();
                }

                if (ThisLevel.MyActorGrid.MyGrid[x, y] != null)
                {
                    if (ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName != null)
                    {
                        for (int i = 0; i < ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName.Count; i++)
                        {
                            var actor = GameObject.Instantiate((Resources.Load("Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                            actor.GetComponent<Actor>().CurrentPosition = new Vector2Int((int)x, (int)y);
                            ActorReferences[x, y].Add(actor);
                        }
                    }
                    else
                    {
                        ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName = new List<string>();
                        for (int i = 0; i < ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName.Count; i++)
                        {
                            var actor = GameObject.Instantiate((Resources.Load("Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                            actor.GetComponent<Actor>().CurrentPosition = new Vector2Int((int)x, (int)y);
                            ActorReferences[x, y].Add(actor);
                        }
                    }
                }
                else
                {
                    ThisLevel.MyActorGrid.MyGrid[x, y] = new ActorData();
                    if (ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName != null)
                    {
                        for (int i = 0; i < ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName.Count; i++)
                        {
                            var actor = GameObject.Instantiate((Resources.Load("Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                            actor.GetComponent<Actor>().CurrentPosition = new Vector2Int((int)x, (int)y);
                            ActorReferences[x, y].Add(actor);
                        }
                    }
                    else
                    {
                        ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName = new List<string>();
                        for (int i = 0; i < ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName.Count; i++)
                        {
                            var actor = GameObject.Instantiate((Resources.Load("Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                            actor.GetComponent<Actor>().CurrentPosition = new Vector2Int((int)x,(int)y);
                            ActorReferences[x, y].Add(actor);
                        }
                    }
                }
                if (ThisLevel.MyOrnaments[x, y] == null)
                {
                    ThisLevel.MyOrnaments[x, y] = new OrnamentStack(x, y);
                }
                for (int i = 0; i < ThisLevel.MyOrnaments[x, y].MyOrnaments.Count; i++)
                {
                    var ornament = GameObject.Instantiate((Resources.Load("Ornaments/" + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].PrefabName, typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                    //apply transforms
                    ornament.transform.Rotate(new Vector3(ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalRotation.X, ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalRotation.Y, ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalRotation.Z), Space.World);
                    ornament.transform.position = (new Vector3(ornament.transform.position.x + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalTransform.X, ornament.transform.position.y + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalTransform.Y, ornament.transform.position.z + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalTransform.Z));
                    ornament.transform.localScale = (new Vector3(ornament.transform.localScale.x * ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalScale.X, ornament.transform.localScale.y * ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalScale.Y, ornament.transform.localScale.z * ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalScale.Z));
                    if (OrnamentReferences[x, y] == null)
                    {
                        OrnamentReferences[x, y] = new List<GameObject>();
                    }
                    OrnamentReferences[x, y].Add(ornament);
                }
            }
        }
        LoadPanel.SetActive(false);
        GM.GameInit();
    }

    public void InsertNewActor(Actor actor, Vector2Int pos)
    {
        ActorReferences[pos.x, pos.y].Add(actor.gameObject);
    }

    public void RemoveActor(Actor actor)
    {
        ActorReferences[actor.CurrentPosition.x, actor.CurrentPosition.y].Remove(actor.gameObject);
    }

    public void MoveActor(Actor act, Vector2Int pos)
    {
        if (ActorReferences[act.CurrentPosition.x, act.CurrentPosition.y].Contains(act.gameObject))
        {
            ActorReferences[act.CurrentPosition.x, act.CurrentPosition.y].Remove(act.gameObject);
            ActorReferences[pos.x, pos.y].Add(act.gameObject);

        }
        else
        {
            ActorReferences[pos.x, pos.y].Add(act.gameObject);
        }
        act.CurrentPosition = pos;
    }

    public void LoadLevel()
    {
        var ting = GameObject.FindGameObjectWithTag("sfxx");
        if(ting!=null)
        ting.GetComponent<SFXDude>().PlaySelect();
        string path = Application.dataPath + "/levels/" + levname + ".birb";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            ThisLevel = (TileGrid)bf.Deserialize(file);
            file.Close();
            BuildLevel();
        }
    }

    void Start()
    {
        ThisLevel = new TileGrid();
        GM = GetComponent<TacticsManager>();
        //LoadTextObject.GetComponent<InputField>().text = "prototest";
        StartCoroutine(FuckShit());
    }

    public IEnumerator FuckShit()
    {
        yield return new WaitForSeconds(1f);
        LoadLevel();
    }

    void Update () {
		
	}
}
