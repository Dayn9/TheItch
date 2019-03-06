using UnityEngine;

[RequireComponent(typeof(AudioPlayer))]
public class Building : Global {

    [SerializeField] private GameObject Exterior; //Exterior Tilemap
    [SerializeField] private BuildingLayer[] Interiors; //Interior layer tilemaps and joining doors

    private const float doorAboveAlpha = 0.4f; //alpha when door leads back a layer
    private const int doorAboveLayer = 8; //order in sorting layer of door that leads back a layer
    private const int doorBelowLayer = 3; //order in sorting layer of door that leads forward a layer

    private PhysicsObject physPlayer; //ref to physicsObject type script attached to player

    private int currentLayer = 0; //layer of the building the player is currently in

    private Color solidDoor; //color of door when on current layer
    private Color transparentDoor; //color of door when on previous layer

    private static Fade fade; //ref to the fade in the UI fade gameobject
    private Renderer[] myRenderers; //array of all renderers in building (used to fade)

    /// <summary>
    /// gets or finds the ref to the fade script in the fade UI gameobject
    /// </summary>
    public Fade Fade {
        get
        {
            if(fade == null) { fade = FindObjectOfType<Fade>(); } //find fade is no ref
            return fade; //return the fade ref
        }
    }

    // Use this for initialization
    void Start()
    {
        physPlayer = Player.GetComponent<PhysicsObject>();

        solidDoor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        transparentDoor = new Color(1.0f, 1.0f, 1.0f, doorAboveAlpha);

        if (fade == null) { fade = FindObjectOfType<Fade>(); } //find fade is no ref
        myRenderers = GetComponentsInChildren<Renderer>(true);

        Exterior.SetActive(true); //exterior layer always starts active
        for (int i = 0; i < Interiors.Length; i++)
        {
            foreach (GameObject Door in Interiors[i].Doors)
            {
                Door.SetActive(i == 0 ? true : false); //only first door starts active
                Door.GetComponent<Door>().LayerInBuilding = i; //set layer of door to the layer they appear fully on
                Door.GetComponent<Door>().OnDoorOpen += RoomChange; //Add Room Change to OnDoorOpen Event
                Door.GetComponent<SpriteRenderer>().color = solidDoor;
                Door.GetComponent<SpriteRenderer>().sortingOrder = doorBelowLayer;
            }
            Interiors[i].Layer.SetActive(false); //all interior layers start inactive
        }
    }

    /// <summary>
    /// change activation layers off doors and layers to transition between layers in building
    /// </summary>
    /// <param name="layer">layer of door interacting with</param>
    private void RoomChange(int layer)
    {
        //fade the background
        if (currentLayer == 0)
        {
            Fade.EnableFade(myRenderers);
            fade.EnableFade(Player.GetComponent<Renderer>());
            //fade.EnableFade(MainCamera.GetComponentsInChildren<Renderer>());
        }
        else if (layer == 0)
        {
            Fade.DisableFade(myRenderers);
            fade.DisableFade(Player.GetComponent<Renderer>());
            //fade.DisableFade(MainCamera.GetComponentsInChildren<Renderer>());
        }

        //go back a layer
        if (layer == currentLayer)
        {
            if (layer == 0)
            {
                Exterior.SetActive(false);
                physPlayer.SetCollision("SolidMoveableObject", true);
            }
            else
            {
                Interiors[layer - 1].Layer.SetActive(false);
                Interiors[layer - 1].SetActiveDoors(false);
            }
            Interiors[layer].Layer.SetActive(true);
            Interiors[layer].SetRendererDoors(doorAboveLayer, transparentDoor);
            if (layer + 1 < Interiors.Length)
            {
                Interiors[layer + 1].SetActiveDoors(true);
                Interiors[layer + 1].SetRendererDoors(doorBelowLayer, solidDoor);
            }
            currentLayer++;
        }
        //go forwards a layer
        else if (layer == currentLayer - 1)
        {
            if (layer == 0)
            {
                Exterior.SetActive(true);
                physPlayer.SetCollision("SolidMoveableObject", false);
            }
            else
            {
                Interiors[layer - 1].Layer.SetActive(true);
                Interiors[layer - 1].SetActiveDoors(true);
                Interiors[layer - 1].SetRendererDoors(doorAboveLayer, transparentDoor);
            }
            Interiors[layer].Layer.SetActive(false);
            Interiors[layer].SetRendererDoors(doorBelowLayer, solidDoor);
            if (layer + 1 < Interiors.Length)
            {
                Interiors[layer + 1].SetActiveDoors(false);
            }
            currentLayer--;
        }
    }
}

[System.Serializable]
public class BuildingLayer
{
    [SerializeField] private GameObject[] doors; //Door that connects from previous layer to this one
    [SerializeField] private GameObject layer; //Tilemap for interior layer

    //Properties
    public GameObject[] Doors { get { return doors; } }
    public GameObject Layer { get { return layer; } }

    /// <summary>
    /// sets the active state of all the doors in the layer
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveDoors(bool active)
    {
        foreach (GameObject Door in Doors)
        {
            Door.SetActive(active);
        }
    }

    /// <summary>
    /// sets the sprite renderer properties of all the doors in the layer
    /// </summary>
    /// <param name="sortingOrder">new sorting order of the doors</param>
    /// <param name="color">new color of the doors</param>
    public void SetRendererDoors(int sortingOrder, Color color)
    {
        foreach (GameObject Door in Doors)
        {
            Door.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            Door.GetComponent<SpriteRenderer>().color = color;
        }
    }
}