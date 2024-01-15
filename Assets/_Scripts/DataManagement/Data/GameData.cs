using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int themeID;
    public int leaderID;
    public SerializableDictionary<string, UnitStats> units;

    public GameData() 
    { 
        this.themeID = 0;
        this.leaderID = 0;
        units = new SerializableDictionary<string, UnitStats>();
    }

}
