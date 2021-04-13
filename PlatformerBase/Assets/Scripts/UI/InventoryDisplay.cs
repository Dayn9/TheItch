using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIAnchor))]
public class InventoryDisplay : Inventory {

    [SerializeField] private Vector2 hiddenOffset; //relative position of display when hidden
    [SerializeField] private float moveSpeed; //how fast to hide and show

    private Transform display; //child object with the inventory display 
    private bool hidden = true; //true when inventory is empty and display not showing
    private Vector2 moveVector; //temporary vector when moving 

    [SerializeField] protected GameObject collectEffectPrefab; //prefab for collection effects
    [SerializeField] private List<GameObject> allItems;

    [SerializeField] private GameObject itemLabelPrefab;

    public static bool loadGame = false;

    void Awake()
    {
        inventoryUI = transform; //set the inventory transform to this object
        display = transform.GetChild(0);
        inventoryRenderer = display.GetComponent<SpriteRenderer>();
        //hidden = true;

        itemLabels = new List<ItemLabel>();
        
        for (int i = 0; i < inventorySize; i++)
        {
            CreateItemLabel(i);
        }

        //set up the all items states if it doesn't already exist
        if (allItemsStates == null) {
            allItemsStates = new Dictionary<string, int>();
            for(int i = 0; i < allItems.Count; i++)
            {
                allItemsStates.Add(allItems[i].name, loadGame ? GameSaver.gameData.itemStates[i]: 0); //all items start in state 0 - uncollected
            }
            loadGame = false;
        }

        GameObject newItem = null; //temp newItem Gameobject
        //Instantiate and activate the items
        foreach(GameObject item in allItems)
        {
            if(allItemsStates[item.name] == 1) //check if the item should be in the inventory
            {
                newItem = Instantiate(item, inventoryUI); //instantiate all items as child objects
                newItem.name = newItem.name.Substring(0, newItem.name.Length - 7); //remove (clone) from name
                newItem.SetActive(true);
                Items.Add(newItem.name, newItem); //directly add to the list (skips some unnecissary code)
                newItem.GetComponent<CollectableItem>().Collected(); //sets the internal states and layers
            }
        }

        collectEffect = Instantiate(collectEffectPrefab, Vector2.zero, Quaternion.identity);
        collectEffect.SetActive(false);

        hidden = (Items.Count == 0);
    }

    public void Start()
    {
        //move the display to a hidden position if hidden
        display.localPosition = hidden ? hiddenOffset : Vector2.zero;
        DisplayItems();
    }

    public void CreateItemLabel(int i)
    {
        GameObject tempItemLabel;
        tempItemLabel = Instantiate(itemLabelPrefab, display);
        tempItemLabel.name = "Item Label " + i;

        itemLabels.Add(tempItemLabel.GetComponent<ItemLabel>());

        tempItemLabel.GetComponent<ItemLabel>().SlotNum = i;

        tempItemLabel.transform.localPosition = new Vector2(-0.5f, -3.25f - (1.5f * i));
    }

    public void CreateVirusKey()
    {
        GameObject virusKey = Instantiate(allItems.Find(i => i.name == "VirusKey"), Global.Player.transform.position, Quaternion.identity);
        virusKey.name = "VirusKey";
        virusKey.SetActive(true);
    }


    void Update()
    {
        if (!Pause.menuPaused)
        {
            //display is hidden but there are items
            if (hidden && Items.Count > 0)
            {
                MoveToLocalPosition(Vector2.zero);
            }
            //display is showing but there are no items
            else if (!hidden && Items.Count == 0)
            {
                MoveToLocalPosition(hiddenOffset);
            }
        }
    }

    /// <summary>
    /// Moves the inventoryDisplay to a target location local to inventory
    /// </summary>
    /// <param name="target">position to move to</param>
    private void MoveToLocalPosition(Vector2 target)
    {
        moveVector = (((Vector3)target) - display.localPosition).normalized * moveSpeed * Time.deltaTime;
        //check if close enought to snap into position
        if ((((Vector3)target) - display.localPosition).magnitude < moveVector.magnitude)
        {
            display.localPosition = target; //snap into position
            hidden = !hidden; //toggle hidden
            return; //stop moving
        }
        display.localPosition += (Vector3)moveVector;
    }
}
