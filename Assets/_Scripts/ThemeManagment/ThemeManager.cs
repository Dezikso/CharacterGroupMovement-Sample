using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public class ThemeManager : MonoBehaviour, IDataHandler
{
    [SerializeField] private List<ThemeSO> themes;
    [SerializeField] private Transform groundParentTransform;
    [SerializeField] private Transform obstaclesParentTransform;

    private int activeThemeIndex;

    private AsyncOperationHandle<GameObject> groundHandle;
    private AsyncOperationHandle<GameObject> obstaclesHandle;


    private void Start()
    {
        ChangeTheme(0);
    }

    public void LoadData(GameData data)
    {
        if (data.themeID == activeThemeIndex)
            return;
            
        ChangeTheme(data.themeID);
    }

    public void SaveData(GameData data)
    {
        data.themeID = activeThemeIndex;
    }


    public void ChangeTheme(int themeIndex)
    {
        if (themeIndex < 0 || themeIndex >= themes.Count || themeIndex == activeThemeIndex)
            return;

        UnloadPreviousTheme();

        activeThemeIndex = themeIndex;
        ThemeSO selectedTheme = themes[themeIndex];
        LoadThemeAsync(selectedTheme);
        LoadSkybox(selectedTheme.skyboxMaterial);
    }

    private async void LoadThemeAsync(ThemeSO theme)
    {
        groundHandle = Addressables.LoadAssetAsync<GameObject>(theme.groundAsset);
        obstaclesHandle = Addressables.LoadAssetAsync<GameObject>(theme.obstaclesAsset);

        await Task.WhenAll(groundHandle.Task, obstaclesHandle.Task);

        Instantiate(groundHandle.Result, groundParentTransform);
        Instantiate(obstaclesHandle.Result, obstaclesParentTransform);
    }

    private void LoadSkybox(Material newSkyboxMaterial)
    {
        RenderSettings.skybox = newSkyboxMaterial;
    }

    private void UnloadPreviousTheme()
    {
        UnloadGround();
        UnloadObstacles();
    }

    private void UnloadGround()
    {
        foreach (Transform child in groundParentTransform)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }

        if (groundHandle.IsValid())
        {
            Addressables.Release(groundHandle);
        }
    }

    private void UnloadObstacles()
    {
        foreach (Transform child in obstaclesParentTransform)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }

        if (obstaclesHandle.IsValid())
        {
            Addressables.Release(obstaclesHandle);
        }
    }

}
