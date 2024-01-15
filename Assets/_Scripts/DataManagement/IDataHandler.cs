using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataHandler
{
    public void LoadData(GameData data);
    
    public void SaveData(GameData data);

}
