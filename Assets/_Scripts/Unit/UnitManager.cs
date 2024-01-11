using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<Unit> units;
    [SerializeField] private int unitSpacing = 2;
    [SerializeField] private float unitMoveDelay = 0.5f;

    [SerializeField] private int leaderIndex; //make private later
    private Coroutine activeMoveUnitsCoroutine;



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
        if (activeMoveUnitsCoroutine != null)
        {
            StopCoroutine(activeMoveUnitsCoroutine);
        }

        activeMoveUnitsCoroutine = StartCoroutine(MoveUnitsCoroutine(path));
    } 

    private IEnumerator MoveUnitsCoroutine(List<Node> path)
    {
        if (path.Count <= units.Count + 1)
        {
            yield break;
        }

        Unit leader = units[leaderIndex];
        leader.Move(new List<Node>(path), leader.MoveSpeed);

        yield return new WaitForSeconds(unitMoveDelay);

        for (int i = 0; i < units.Count; i++)
        {
            if (i == leaderIndex)
                continue;

            if (unitSpacing > 0 && unitSpacing <= path.Count)
            {
                path.RemoveRange(path.Count - unitSpacing, unitSpacing);
            }
            
            units[i].Move(new List<Node>(path), leader.MoveSpeed);

            yield return new WaitForSeconds(unitMoveDelay);
        }

        activeMoveUnitsCoroutine = null;
    }

}
