using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Keeps track of common global variables that objects need access to
/// </summary>
public class Global : MonoBehaviour {

    #region global variables
    private static GameObject mainCamera; //Main Camera object in scene
    private static GameObject player; //player object

    protected const int pixelsPerUnit = 8; //number of pixels displayed in each unity unit
    protected const float buffer = 0.01f; //collision buffer 
    #endregion

    #region global accesors
    /// <summary>
    /// accessor for the mainCamera
    /// </summary>
    protected GameObject MainCamera {
        get {
            //find the mainCamera if not assigned
            if(mainCamera == null)
            {
                mainCamera = FindObjectOfType<Camera>().gameObject; 
                Assert.IsNotNull(mainCamera, "Scene needs a Camera");
            }
            return mainCamera;
        }
        set
        {
            //make sure new mainCamera object has a camera attached
            if(value.GetComponent<Camera>() != null) { mainCamera = value; }
        }
    }

    /// <summary>
    /// accessor for player gameObject
    /// </summary>
    protected GameObject Player {
        get
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                Assert.IsNotNull(player, "Need to tag a object \"Player\"");
            }
            return player;
        }
    }

    /// <summary>
    /// accessor for player transform
    /// </summary>
    protected Transform PlayerTransform { get { return Player.transform; } }
    #endregion 
}