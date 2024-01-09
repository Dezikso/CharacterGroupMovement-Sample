using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationGrid : MonoBehaviour
{
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius;

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;


    private void Awake()
    {
        InitializeGridValues();
        CreateGrid();
    }

    private void InitializeGridValues()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.forward * gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft
                    + Vector3.right * (x * nodeDiameter + nodeRadius)
                    + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool isWalkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);

                grid[x,y] = new Node(isWalkable, worldPoint, x, y);
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0;y < gridSizeY; y++)
            {
                grid[x,y].neighbours = CalculateNeighbours(x, y);
            }
        }
    }

    private List<Node> CalculateNeighbours(int x, int y)
    {
        List<Node> neighbours = new List<Node>();

        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                if (xOffset ==0 && yOffset == 0) 
                    continue;
                
                int checkX = x + xOffset;
                int checkY = y + yOffset;

                if (IsWithinBounds(checkX, checkY))
                {
                    Node neighbour = grid[checkX, checkY];
                    if (neighbour.isWalkable)
                    {
                        neighbours.Add(neighbour);
                    }
                }
            }
        }

        return neighbours;
    }

    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < gridSizeX && y >=0 && y < gridSizeY;
    }

    public Node GetNodeFromWorldPoint(Vector3 worldPoint)
    {
        Vector3 localPosition = worldPoint - transform.position;

        float percentX = Mathf.Clamp01(localPosition.x / gridWorldSize.x + 0.5f);
        float percentY = Mathf.Clamp01(localPosition.z / gridWorldSize.y + 0.5f);

        int x = Mathf.Clamp(Mathf.FloorToInt(percentX * gridSizeX), 0, gridSizeX - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt(percentY * gridSizeY), 0, gridSizeY - 1);

        return grid[x, y];
    }

    #region Editor Visualization
    private List<Node> path;

    public void SetPathToVisualise(List<Node> _path)
    {
        path = _path;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.isWalkable ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
    #endregion


}
