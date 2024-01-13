using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitStatsRangesSO statsRanges;
    [SerializeField] private int unitID;
    [SerializeField] private GameObject leaderIndicator;

    private UnitStats stats;
    private Coroutine activeMoveCoroutine;

    public int UnitID { get { return unitID; } }
    public float MoveSpeed { get { return stats.speed; } }

    private void Awake()
    {
        stats = new UnitStats(statsRanges);
    }


    public void SetLeaderIndicator(bool isLeader)
    {
        leaderIndicator.SetActive(isLeader);
    }

    public void Move(List<Node> path, float moveSpeed)
    {
        if (activeMoveCoroutine != null)
        {
            StopCoroutine(activeMoveCoroutine);
        }

        activeMoveCoroutine = StartCoroutine(MoveCoroutine(path, moveSpeed));
    }

    private IEnumerator MoveCoroutine(List<Node> path, float moveSpeed)
    {
        foreach (Node node in path)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = node.worldPosition + new Vector3(0f, transform.position.y, 0f);
            float moveProgress = 0;

            transform.LookAt(targetPosition);

            while (moveProgress < 1)
            {
                moveProgress += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);
                yield return new WaitForEndOfFrame();
            }
        }

        activeMoveCoroutine = null;
    }

}
