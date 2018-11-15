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
        Player.GetComponent<IHealthObject>().FullHeal(); //fully heal the player
        Player.GetComponent<IPlayer>().Power.Heartbeat.BPM = -1; //set the BPM to default value to be set in first scene
        Items.Clear();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        startPosition = Vector2.one;
    }
}
