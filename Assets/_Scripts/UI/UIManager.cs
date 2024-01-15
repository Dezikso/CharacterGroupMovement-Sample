using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static Action<int> OnLeaderSet;
    public static Action<int> OnThemeSet;
    public static Action OnLoadClicked;
    public static Action OnSaveClicked;

    
    public void LeaderSelectButton(int val)
    {
        OnLeaderSet?.Invoke(val);
    }

    public void ThemeSelectButton(int val) 
    { 
        OnThemeSet?.Invoke(val);
    }

    public void LoadButton()
    {
        OnLoadClicked?.Invoke();
    }    
    public void SaveButton()
    {
        OnSaveClicked?.Invoke();
    }

}
