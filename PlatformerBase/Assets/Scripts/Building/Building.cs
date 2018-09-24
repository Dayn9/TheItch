using UnityEngine;

public class Building : Global {

    [SerializeField] private GameObject Exterior; //Exterior Tilemap
    [SerializeField] private BuildingLayer[] Interior; //Interior layer tilemaps and joining doors
    [SerializeField] [Range(0.0f, 1.0f)] private float doorAboveAlpha; //alpha when door leads back a layer

    [SerializeField] private int doorAboveLayer; //order in sorting layer of door that leads back a layer
    [SerializeField] private int doorBelowLayer; //order in sorting layer of door that leads forward a layer

    private PhysicsObject player; //ref to physicsObject type script attached to player

    private int currentLayer = 0; //layer of the building the player is currently in

    private Color solidDoor; //color of door when on current layer
    private Color transparentDoor; //color of door when on previous layer

	// Use this for initialization
	void Start () {
        player = Player.GetComponent<PhysicsObject>(); //UNTESTED

        solidDoor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        transparentDoor = new Color(1.0f, 1.0f, 1.0f, doorAboveAlpha);

        Exterior.SetActive(true); //exterior layer always starts active
        for(int i = 0; i<Interior.Length; i++)
        {
            Interior[i].Door.SetActive(i == 0 ? true : false); //only first door starts active
            Interior[i].Door.GetComponent<Door>().LayerInBuilding = i; //set layer of door to the layer they appear fully on
            Interior[i].Door.GetComponent<Door>().OnDoorOpen += RoomChange; //Add Room Change to OnDoorOpen Event
            Interior[i].Door.GetComponent<SpriteRenderer>().color = solidDoor;
            Interior[i].Door.GetComponent<SpriteRenderer>().sortingOrder = doorBelowLayer;

            Interior[i].Layer.SetActive(false); //all inyterior layers start inactive
        }
	}

	/// <summary>
    /// change activation layers off doors and layers to transition between layers in building
    /// </summary>
    /// <param name="layer">layer of door interacting with</param>
    private void RoomChange(int layer)
    {
        //go back a layer
        if(layer == currentLayer)
        {
            if(layer == 0)
            {
                Exterior.SetActive(false);
                player.SetCollision("SolidMoveableObject", true);
            }
            else
            {
                Interior[layer - 1].Layer.SetActive(false); 
                Interior[layer - 1].Door.SetActive(false);
            }
            Interior[layer].Layer.SetActive(true);
            Interior[layer].Door.GetComponent<SpriteRenderer>().color = transparentDoor;
            Interior[layer].Door.GetComponent<SpriteRenderer>().sortingOrder = doorAboveLayer;
            if (layer+1 < Interior.Length)
            {
                Interior[layer + 1].Door.GetComponent<SpriteRenderer>().sortingOrder = doorBelowLayer;
                Interior[layer + 1].Door.GetComponent<SpriteRenderer>().color = solidDoor;
                Interior[layer + 1].Door.SetActive(true);
            }
            currentLayer++;
        }
        //go forwards a layer
        else if(layer == currentLayer - 1)
        {
            if(layer == 0)
            {
                Exterior.SetActive(true);
                player.SetCollision("SolidMoveableObject", false);
            }
            else
            {
                Interior[layer - 1].Layer.SetActive(true);
                Interior[layer - 1].Door.SetActive(true);
                Interior[layer - 1].Door.GetComponent<SpriteRenderer>().sortingOrder = doorAboveLayer;
                Interior[layer - 1].Door.GetComponent<SpriteRenderer>().color = transparentDoor;
            }
            Interior[layer].Layer.SetActive(false);
            Interior[layer].Door.GetComponent<SpriteRenderer>().sortingOrder = doorBelowLayer;
            Interior[layer].Door.GetComponent<SpriteRenderer>().color = solidDoor;
            if (layer + 1 < Interior.Length)
            {
                Interior[layer + 1].Door.SetActive(false);
            }
            currentLayer--;
        }
    }
}

[System.Serializable]
public class BuildingLayer
{
    [SerializeField] private GameObject door; //Door that connects from previous layer to this one
    [SerializeField] private GameObject layer; //Tilemap for interior layer
    
    //Properties
    public GameObject Door { get { return door; } }
    public GameObject Layer { get { return layer; } }
}