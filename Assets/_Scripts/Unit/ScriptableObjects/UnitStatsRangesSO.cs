using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatsRange", menuName = "Stats/StatsRange")]
public class UnitStatsRangesSO : ScriptableObject
{
    public float minHealth, maxHealth;
    public float minSpeed, maxSpeed;
    public float minAgility, maxAgility;
    
}
