using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Sources:
/// http://amalgamatelabs.com/Blog/4/data_persistence
/// https://github.com/RITGameDev/GameSaveDemo/blob/master/Assets/Scripts/GameSave.cs
/// </summary>

public class GameSaver : Global
{
    /// <summary>
    /// Handles the saving and loading of gameData
    /// </summary>
    
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);

    private const string FolderName = "/SaveFile0/";
    private const string GameSaveFileName = "GameSaveData";
    private const string FileExtension = ".dat";

    public static GameSaveData gameData = null;

    public static string CurrentLevelName = "Fall"; //name of the level player is currently on


    public static void SaveGameData()
    {
        Jump player = Player.GetComponent<Jump>();

        //save the game data
        GameSaveData saveData = new GameSaveData(
            CurrentLevelName,
            player.transform.position,
            player.ReturnPosition,
            player.Health,
            Heartbeat.BPM
        );

        SaveGameData(saveData);
    }

    /// <summary>
    /// Saves the game save data
    /// </summary>
    /// <param name="saveData">Data to save</param>
    public static void SaveGameData(GameSaveData saveData)
    {
        MakeDirectory();

        string dataPath = Application.persistentDataPath + FolderName + GameSaveFileName + FileExtension;
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

    public static void SaveLevelData(LevelSaveData saveData)
    {
        MakeDirectory();
        string dataPath = Application.persistentDataPath + FolderName + saveData.levelName + FileExtension;

        Debug.Log(dataPath);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate);

        try
        {
            binaryFormatter.Serialize(fileStream, saveData);
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                SyncFiles();
            }

        }
        catch (System.Exception e)
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
        string dataPath = Application.persistentDataPath + FolderName + GameSaveFileName + FileExtension;

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
                PlatformSafeMessage("Game Deserialization Failed: " + e.Message);
            }
            finally
            {
                //always close the fileStream
                fileStream.Close();
            }
        }
        gameData = saveData;
        return saveData;
    }

    public static LevelSaveData LoadLevelData(string levelName)
    {
        LevelSaveData saveData = null;
        string dataPath = Application.persistentDataPath + FolderName + levelName + FileExtension;

        //make sure the file actually exists
        if (File.Exists(dataPath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(dataPath, FileMode.Open);

            try
            {
                saveData = (LevelSaveData)binaryFormatter.Deserialize(fileStream);
            }
            catch (System.Exception e)
            {
                PlatformSafeMessage("Level Deserialization Failed: " + e.Message);
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

    private static void MakeDirectory()
    {
        string filePath = Application.persistentDataPath + FolderName;
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
    }
}
