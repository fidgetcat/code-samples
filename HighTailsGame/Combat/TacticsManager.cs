using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TacticsManager : MonoBehaviour {

    public LevelBuilder Level;
    public List<Actor> LevelActors;
    public Turnstile TurnManager;

    public GameObject DamageText;
    public GameObject HealText;
    public GameObject StunText;

    public GameObject NormAllySbub;
    public GameObject RedSBUB;
    public GameObject Debuff;
    public GameObject Buff;

    public string ThisSceneName;

    public string NextSceneName;

   public void GameInit()
    {
        FindAllActors();
        TurnManager.PreInit();
        foreach(Actor acty in LevelActors)
        {
            acty.Initialize(1, this, TurnManager); //fix init
        }
        TurnManager.Initialize(LevelActors);
        TurnManager.BeginGame();
    }

    public void InsertNewActor(Actor actor, Vector2Int loc)
    {
        LevelActors.Add(actor);
        actor.Initialize(1, this, TurnManager);
        TurnManager.AddNewActor(actor);
        Level.InsertNewActor(actor, loc);
    }

    public void RemoveActor(Actor actor)
    {
        if (actor != null)
        {
            Level.ActorReferences[actor.CurrentPosition.x, actor.CurrentPosition.y].Remove(actor.gameObject);
            if (LevelActors.Contains(actor))
            {
                LevelActors.Remove(actor);
            }
            TurnManager.RemoveActor(actor);
        }
    }

    public void EndGame()
    {

    }

    public void LoadTitle()
    {
        var doo = GameObject.FindGameObjectWithTag("mus").GetComponent<MusicMan>();
        doo.Play(doo.Combat);
        GameObject.FindGameObjectWithTag("sfxx").GetComponent<SFXDude>().PlaySelect();
        SceneManager.LoadScene("title");
    }

    public void ReloadLevel()
    {
        var doo = GameObject.FindGameObjectWithTag("mus").GetComponent<MusicMan>();
        doo.Play(doo.Dialogue);
        GameObject.FindGameObjectWithTag("sfxx").GetComponent<SFXDude>().PlaySelect();
        SceneManager.LoadScene(ThisSceneName);
    }

    public void LoadNextLevel()
    {
        GameObject.FindGameObjectWithTag("sfxx").GetComponent<SFXDude>().PlaySelect();
        SceneManager.LoadScene(NextSceneName);
    }

    void FindAllActors()
    {
        foreach (List<GameObject> item in Level.ActorReferences)
        {
            foreach (GameObject actor in item)
            {
                var act = actor.GetComponent<Actor>();
                if (act != null)
                {
                    LevelActors.Add(act);
                    //
                }
                else
                {
                }
            }
        }
    }


    public bool CheckIfObstructed(Vector2Int coords)
    {
        foreach (Actor actor in LevelActors)
        {
            if (actor.IsObstructingTile(coords))
            {
                return true;
            }
        }
        return Level.ThisLevel.MyGrid[coords.x, coords.y].IsObstruction;
    }

    public bool CheckIfObstructed(Vector2Int coords, Actor ignore)
    {
        foreach (Actor actor in LevelActors)
        {
            if (actor.IsObstructingTile(coords)&&actor!=ignore)
            {
                return true;
            }
        }
        return Level.ThisLevel.MyGrid[coords.x, coords.y].IsObstruction;
    }

    public bool TileExists(Vector2Int input)
    {
        return Level.ThisLevel.Exists(input);
    }

    /*        if (input.x-1 < Level.ThisLevel.MyGrid.GetLength(0)
          && input.y-1 < Level.ThisLevel.MyGrid.GetLength(0))
        {


        }*/

    public List<Vector2Int> GetValidTiles(Vector2Int origin, int range, bool includesorigin, bool ignoresobstruction)
    {
        List<Vector2Int> output = new List<Vector2Int>();
        //for (x = -range; x <= range; x++) for (y = abs(x)-range; y <= range-abs(x); y++) process_tile (player_x + x, player_y + y);
        for(int x = -range; x<= range; x++)
        {
            for(int y = Mathf.Abs(x)-range; y <= range-Mathf.Abs(x); y++)
            {
               var refvec = new Vector2Int(origin.x + x, origin.y + y);
                if (TileExists(refvec))
                {
                    if (!ignoresobstruction)
                    {
                        if (!CheckIfObstructed(refvec))
                        {
                            if (!includesorigin)
                            {
                                if (refvec != origin)
                                {
                                    output.Add(refvec);
                                }
                            }
                            else
                            {
                                output.Add(refvec);
                            }
                        }
                    }
                    else
                    {
                        if (!includesorigin)
                        {
                            if (refvec != origin)
                            {
                                output.Add(refvec);
                            }
                        }
                        else
                        {
                            output.Add(refvec);
                        }
                    }
                }
            }
        }

        return output;
    }

    public List<Vector2Int> GetPlayerWalkTiles(Vector2Int origin, int range)
    {
        List<Vector2Int> output = new List<Vector2Int>();
        //for (x = -range; x <= range; x++) for (y = abs(x)-range; y <= range-abs(x); y++) process_tile (player_x + x, player_y + y);
        for (int x = -range; x <= range; x++)
        {
            for (int y = Mathf.Abs(x) - range; y <= range - Mathf.Abs(x); y++)
            {
                var refvec = new Vector2Int(origin.x + x, origin.y + y);
                if (TileExists(refvec))
                {
                    if (!CheckIfObstructed(refvec))
                    {
                            if (refvec != origin)
                            {
                                output.Add(refvec);
                            }
                    }
                }
            }
        }
        var list = output;
        var nodes = new List<Node>();
        var start = new Node(999, origin);
        foreach (Vector2Int coord in list)
        {
            var nd = new Node(999, coord);
            nodes.Add(nd);
        }
        nodes.Add(start);
        foreach (Node nd in nodes)
        {
            if (list.Exists(x => x.x == nd.Coordinates.x + 1 && x.y == nd.Coordinates.y))
            {
                nd.Adjacent.Add(nodes.Find(x => x.Coordinates.x == nd.Coordinates.x + 1 && x.Coordinates.y == nd.Coordinates.y));
            }
            if (list.Exists(x => x.x == nd.Coordinates.x - 1 && x.y == nd.Coordinates.y))
            {
                nd.Adjacent.Add(nodes.Find(x => x.Coordinates.x == nd.Coordinates.x - 1 && x.Coordinates.y == nd.Coordinates.y));
            }
            if (list.Exists(x => x.x == nd.Coordinates.x && x.y == nd.Coordinates.y + 1))
            {
                nd.Adjacent.Add(nodes.Find(x => x.Coordinates.x == nd.Coordinates.x && x.Coordinates.y == nd.Coordinates.y + 1));
            }
            if (list.Exists(x => x.x == nd.Coordinates.x && x.y == nd.Coordinates.y - 1))
            {
                nd.Adjacent.Add(nodes.Find(x => x.Coordinates.x == nd.Coordinates.x && x.Coordinates.y == nd.Coordinates.y - 1));
            }
        }
        nodes.Remove(start);
        var path = new List<Node>();
        Node.DijkstraSearch(start);
        //find viable end node
        nodes.RemoveAll(x => x.Distance > range);

        var nlist = new List<Vector2Int>();
        foreach (Node item in nodes)
        {
            nlist.Add(item.Coordinates);
        }
        return nlist;
    }



    /*public List<Vector2Int> MoveRangeSearch(Vector2Int origin, List<Vector2Int> area, int range)
    {
        List<Vector2Int> Unexplored = new List<Vector2Int>(area);
        List<Node> Explored = new List<Node>();
        List<Vector2Int> Final = new List<Vector2Int>();

        foreach(Vector2Int item in Unexplored)
        {
            var distance = (int)Mathf.Sqrt(((item.x - origin.x) * (item.x - origin.x)) + ((item.y - origin.y) * (item.y - origin.y)));
            Explored.Add(new Node(distance,item));
        }

        foreach(Node vec in Explored)
        {
            int distance = 999;
            if(Explored.Exists(x => x.Coordinates.x == vec.Coordinates.x && x.Coordinates.y == vec.Coordinates.y+1))
            {
                var potdist = Explored.Find(x => x.Coordinates.x == vec.Coordinates.x && x.Coordinates.y == vec.Coordinates.y + 1).Distance + 1;
                if (potdist < distance)
                {
                    distance = potdist;
                }
            }
            if (Explored.Exists(x => x.Coordinates.x == vec.Coordinates.x && x.Coordinates.y == x.Coordinates.y - 1))
            {
                var potdist = Explored.Find(x => x.Coordinates.x == vec.Coordinates.x && x.Coordinates.y == vec.Coordinates.y - 1).Distance + 1;
                if (potdist < distance)
                {
                    distance = potdist;
                }
            }
            if (Explored.Exists(x => x.Coordinates.x == vec.Coordinates.x + 1 && x.Coordinates.y == vec.Coordinates.y))
            {
                var potdist = Explored.Find(x => x.Coordinates.x == vec.Coordinates.x + 1 && x.Coordinates.y == vec.Coordinates.y).Distance + 1;
                if (potdist < distance)
                {
                    distance = potdist;
                }
            }
            if (Explored.Exists(x => x.Coordinates.x == vec.Coordinates.x - 1 && x.Coordinates.y == vec.Coordinates.y))
            {
                var potdist = Explored.Find(x => x.Coordinates.x == vec.Coordinates.x - 1 && x.Coordinates.y == vec.Coordinates.y).Distance + 1;
                if (potdist < distance)
                {
                    distance = potdist;
                }
            }

            vec.Distance = distance;
        }

        foreach (Node vec in Explored)
        {
            int distance = 999;
            if (Explored.Exists(x => x.Coordinates.x == vec.Coordinates.x && x.Coordinates.y == vec.Coordinates.y + 1))
            {
                var potdist = Explored.Find(x => x.Coordinates.x == vec.Coordinates.x && x.Coordinates.y == vec.Coordinates.y + 1).Distance + 1;
                if (potdist < distance)
                {
                    distance = potdist;
                }
            }
            if (Explored.Exists(x => x.Coordinates.x == vec.Coordinates.x && x.Coordinates.y == x.Coordinates.y - 1))
            {
                var potdist = Explored.Find(x => x.Coordinates.x == vec.Coordinates.x && x.Coordinates.y == vec.Coordinates.y - 1).Distance + 1;
                if (potdist < distance)
                {
                    distance = potdist;
                }
            }
            if (Explored.Exists(x => x.Coordinates.x == vec.Coordinates.x + 1 && x.Coordinates.y == vec.Coordinates.y))
            {
                var potdist = Explored.Find(x => x.Coordinates.x == vec.Coordinates.x + 1 && x.Coordinates.y == vec.Coordinates.y).Distance + 1;
                if (potdist < distance)
                {
                    distance = potdist;
                }
            }
            if (Explored.Exists(x => x.Coordinates.x == vec.Coordinates.x - 1 && x.Coordinates.y == vec.Coordinates.y))
            {
                var potdist = Explored.Find(x => x.Coordinates.x == vec.Coordinates.x - 1 && x.Coordinates.y == vec.Coordinates.y).Distance + 1;
                if (potdist < distance)
                {
                    distance = potdist;
                }
            }

            vec.Distance = distance;
            if (vec.Distance <= range+1)
                Final.Add(vec.Coordinates);
        }

        return Final;
    }*/

    void Start () {
        TurnManager = GetComponent<Turnstile>();
        Level = GetComponent<LevelBuilder>();
        LevelActors = new List<Actor>();

	}

	void Update () {
		
	}
}
