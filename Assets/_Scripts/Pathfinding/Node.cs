using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isWalkable;
    public int gridX, gridY;
    public Vector3 worldPosition;

    public List<Node> neighbours;
    public Node parent;

    public int gCost;
    public int hCost;

    public int fCost { get { return gCost + hCost; } }


    public Node(bool isWalkable, int gridX, int gridY, Vector3 worldPosition)
    {
        this.isWalkable = isWalkable;
        this.gridX = gridX;
        this.gridY = gridY;
        this.worldPosition = worldPosition;

        neighbours = new List<Node>();
    }

}
