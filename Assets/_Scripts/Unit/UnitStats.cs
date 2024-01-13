using UnityEngine;

[System.Serializable]
public class UnitStats
{
    public float health;
    public float speed;
    public float agility;


    public UnitStats(UnitStatsRangesSO statsRanges)
    {
        this.health = Random.Range(statsRanges.minHealth, statsRanges.maxHealth);
        this.speed =  Random.Range(statsRanges.minSpeed, statsRanges.maxSpeed);
        this.agility = Random.Range(statsRanges.minAgility, statsRanges.maxAgility);
    }

}
