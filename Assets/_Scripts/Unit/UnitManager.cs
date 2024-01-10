using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<Unit> units;

    private int leaderIndex;


    private void OnEnable()
    {
        Pathfinding.OnPathFound += MoveUnits;
    }
    private void OnDisable()
    {
        Pathfinding.OnPathFound -= MoveUnits;
    }

    private void Start()
    {
        leaderIndex = 0;
    }


    private void MoveUnits(List<Node> path)
    {
        units[leaderIndex].Move(path);
    }

}
