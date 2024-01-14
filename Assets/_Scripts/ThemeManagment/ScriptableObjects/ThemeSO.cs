using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "NewTheme", menuName = "Themes/Theme")]
public class ThemeSO : ScriptableObject
{
    public AssetReferenceGameObject groundAsset;
    public AssetReferenceGameObject obstaclesAsset;
    public Material skyboxMaterial;


}
