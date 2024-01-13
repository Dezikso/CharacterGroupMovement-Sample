using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static Action<Unit> OnLeaderChanged;

    [SerializeField] private int defaultLeaderIndex = 0;
    [SerializeField] private List<Unit> units;
    [SerializeField] private int unitSpacing = 2;
    [SerializeField] private float unitMoveDelay = 0.5f;

    private int leaderIndex;
    private Coroutine activeMoveUnitsCoroutine;



    private void OnEnable()
    {
        UIManager.OnLeaderSet += SetLeader;
        Pathfinding.OnPathFound += MoveUnits;
    }
    private void OnDisable()
    {
        UIManager.OnLeaderSet -= SetLeader;
        Pathfinding.OnPathFound -= MoveUnits;
    }

    private void Start()
    {
        SetLeader(defaultLeaderIndex);
    }


    private void SetLeader(int leaderID)
    {
        leaderIndex = leaderID;
        SetLeaderIndicator();
        OnLeaderChanged?.Invoke(units[leaderIndex]);
    }

    private void SetLeaderIndicator()
    {
        foreach (Unit unit in units) 
        {
            unit.SetLeaderIndicator(false);
        }
        units[leaderIndex].SetLeaderIndicator(true);
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
