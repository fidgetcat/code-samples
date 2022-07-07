using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Act {
    public enum ActType { Movement, Action};
    public ActType MyType;
    public List<Effect> Effects;
    public List<Actor> Targets;
    public Module MyModule;

    public string ActName = "---";

    public string Catchphrase = "You suck!";
    public bool IsCriticalCutscene = false;

    public bool IsCriticalDamage = false;
}

public class Module
{
    public enum ModuleClass {Active, Passive, Mixed };
    public ModuleClass MyClass;
    public Actor MyActor;

    public void CriticalCheck(Act akk, float input)
    {
        if (input >= 1.9f)
        {
            akk.IsCriticalCutscene = true;
            akk.IsCriticalDamage = true;
        }
    }

    public virtual void InitApply()
    {
        
    }
}

/* public class SnideRemarks : Skill
{
    public SnideRemarks(AreaDelegate adel, Actor actor)
    {
        Name = "Snide Remarks";
        Range = 4;
        EnemyOnly = true;
        AllyOnly = false;
        MyTargetType = TargetType.Area;
        AreaDel = adel;
        MyActor = actor;
    }
} */

public class Skill
{
    public string Name;
    public int Range;
    public bool AllyOnly;
    public bool EnemyOnly;
    public enum TargetType { Actor, Area, Point};
    public TargetType MyTargetType;

    public int Cooldown;
    public int CurrentCooldown;

    public Actor MyActor;

    public delegate Act ActorDelegate(Actor target, float eff);
    public delegate Act AreaDelegate(List<Vector2Int> target, float eff);
    public delegate Act PointDelegate(Vector2Int point, float eff);

    public ActorDelegate ActorDel;
    public AreaDelegate AreaDel;
    public PointDelegate PointDel;

    public string Description;

    public Skill(string name, Actor actor, ActorDelegate adel, int range, bool allyonly, bool enemyonly, int cd)
    {
        Name = name;
        MyActor = actor;
        MyTargetType = TargetType.Actor;
        ActorDel = adel;
        Range = range;
        AllyOnly = allyonly;
        EnemyOnly = enemyonly;
        Cooldown = cd;
        CurrentCooldown = 0;
    }

    public Skill(string name, Actor actor, AreaDelegate ardel, int range, bool allyonly, bool enemyonly, int cd)
    {
        Name = name;
        MyActor = actor;
        MyTargetType = TargetType.Area;
        AreaDel = ardel;
        Range = range;
        AllyOnly = allyonly;
        EnemyOnly = enemyonly;
        Cooldown = cd;
        CurrentCooldown = 0;
    }

    public Skill(string name, Actor actor, PointDelegate pdel, int range, bool allyonly, bool enemyonly, int cd)
    {
        Name = name;
        MyActor = actor;
        MyTargetType = TargetType.Point;
        PointDel = pdel;
        Range = range;
        AllyOnly = allyonly;
        EnemyOnly = enemyonly;
        Cooldown = cd;
        CurrentCooldown = 0;
    }

    public virtual void UseSkill(Actor targ, float efficacy)
    {
        if (ActorDel != null)
        {
            CurrentCooldown = Cooldown;
            MyActor.CurrentTurn.SubmitAction(ActorDel(targ, efficacy));
        }
        else
        {
        }
    }

    public virtual void UseSkill(Vector2Int targ, float efficacy)
    {
        if (PointDel != null)
        {
            CurrentCooldown = Cooldown;
            MyActor.CurrentTurn.SubmitAction(PointDel(targ, efficacy));
        }
        else
        {
        }
    }

    public virtual void UseSkill(List<Vector2Int> targ, float efficacy)
    {
        if (AreaDel != null)
        {
            CurrentCooldown = Cooldown;
            MyActor.CurrentTurn.SubmitAction(AreaDel(targ, efficacy));
        }
        else
        {
        }
    }

