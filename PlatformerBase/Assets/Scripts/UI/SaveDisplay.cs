using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveDisplay : MonoBehaviour
{
    [Header("Setup Variables")]
    [SerializeField] [Range(1, 3)] public int saveNumber;
    [SerializeField] private Vector2 buttonLocationOneOffset;
    [SerializeField] private Vector2 buttonLocationTwoOffset;
    [SerializeField] private Vector2 saveNumberLabelOffset;
    [SerializeField] private Vector2 nameOutputLabelOffset;

    private string folderName = "/SaveFileX/";

    [Space(5)]
    [Header("References too child objects")]
    [SerializeField] private Transform newButton;
    [SerializeField] private Transform playButton;
    [SerializeField] private Transform deleteButton;
    [SerializeField] private Transform saveNumberLabel;
    [SerializeField] private DialogueBox saveLevelNameBox;


    // Start is called before the first frame update
    void Awake()
    {
        //put the objects in their place
        newButton.localPosition = buttonLocationOneOffset;
        playButton.localPosition = buttonLocationOneOffset;
        deleteButton.localPosition = buttonLocationTwoOffset;
        saveNumberLabel.localPosition = saveNumberLabelOffset;
        saveLevelNameBox.transform.localPosition = nameOutputLabelOffset;
    }

    private void Start()
    {
        Refresh(); //set the save file data
    }
    
    /// <summary>
    /// called when the save file dat needs updating
    /// </summary>
    public void Refresh()
    {
        saveLevelNameBox.Reset();

        folderName = "/SaveFile" + (saveNumber - 1).ToString() + "/"; //get the folder name
        //check if the file already exists
        string filePath = Application.persistentDataPath + folderName;
        if (Directory.Exists(filePath))
        {
            //File Exists
            newButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
            deleteButton.gameObject.SetActive(true);

            //get the name of the level saved on
            GameSaveData saveData = GameSaver.LoadGameData(saveNumber);
            saveLevelNameBox.OnTriggerKeyPressed(saveData.currentLevel);
        }
        else
        {
            //File doesn't exist
            newButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);

            saveLevelNameBox.OnTriggerKeyPressed("---");
        }
    }

}
