using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isWalkable;
    public Vector3 worldPosition;
    public int gridX, gridY;

    public List<Node> neighbours;
    public Node parent;

    public int gCost;
    public int hCost;
    public int fCost { get { return gCost + hCost; } }


    public Node(bool _isWalkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        isWalkable = _isWalkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;

        neighbours = new List<Node>();
    }

}
