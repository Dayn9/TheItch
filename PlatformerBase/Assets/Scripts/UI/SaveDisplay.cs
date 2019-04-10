using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveDisplay : MonoBehaviour
{
    [Header("Setup Variables")]
    [SerializeField] [Range(1, 3)] private int saveNumber;
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

        folderName = "/SaveFile" + (saveNumber - 1).ToString() + "/"; //get the folder name
        //check if the file already exists
        string filePath = Application.persistentDataPath + folderName;
        if (Directory.Exists(filePath))
        {
            //File Exists
            newButton.gameObject.SetActive(false);

            //get the name of the level saved on
            GameSaveData saveData = GameSaver.LoadGameData();
            saveLevelNameBox.OnTriggerKeyPressed(saveData.currentLevel);
        }
        else
        {
            Debug.Log("no file");

            //File doesn't exist
            playButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