    public virtual TargetingModule.TargetingData GetTargetingData(Vector2Int origin)
    {
        return new TargetingModule.TargetingData();
    }

    public bool CheckAnyWithinRange()
    {
        if(MyTargetType== TargetType.Area)
        {
            return VerifyTargetArea(GetTargetingData(MyActor.CurrentPosition).Tiles);
        }
        else
        {
            var tiles = MyActor.GameManager.GetValidTiles(MyActor.CurrentPosition, Range, true, true);
            if (AllyOnly)
            {
                if(tiles.Exists(x=> MyActor.TurnManager.ActorQueue.Exists(y=> x == y.CurrentPosition && y.ThisType == MyActor.ThisType)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (tiles.Exists(x => MyActor.TurnManager.ActorQueue.Exists(y => x == y.CurrentPosition && y.ThisType != MyActor.ThisType)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public bool VerifyTargetArea(List<Vector2Int> area)
    {
        if (AllyOnly)
        {
            if(MyActor.GameManager.LevelActors.Any(x=> area.Any(y=> y == x.CurrentPosition && x.ThisType == Actor.UnitType.Organic)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (EnemyOnly)
        {
            if (MyActor.GameManager.LevelActors.Any(x => area.Any(y => y == x.CurrentPosition && x.ThisType == Actor.UnitType.AI)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (MyActor.GameManager.LevelActors.Any(x => area.Any(y => y == x.CurrentPosition)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public List<Vector2Int> ProvideTargetArea(Vector2Int origin)
    {
        return GetTargetingData(origin).Tiles;
    }
}

public class LaughingGas : Skill
{
    // public Skill(string name, Actor actor, PointDelegate pdel, int range, bool allyonly, bool enemyonly, int cd)

    public LaughingGas(Actor act, AreaDelegate adl) : base("Laughing Gas", act, adl, 0, false, true, 5)
    {
        MyTargetType = TargetType.Area;
    }

    public override TargetingModule.TargetingData GetTargetingData(Vector2Int origin)
    {
        return MyActor.TargModule.GenerateTargetList(TargetingModule.TargetBehavior.WithinArea, TargetingModule.AreaSelection.PointBlank, TargetingModule.SelectionShape.Diamond, origin, 20, 2);
    }
}

public class BasicModule : Module
{
    public BasicModule(Actor myactor)
    {
        MyActor = myactor;
        MyClass = ModuleClass.Mixed;
    }

    public Act CreateEndTurnAct()
    {

        var me = new List<Actor>();
        me.Add(this.MyActor);
        var list = new List<Effect>();
        list.Add(new EndTurn(me));
        var ak = new Act()
        {
            MyType = Act.ActType.Action,
            Effects = list,
            Targets = me
        };
        list[0].ReceiveAct(ak);
        ak.ActName = "End Turn";
        return ak;
    }

    public override void InitApply()
    {
        MyActor.CurrentAffects.Add(new CheckForDeath(MyActor));
    }
}



public class Locomotion : Module
{
    public Locomotion(Actor myactor)
    {
        MyActor = myactor;
        MyClass = ModuleClass.Active;
    }

    public Act CreateWalkAct(Vector2Int target)
    {
        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        tarlist.Add(this.MyActor);
        list.Add(new Walk(tarlist, WalkToPoint(target)));
        var ak = new Act()
        {
            MyType = Act.ActType.Movement,
            Effects = list,
            MyModule = this
        };
        list[0].ReceiveAct(ak);
        ak.ActName = "Move";
        return ak;
    }

    public Act ForceWalkAct(Vector2Int target)
    {
        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        tarlist.Add(this.MyActor);
        list.Add(new Walk(tarlist, ForceWalkToPoint(target)));
        var ak = new Act()
        {
            MyType = Act.ActType.Movement,
            Effects = list,
            MyModule = this
        };
        list[0].ReceiveAct(ak);
        return ak;
    }

    public Act DeathWalkAct(Vector2Int target)
    {
        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        tarlist.Add(this.MyActor);
        list.Add(new Walk(tarlist, ForceWalkToPoint(target)));
        var ak = new Act()
        {
            MyType = Act.ActType.Movement,
            Effects = list,
            MyModule = this
        };
        list[0].ReceiveAct(ak);
        return ak;
    }

    public Act CreateWalkAct(Actor target)
    {
        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        tarlist.Add(this.MyActor);
        list.Add(new Walk(tarlist, WalkToActor(target)));
        var ak = new Act()
        {
            MyType = Act.ActType.Movement,
            Effects = list,
            MyModule = this
        };
        list[0].ReceiveAct(ak);
        ak.ActName = "Move";
        return ak;
    }

    public List<Vector2Int> DeathWalkToPoint(Vector2Int target)
    {
        var range = 40;
        var list = MyActor.GameManager.GetValidTiles(MyActor.CurrentPosition, range, false, true);
        var nodes = new List<Node>();
        var start = new Node(999, MyActor.CurrentPosition);
        foreach (Vector2Int coord in list)
        {
            var nd = new Node(999, coord);
            nodes.Add(nd);
        }
        nodes.Add(start);
        var end = nodes.Find(a => a.Coordinates.x == target.x && a.Coordinates.y == target.y);
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
        nodes = nodes.OrderBy(x => x.Distance).ToList();
        path.Add(end);
        Node.BuildShortestPath(path, end);
        path.Reverse();
        foreach (Node item in path)
        {
            //
        }
        var nlist = new List<Vector2Int>();
        foreach (Node item in path)
        {
            nlist.Add(item.Coordinates);
        }
        return nlist;
    }

    public List<Vector2Int> ForceWalkToPoint(Vector2Int target)
    {
        var range = 20;
        var list = MyActor.GameManager.GetValidTiles(MyActor.CurrentPosition, range, false, false);
        var nodes = new List<Node>();
        var start = new Node(999, MyActor.CurrentPosition);
        foreach (Vector2Int coord in list)
        {
            var nd = new Node(999, coord);
            nodes.Add(nd);
        }
        nodes.Add(start);
        var end = nodes.Find(a => a.Coordinates.x == target.x && a.Coordinates.y == target.y);
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
        nodes = nodes.OrderBy(x => x.Distance).ToList();
        path.Add(end);
        Node.BuildShortestPath(path, end);
        path.Reverse();
        foreach (Node item in path)
        {
            //
        }
        var nlist = new List<Vector2Int>();
        foreach (Node item in path)
        {
            nlist.Add(item.Coordinates);
        }
        return nlist;
    }

    public List<Vector2Int> WalkToPoint(Vector2Int target)
    {
        var range = MyActor.TurnMoveCount;
        var list = MyActor.GameManager.GetValidTiles(MyActor.CurrentPosition, range, false, false);
        var nodes = new List<Node>();
        var start = new Node(999, MyActor.CurrentPosition);
        foreach (Vector2Int coord in list)
        {
            var nd = new Node(999, coord);
            nodes.Add(nd);
        }
        nodes.Add(start);
        var end = nodes.Find(a => a.Coordinates.x == target.x && a.Coordinates.y == target.y);
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
        nodes = nodes.OrderBy(x => x.Distance).ToList();
        path.Add(end);
        Node.BuildShortestPath(path, end);
        path.Reverse();
        foreach (Node item in path)
        {
//
        }
        var nlist = new List<Vector2Int>();
        foreach (Node item in path)
        {
            nlist.Add(item.Coordinates);
        }
        return nlist;
    }

    public List<Vector2Int> WalkToActor(Actor target)
    {
        var range = MyActor.TurnMoveCount;
        var list = MyActor.GameManager.GetValidTiles(MyActor.CurrentPosition, range, false, false);
        var nodes = new List<Node>();
        var start = new Node(999, MyActor.CurrentPosition);
        foreach(Vector2Int coord in list)
        {
            var nd = new Node(999, coord);
            nodes.Add(nd);
        }
        nodes.Add(start);
        foreach(Node nd in nodes)
        {
            if(list.Exists(x => x.x == nd.Coordinates.x+1 && x.y == nd.Coordinates.y))
            {
                nd.Adjacent.Add(nodes.Find(x => x.Coordinates.x == nd.Coordinates.x+1 && x.Coordinates.y == nd.Coordinates.y));
            }
            if (list.Exists(x => x.x == nd.Coordinates.x-1 && x.y == nd.Coordinates.y))
            {
                nd.Adjacent.Add(nodes.Find(x => x.Coordinates.x == nd.Coordinates.x-1 && x.Coordinates.y == nd.Coordinates.y));
            }
            if (list.Exists(x => x.x == nd.Coordinates.x && x.y == nd.Coordinates.y+1))
            {
                nd.Adjacent.Add(nodes.Find(x => x.Coordinates.x == nd.Coordinates.x && x.Coordinates.y == nd.Coordinates.y+1));
            }
            if (list.Exists(x => x.x == nd.Coordinates.x && x.y == nd.Coordinates.y-1))
            {
                nd.Adjacent.Add(nodes.Find(x => x.Coordinates.x == nd.Coordinates.x && x.Coordinates.y == nd.Coordinates.y-1));
            }
        }
        nodes.Remove(start);
        var path = new List<Node>();
        Node.DijkstraSearch(start);
        //find viable end node
        nodes.RemoveAll(x => x.Distance > range);
        nodes = nodes.OrderBy(x => x.Distance).ToList();
        Node targetnode = null;
       /* foreach(Node item in nodes)
        {
            if (item.Adjacent.Exists(x => x.Coordinates.x == target.CurrentPosition.x && x.Coordinates.y == target.CurrentPosition.y))
            {
                targetnode = item.Adjacent.Find(x => x.Coordinates.x == target.CurrentPosition.x && x.Coordinates.y == target.CurrentPosition.y);
                break;
            }
        }*/

        var thing = nodes.Find(a => a.Adjacent.Exists(b => b.Coordinates.x == target.CurrentPosition.x && b.Coordinates.y == target.CurrentPosition.y));
        if (thing != null)
        {
            targetnode = thing;
        }
        if(targetnode == null)
        {
            var newlist = new List<Node>(nodes).OrderBy(x => (x.Coordinates - target.CurrentPosition).magnitude).ToList();
            targetnode = newlist.First();
        }

        path.Add(targetnode);
        Node.BuildShortestPath(path, targetnode);
        path.Reverse();
        foreach (Node item in path)
        {
        }
        var nlist = new List<Vector2Int>();
        foreach (Node item in path)
        {
            nlist.Add(item.Coordinates);
        }
        return nlist;
    }
}



public class Node
{
    public int Distance;
    public Vector2Int Coordinates;
    public List<Node> Adjacent;
    public bool Visited;
    public Node NearestToStart;
    public int Cost;
    public Node(int dist, Vector2Int coord)
    {
        Distance = dist;
        Coordinates = coord;
        Adjacent = new List<Node>();
        NearestToStart = null;
        Visited = false;
        Cost = 1;
    }

    public static void DijkstraSearch(Node Start) {
        Start.Distance = 0;
        var prioQueue = new List<Node>();
        prioQueue.Add(Start);
        do
        {
            prioQueue = prioQueue.OrderBy(x => x.Distance).ToList();
            var node = prioQueue.First();
            prioQueue.Remove(node);
            foreach (Node cnn in node.Adjacent.OrderBy(x => x.Cost))
            {
                var childNode = cnn;
                if (childNode.Visited)
                    continue;
                if (childNode.Distance == 999 || node.Distance + cnn.Cost < childNode.Distance)
                {
                    childNode.Distance = node.Distance + cnn.Cost;
                    childNode.NearestToStart = node;
                    if (!prioQueue.Contains(childNode))
                        prioQueue.Add(childNode);
                }
            }
            node.Visited = true;
        } while (prioQueue.Any());
    }

    public static void BuildShortestPath(List<Node> list, Node node)
    {
        if (node.NearestToStart == null)
            return;
        list.Add(node.NearestToStart);
        BuildShortestPath(list, node.NearestToStart);
    }
}

public class TargetingModule : Module
{
    public enum TargetBehavior {WithinArea, AreaSample, RandomPoint};
    public enum AreaSelection {PointBlank, CenteredPoint };
    public enum SelectionShape {Diamond, Square };
    public TargetingModule(Actor myactor)
    {
        MyActor = myactor;
        MyClass = ModuleClass.Passive;
    }

    public TargetingData GenerateTargetList(TargetBehavior behavior, AreaSelection area, SelectionShape shape, Vector2Int target = new Vector2Int(), int targetcount = 20, int range = 1)
    {
        List<Actor> actors = new List<Actor>();
        List<Vector2Int> tiles = new List<Vector2Int>();

        List<Vector2Int> searchtiles = new List<Vector2Int>();
        List<Vector2Int> searchactors = new List<Vector2Int>();

        if(area == AreaSelection.PointBlank)
        {
            if(shape== SelectionShape.Diamond)
            {
                searchtiles = new List<Vector2Int>(MyActor.GameManager.GetValidTiles(MyActor.CurrentPosition, range, false, true));
            }
        }
        else if(area == AreaSelection.CenteredPoint)
        {
            if (shape == SelectionShape.Diamond)
            {
                searchtiles = new List<Vector2Int>(MyActor.GameManager.GetValidTiles(target, range, true, true));
            }
        }

        if (behavior == TargetBehavior.AreaSample)
        {
            tiles.Add(searchtiles[0]);
        }
        else if (behavior == TargetBehavior.WithinArea)
        {
            tiles = new List<Vector2Int>(searchtiles);
            foreach (Vector2Int item in tiles)
            {
                actors.Add(MyActor.GameManager.LevelActors.Find(x => x.CurrentPosition == item));
            }
        }

        var tar = new TargetingData();
        tar.Targets = actors;
        tar.Tiles = tiles;
        return tar;
    }


    public struct TargetingData
    {
        public List<Actor> Targets;
        public List<Vector2Int> Tiles;
    }

}

public class Summoning : Module
{
    public Summoning(Actor myactor)
    {
        MyClass = ModuleClass.Active;
        MyActor = myactor;
    }

    public Act CreateSummonAct(GameObject prefab, Vector2Int loc)
    {
        var list = new List<Effect>();
        var tarlist = new List<Actor>();
        list.Add(new CreateUnit(prefab, loc));
        tarlist.Add(this.MyActor);
        var ak = new Act()
        {
            MyModule = this,
            MyType = Act.ActType.Action,
            Effects = list,
            Targets =  tarlist
        };
        list[0].Targets = tarlist;
        list[0].ReceiveAct(ak);
        return ak;
    }
}



public class BasicAttacking : Module    
{
    public BasicAttacking(Actor myactor)
    {
        MyActor = myactor;
        MyClass = ModuleClass.Active;
    }

    public bool CheckWithinRange(Actor target)
    {
        return (target.CurrentPosition - MyActor.CurrentPosition).magnitude <= MyActor.MyProfile.GetStat(ActorStat.StatType.Range);
    }

    public Act CreateAttackAct(Actor target)
    {
        var list = new List<Effect>();
        var dude = new List<Actor>();
        dude.Add(target);
        list.Add(new SpawnSpeechBubble(MyActor, MyActor.ReturnInsult()));
        list.Add(new DealDamage(dude));
        var ak = new Act()
        {
            MyModule = this,
            MyType = Act.ActType.Action,
            Effects = list,
            Targets = dude
        };
        list[1].ReceiveAct(ak);
        ak.ActName = "Basic Attack";
        return ak;
    }
}




