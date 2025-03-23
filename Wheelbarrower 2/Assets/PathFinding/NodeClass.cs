using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeClass
{
    public Vector2Int coordinates;
    public bool isWalkable;
    public bool isExplode;
    public bool isPath;
    public NodeClass connectedTo;

    public NodeClass(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}
