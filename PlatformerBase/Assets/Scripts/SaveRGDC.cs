using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //yikes

public class SaveRGDC : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SavePrefs();
	}

    public void LoadPrefs() 
    {
        if (PlayerPrefs.HasKey("INT_KEY"))
        {
            PlayerPrefs.GetInt("INT_KEY");
        }
        
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("INT_KEY", 25);
        PlayerPrefs.Save();
    }

    public void LoadBinary()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/gameData.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

        }
    }

    
    public void SaveBinary()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.OpenOrCreate);

        GameData data = new GameData();
        data.flow = 12.726f;
        data.integer = 12;
        data.boolean = false;

        bf.Serialize(file, data);

        file.Close();
    }
}

[System.Serializable]
public class GameData
{
    public float flow;
    public int integer;
    public bool boolean;
}