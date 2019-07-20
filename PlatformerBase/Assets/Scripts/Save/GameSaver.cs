using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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

    private static string folderName = "/SaveFile0/";
    private const string GameSaveFileName = "GameSaveData";
    private const string FileExtension = ".dat";

    public static GameSaveData gameData = null;

    public static string currentLevelName = "Fall"; //name of the level player is currently on

    public static int FolderNumber { set { folderName = "/SaveFile" + (value - 1) + (Web ? "_" : "/"); } }
    public static string SaveName { get { return Application.persistentDataPath + folderName + GameSaveFileName + FileExtension; } }

    public static IEnumerable<ILevelData> levelDataObjects;

    private static bool Web { get { return Application.platform == RuntimePlatform.WebGLPlayer; } }

    /// <summary>
    /// General Game Saving
    /// </summary>
    public static bool SaveGameData()
    {
        Jump player = Player.GetComponent<Jump>();

        //save the game data
        GameSaveData saveData = new GameSaveData(
            currentLevelName,
            player.transform.position + (player.InFallZone ? Vector3.up : Vector3.zero),
            player.ReturnPosition,
            player.Health,
            Heartbeat.BPM,
            AbilityHandler.Unlocked,
            Inventory.ItemStates,
            Inventory.GemLock
        );

        return SaveGameData(saveData);
    }

    /// <summary>
    /// Saves the game save data
    /// </summary>
    /// <param name="saveData">Data to save</param>
    private static bool SaveGameData(GameSaveData saveData)
    {
        if (!Web) { MakeDirectory(); }

        string dataPath = Application.persistentDataPath + folderName + GameSaveFileName + FileExtension;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate);

        bool success = false;
        try
        {
            binaryFormatter.Serialize(fileStream, saveData);
            if (Web) { SyncFiles(); }
            success = true;
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
        return success;
    }

    public static bool SaveLevelData()
    {
        //create a new level save object
        LevelSaveData levelData = new LevelSaveData(currentLevelName);

        //add the states data of all the level data objects 
        foreach (ILevelData data in levelDataObjects)
        {
            levelData.AddObject(data.Name, data.State);
        }

        BreakableTilemap breakable = FindObjectOfType<BreakableTilemap>();
        //add the breakable tilemap data
        if (breakable)
        {
            foreach (Vector2Int broken in breakable.Broken)
            {
                levelData.AddBroken(broken);
            }
        }
        //save the level data
        return SaveLevelData(levelData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="saveData"></param>
    public static bool SaveLevelData(LevelSaveData saveData)
    {
        string dataPath = Application.persistentDataPath + folderName + saveData.levelName + FileExtension;

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Open(dataPath, FileMode.OpenOrCreate);

        bool success = false;
        try
        {
            binaryFormatter.Serialize(fileStream, saveData);
            if (Web) { SyncFiles(); }
            success = true;
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
        return success;
    }

    /// <summary>
    /// Loads in the game save data
    /// </summary>
    /// <returns>Deserialized Game Data</returns>
    public static GameSaveData LoadGameData(int f)
    {
        FolderNumber = f;
        GameSaveData saveData = null;
        string dataPath = Application.persistentDataPath + folderName + GameSaveFileName + FileExtension;

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
        string dataPath = Application.persistentDataPath + folderName + levelName + FileExtension;

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
        string filePath = Application.persistentDataPath + folderName;
        //make a new file folder if not a web build
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
    }

    public static void RemoveDirectory()
    {
        string filePath = Application.persistentDataPath + folderName;

        if(Web)
        {
            //loop through all the possible files names and delete them
            foreach(string file in new string[] { GameSaveFileName, "Fall", "Garden", "Basophil", "Wilds", "Shrine", "Sanctuary", "Graveyard", "Island"})
            {
                string fileName = filePath + file + FileExtension;
                if (File.Exists(fileName))
                {
                    PlatformSafeMessage("file found: " + file);
                    File.Delete(fileName);
                }
            }
            if(File.Exists(filePath))
            {
                PlatformSafeMessage("file found: " + filePath);
                File.Delete(filePath);
            }
        }
        else if (Directory.Exists(filePath))
        {
            //remove the file folder
            Directory.Delete(filePath, true);
        }
    }
}
