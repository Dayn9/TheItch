using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Sources:
/// http://amalgamatelabs.com/Blog/4/data_persistence
/// https://github.com/RITGameDev/GameSaveDemo/blob/master/Assets/Scripts/GameSave.cs
/// </summary>

public class GameSaver : MonoBehaviour
{
    /// <summary>
    /// Handles the saving and loading of gameData
    /// </summary>
    
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);

    private const string SaveFileName = "/GameSaveData.dat";
    private string[] levelLookup = new string[] {
            "Fall", //0
            "Garden", //1
            "Basophil", //2
            "Shrine", //3
            "Wilds", //4
            "Climb",//5
            "Graveyard",//6
            "Island"//7
    };

/// <summary>
/// Saves the game save data
/// </summary>
/// <param name="saveData">Data to save</param>
public static void SaveGameData(GameSaveData saveData)
    {
        string dataPath = Application.persistentDataPath + SaveFileName;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate);

        try
        {
            binaryFormatter.Serialize(fileStream, saveData);
            if(Application.platform == RuntimePlatform.WebGLPlayer)
            {
                SyncFiles();
            }

        }
        catch(System.Exception e)
        {
            PlatformSafeMessage("Serialization Failed: " + e.Message);
        }
        finally
        {
            //always close the fileStream
            fileStream.Close();
        }
    }

    /// <summary>
    /// Loads in the game save data
    /// </summary>
    /// <returns>Deserialized Game Data</returns>
    public static GameSaveData LoadGameData()
    {
        GameSaveData saveData = null;
        string dataPath = Application.persistentDataPath + SaveFileName;

        //make sure the file actually exists
        if (File.Exists(dataPath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(dataPath, FileMode.Open);

            
            try
            {
                saveData = (GameSaveData)binaryFormatter.Deserialize(fileStream);
            }
            catch(System.Exception e)
            {
                PlatformSafeMessage("Deserialization Failed: " + e.Message);
            }
            finally
            {
                //always close the fileStream
                fileStream.Close();
            }
        }

        return saveData;
    }

    /// <summary>
    /// logs a message based on the platform
    /// </summary>
    /// <param name="message">message to display</param>
    private static void PlatformSafeMessage(string message)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            WindowAlert(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public string LevelNumToName(int levelNum)
    {
        return levelLookup[levelNum];
    }

    public int LevelNameToNum(string levelName)
    {
        for(int i = 0; i < levelLookup.Length; i++)
        {
            if(levelLookup[i] == levelName) { return i; }
        }
        return 0;
    }

}
