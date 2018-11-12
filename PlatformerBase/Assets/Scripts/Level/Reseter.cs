using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reseter : Inventory {

    /// <summary>
    /// in charge of reseting the game when the player dies 
    /// </summary>
    
    public void ResetGame()
    {
        Items.Clear();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
