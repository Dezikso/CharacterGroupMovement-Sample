using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance {  get; private set; }

    [SerializeField] private string fileName;

    private List<IDataHandler> dataHandlers;
    private FileDataHandler fileDataHandler;
    private GameData gameData;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }

    private void OnEnable()
    {
        UIManager.OnLoadClicked += LoadData;
        UIManager.OnSaveClicked += SaveData;
    }

    private void OnDisable()
    {
        UIManager.OnLoadClicked -= LoadData;
        UIManager.OnSaveClicked -= SaveData;
    }

    private void Start()
    {
        gameData = new GameData();
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandlers = FindAllDataHandlers();
    }


    private void LoadData()
    {
        gameData = fileDataHandler.Load();

        if (gameData == null)
        {
            return;
        }

        foreach (IDataHandler handler in dataHandlers)
        {
            handler.LoadData(gameData);
        }
    }

    private void SaveData()
    {
        foreach (IDataHandler handler in dataHandlers)
        {
            handler.SaveData(gameData);
        }
        fileDataHandler.Save(gameData);
    }

    private List<IDataHandler> FindAllDataHandlers()
    {
        IEnumerable<IDataHandler> _dataHandlers = FindObjectsOfType<MonoBehaviour>().OfType<IDataHandler>();

        return new List<IDataHandler>(_dataHandlers);
    }

}
