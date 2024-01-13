using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static Action<int> OnLeaderSet;

    
    public void OnUnitSelectButton(int val)
    {
        OnLeaderSet?.Invoke(val);
    }

}
