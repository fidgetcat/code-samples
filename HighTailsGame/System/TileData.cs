using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileGrid
{
   public enum Direction {N, E, S, W};
    public int size;
    public TileStack[,] MyGrid;
    public ActorGrid MyActorGrid;
    public OrnamentStack[,] MyOrnaments;

    public bool Exists(Vector2Int coords)
    {
        if (coords.x > size-1 || coords.x < 0)
        {
            return false;
        }
        else if(coords.y > size-1 || coords.y < 0)
        {
            return false;
        }
        else if(coords.x > MyGrid.GetLength(0) - 1)
        {
            return false;
        }
        else if(coords.y > MyGrid.GetLength(1) - 1)
        {
            return false;
        }
        else if (MyGrid[coords.x, coords.y].MyStack.Count <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

public class TileGridReference
{
    public int size;
    public TileStackReference[,] MyGrid;
}

public class TileStackReference
{
    public List<GameObject> Blocks;
    public Vector2 pos;

    public Transform GetTopBlockTransform()
    {
        return Blocks[Blocks.Count - 1].transform;
    }

    public Vector3 GetWalkablePoint()
    {
        float totalheight = 0f;
        Vector3 output = new Vector3(Blocks[0].transform.position.x, 0f, Blocks[0].transform.position.z);
        foreach(GameObject item in Blocks) {
            totalheight += item.transform.localScale.y;
        }
        totalheight += 0.3f;
        output.y = totalheight;
        return output;
    }
}


[System.Serializable]
public class ActorGrid
{
    public int size;
    public ActorData[,] MyGrid;

    public ActorGrid(int gridsize)
    {
        size = gridsize;
        MyGrid = new ActorData[size, size];
    }
}

[System.Serializable]
public class ActorData
{
    public List<string> PrefabName;
    public List<TileGrid.Direction> DefaultDirection;

    public ActorData()
    {
        PrefabName = new List<string>();
        DefaultDirection = new List<TileGrid.Direction>();
    }
}

[System.Serializable]
public class OrnamentStack
{
    public int X;
    public int Y;
    public List<OrnamentData> MyOrnaments;

    public OrnamentStack()
    {
        MyOrnaments = new List<OrnamentData>();
    }

    public OrnamentStack(int x, int y)
    {
        X = x;
        Y = y;
        MyOrnaments = new List<OrnamentData>();
    }
}

[System.Serializable]
public class OrnamentData
{
    public string PrefabName;

    public VectorWrapper LocalTransform;

    public VectorWrapper LocalRotation;

    public VectorWrapper LocalScale;

    public OrnamentData()
    {
        LocalRotation = new VectorWrapper();
        LocalTransform = new VectorWrapper();
        LocalScale = new VectorWrapper();
    }

    public OrnamentData(string input)
    {
        PrefabName = input;
        LocalRotation = new VectorWrapper();
        LocalTransform = new VectorWrapper();
        LocalScale = new VectorWrapper();
    }
}

[System.Serializable]
public class VectorWrapper
{
    public float X;
    public float Y;
    public float Z;

    public VectorWrapper()
    {
        X = 0f;
        Y = 0f;
        Z = 0f;
    }

    public VectorWrapper(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

[System.Serializable]
public class TileStack
{
    public List<BlockTile> MyStack;
    public int X;
    public int Y;
    public bool IsSpawn;
    public bool IsObstruction;
}

[System.Serializable]
public class BlockTile
{
    public string TileName;
    public float Height;
    public BlockTile(string name, float height)
    {
        TileName = name;
        Height = height;
    }
}
