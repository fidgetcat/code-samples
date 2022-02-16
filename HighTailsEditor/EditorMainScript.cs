using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class EditorMainScript : MonoBehaviour {
    public TileGrid ThisLevel;
    public TileGridReference ThisLevelReference;
    public GameObject SaveTextObject;
    public GameObject LoadTextObject;
    public GameObject LevelSizeObject;

    public List<GameObject>[,] ActorReferences;
    public List<GameObject>[,] OrnamentReferences;

    public GameObject CamPivot;

    public GameObject assetFieldText;
    public Text AssetFieldText;

    void Start()
    {
        ThisLevel = new TileGrid();
        AssetFieldText = assetFieldText.GetComponent<Text>();
    }

    /*   public void SaveLevel()
       {
           string data = JsonUtility.ToJson(ThisLevel);
           string path = Application.dataPath + "/levels/" + SaveTextObject.GetComponent<InputField>().text + ".json";
           File.WriteAllText(path, data);
       }

       public void LoadLevel()
       {
           string path = Application.dataPath + "/levels/" + LoadTextObject.GetComponent<InputField>().text + ".json"; 
           if (File.Exists(path))
           {
               string data = File.ReadAllText(path);
               var thing = JsonUtility.FromJson<TileGrid>(data);
               ThisLevel = thing;
               BuildLevel();
           }
       } */

    public void SaveLevel()
    {
        string path = Application.dataPath + "/levels/" + SaveTextObject.GetComponent<InputField>().text + ".birb";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, ThisLevel);
        file.Close();
    }

    public void LoadLevel()
    {
        string path = Application.dataPath + "/levels/" + LoadTextObject.GetComponent<InputField>().text + ".birb";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            ThisLevel = (TileGrid)bf.Deserialize(file);
            file.Close();
            BuildLevel();
        }
    }

    public void SetGridDimensions()
    {
        var input = int.Parse(LevelSizeObject.GetComponent<InputField>().text);
        ThisLevel.size = input;
        ThisLevel.MyGrid = new TileStack[input, input];
        ThisLevel.MyActorGrid = new ActorGrid(input);
        ThisLevel.MyOrnaments = new OrnamentStack[input, input];
        for(int y = 0; y < input; y++)
        {
            for(int x = 0; x< input; x++)
            {
                var thing = new TileStack
                {
                    X=x,
                    Y=y,
                    MyStack = new List<BlockTile>(),
                    IsObstruction = false,
                    IsSpawn = false
                };
                thing.MyStack.Add(new BlockTile("defaultile",1f));
                ThisLevel.MyGrid[x, y] = thing;
            }
        }
        BuildLevel();
    }

    public GameObject RebuildStack(int x, int y, int reference)
    {
        if (OrnamentReferences[x, y] == null)
        {
            OrnamentReferences[x, y] = new List<GameObject>();
        }
        if (OrnamentReferences[x, y].Count >= 1)
        {
            foreach (GameObject thingy in OrnamentReferences[x, y])
            {
                Destroy(thingy);
                OrnamentReferences[x, y] = new List<GameObject>();
            }
        }
        foreach (GameObject thingy in ThisLevelReference.MyGrid[x, y].Blocks)
        {
            Destroy(thingy);
        }
        if (ActorReferences[x, y] != null)
        {
            foreach (GameObject thingy in ActorReferences[x, y])
            {
                Destroy(thingy);
                ActorReferences[x, y] = new List<GameObject>();
            }
        }
        else
        {
            ActorReferences[x, y] = new List<GameObject>();
        }
        var xzero = ThisLevel.size / 2;
        var yzero = xzero;
        var tile = ThisLevel.MyGrid[x, y];
        float newpos = 0f;
        float previouspos = 0f;
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
            var cube = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Tiles/" + tile.MyStack[i].TileName, typeof(GameObject))) as GameObject, new Vector3(yzero - y, newpos, xzero - x), Quaternion.identity);
            cube.transform.localScale = new Vector3(cube.transform.localScale.x, cube.transform.localScale.y* tile.MyStack[i].Height, cube.transform.localScale.z);
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
                    var actor = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                    ActorReferences[x, y].Add(actor);
                }
            }
            else
            {
                ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName = new List<string>();
                for (int i = 0; i < ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName.Count; i++)
                {
                    var actor = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
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
                    var actor = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                    ActorReferences[x, y].Add(actor);
                }
            }
            else
            {
                ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName = new List<string>();
                for (int i = 0; i < ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName.Count; i++)
                {
                    var actor = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                    ActorReferences[x, y].Add(actor);
                }
            }
        }
        for (int i = 0; i < ThisLevel.MyOrnaments[x, y].MyOrnaments.Count; i++)
        {
            var ornament = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Ornaments/" + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].PrefabName, typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
            ornament.transform.Rotate(new Vector3(ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalRotation.X, ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalRotation.Y, ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalRotation.Z), Space.World);
            ornament.transform.position = (new Vector3(ornament.transform.position.x+ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalTransform.X, ornament.transform.position.y + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalTransform.Y, ornament.transform.position.z + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalTransform.Z));
            ornament.transform.localScale = (new Vector3(ornament.transform.localScale.x * ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalScale.X, ornament.transform.localScale.y * ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalScale.Y, ornament.transform.localScale.z * ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalScale.Z));
            OrnamentReferences[x, y].Add(ornament);
        }

        return ThisLevelReference.MyGrid[x,y].Blocks[reference];

        //loop through ornaments
        //loop through actors
    }

    public void BuildLevel()
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
                    var cube = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Tiles/" + tile.MyStack[i].TileName, typeof(GameObject))) as GameObject, new Vector3(yzero - y, newpos, xzero - x), Quaternion.identity);
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
                            var actor = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x,y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count-1].transform.position.x, totalheight+0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                            ActorReferences[x, y].Add(actor);
                        }
                    }
                    else
                    {
                        ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName = new List<string>();
                        for (int i = 0; i < ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName.Count; i++)
                        {
                            var actor = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
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
                            var actor = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                            ActorReferences[x, y].Add(actor);
                        }
                    }
                    else
                    {
                        ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName = new List<string>();
                        for (int i = 0; i < ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName.Count; i++)
                        {
                            var actor = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Actors/" + ThisLevel.MyActorGrid.MyGrid[x, y].PrefabName[i], typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                            ActorReferences[x, y].Add(actor);
                        }
                    }
                }
                if(ThisLevel.MyOrnaments[x, y] == null)
                {
                    ThisLevel.MyOrnaments[x, y] = new OrnamentStack(x, y);
                }
                for (int i = 0; i < ThisLevel.MyOrnaments[x, y].MyOrnaments.Count; i++)
                {
                    var ornament = GameObject.Instantiate((Resources.Load(AssetFieldText.text + "/" + "Ornaments/" + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].PrefabName, typeof(GameObject))) as GameObject, new Vector3(ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.x, totalheight + 0.3f, ThisLevelReference.MyGrid[x, y].Blocks[ThisLevelReference.MyGrid[x, y].Blocks.Count - 1].transform.position.z), Quaternion.identity);
                    //apply transforms
                    ornament.transform.Rotate(new Vector3(ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalRotation.X, ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalRotation.Y, ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalRotation.Z), Space.World);
                    ornament.transform.position = (new Vector3(ornament.transform.position.x + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalTransform.X, ornament.transform.position.y + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalTransform.Y, ornament.transform.position.z + ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalTransform.Z));
                    ornament.transform.localScale = (new Vector3(ornament.transform.localScale.x * ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalScale.X, ornament.transform.localScale.y * ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalScale.Y, ornament.transform.localScale.z * ThisLevel.MyOrnaments[x, y].MyOrnaments[i].LocalScale.Z));
                    if(OrnamentReferences[x, y] == null)
                    {
                        OrnamentReferences[x, y] = new List<GameObject>();
                    }
                    OrnamentReferences[x, y].Add(ornament);
                }
            }
        }
    }

    void GridTest()
    {
        Debug.Log("Beginning test for real grid");
        for (int y = 0; y < ThisLevel.size; y++)
        {
            for (int x = 0; x < ThisLevel.size; x++)
            {
                Debug.Log(ThisLevel.MyGrid[x, y].X.ToString() + "," + ThisLevel.MyGrid[x, y].Y.ToString());
            }
        }
    }

    void GridTestRef()
    {
        Debug.Log("Beginning test for grid reference");
        for (int y = 0; y < ThisLevel.size; y++)
        {
            for (int x = 0; x < ThisLevel.size; x++)
            {
                Debug.Log(ThisLevelReference.MyGrid[x,y].pos.ToString());
            }
        }
    }

    void Update () {
		
	}
}
