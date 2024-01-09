using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavigationGrid))]
public class Pathfinding : MonoBehaviour
{
    public static Action<List<Node>> OnPathFound;

    [SerializeField] private Transform seeker; //change to private later
    [SerializeField] private LayerMask walkableMask;

    private NavigationGrid grid;


    private void Awake()
    {
        grid = GetComponent<NavigationGrid>();

        //UnitManager.OnLeaderChanged += SetSeeker;
        InputManager.OnPress += SetTargetToScreenPoint;
    }

    private void OnDestroy()
    {
        //UnitManager.OnLeaderChanged -= SetSeeker;
        InputManager.OnPress -= SetTargetToScreenPoint;
    }

    private void SetSeeker(Transform leader)
    {
        seeker = leader;
    }

    private void SetTargetToScreenPoint(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, walkableMask))
        {
            Node clickedNode = grid.GetNodeFromWorldPoint(hit.point);
            
            FindPath(seeker.position, clickedNode.worldPosition);
        }
    }

    private void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.GetNodeFromWorldPoint(startPosition);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPosition);

        if (startNode == targetNode) { return; }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node node = FindLowestFCostNode(openSet);

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in node.neighbours)
            {
                if (closedSet.Contains(neighbour)) { continue; }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    private Node FindLowestFCostNode(List<Node> openSet)
    {
        Node lowestCostNode = openSet[0];
        foreach (Node node in openSet)
        {
            if (node.fCost < lowestCostNode.fCost || (node.fCost == lowestCostNode.fCost && node.hCost < lowestCostNode.hCost))
            {
                lowestCostNode = node;
            }
        }

        return lowestCostNode;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distanceX > distanceY)
        {
            return (14 * distanceY) + (10 * (distanceX - distanceY));
        }
        else
        {
            return (14 * distanceX) + (10 * (distanceY - distanceX));
        }
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        OnPathFound?.Invoke(path);

#if UNITY_EDITOR
        grid.SetPathToVisualise(path);
#endif

    }

}
