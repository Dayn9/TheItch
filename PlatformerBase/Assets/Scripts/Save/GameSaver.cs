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

    public static int FolderNumber { set { folderName = "/SaveFile" + (value - 1)+ "/"; } }

    /// <summary>
    /// General Game Saving
    /// </summary>
    public static void SaveGameData()
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
            Inventory.ItemStates
        );

        SaveGameData(saveData);
    }

    /// <summary>
    /// Saves the game save data
    /// </summary>
    /// <param name="saveData">Data to save</param>
    private static void SaveGameData(GameSaveData saveData)
    {
        MakeDirectory();

        string dataPath = Application.persistentDataPath + folderName + GameSaveFileName + FileExtension;
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


    public static void SaveLevelData()
    {
        LevelSaveData levelData = new LevelSaveData(currentLevelName);

        
        //add the states data of all the level data objects 
        foreach (ILevelData data in FindObjectsOfType<MonoBehaviour>().OfType<ILevelData>())
        {
            levelData.AddObject(data.Name, data.State);
        }

        SaveLevelData(levelData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="saveData"></param>
    public static void SaveLevelData(LevelSaveData saveData)
    {
        MakeDirectory();
        string dataPath = Application.persistentDataPath + folderName + saveData.levelName + FileExtension;

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
        if (!Directory.Exists(filePath) && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            Directory.CreateDirectory(filePath);
        }
    }

    public static void RemoveDirectory()
    {
        string filePath = Application.persistentDataPath + folderName;

        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            //loop through all the possible files names and delete them
            foreach(string file in new string[] { "GameSaveDat", "Fall", "Garden", "Basophil", "Wilds", "Shrine", "Climb"})
            {
                string fileName = filePath + file + FileExtension;
                if (Directory.Exists(fileName))
                {
                    Directory.Delete(fileName, true);
                }
            }
        }
        else if (Directory.Exists(filePath))
        {
            //remove the file folder
            Directory.Delete(filePath, true);
        }
    }
}
