using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitStatsRangesSO statsRanges;
    [SerializeField] private int unitID;

    private UnitStats stats;


    private void Awake()
    {
        stats = new UnitStats(statsRanges);
    }


    public void Move(List<Node> path)
    {
        StartCoroutine(MoveCooroutine(path));
    }

    private IEnumerator MoveCooroutine(List<Node> path)
    {
        foreach (Node node in path)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = node.worldPosition + new Vector3(0f, transform.position.y, 0f);
            float moveProgress = 0;

            transform.LookAt(targetPosition);

            while (moveProgress < 1)
            {
                moveProgress += Time.deltaTime * stats.speed;
                transform.position = Vector3.Lerp(startPosition, targetPosition, moveProgress);
                yield return new WaitForEndOfFrame();
            }
        }
    }

}
