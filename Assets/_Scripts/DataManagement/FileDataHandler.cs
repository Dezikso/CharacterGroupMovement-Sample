using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirectoryPath;
    private string dataFileName;
    

    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {  
        this.dataDirectoryPath = dataDirectoryPath; 
        this.dataFileName = dataFileName; 
    }


    public GameData Load()
    {
        string savePath = Path.Combine(dataDirectoryPath, dataFileName);
        GameData data = null;
        if (File.Exists(savePath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(savePath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                data = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured while loading data from a file: " + e);
            }
        }
        return data;
    }

    public void Save(GameData data)
    {
        string savePath = Path.Combine(dataDirectoryPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            string dataToStore = JsonUtility.ToJson(data);

            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured while saving data to a file: " + e);
        }
    }

}
