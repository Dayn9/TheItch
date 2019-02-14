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

    void Awake()
    {
        inventoryUI = transform; //set the inventory transform to this object
        display = transform.GetChild(0);
        hidden = true;

        //may need to be removed later, makes sure no items stored when new level is loaded
        if (Items.Count > 0)
        {
            Dictionary<string, GameObject> tempItems = Items;
            foreach(GameObject item in Items.Values)
            {
                item.transform.parent = inventoryUI;
            }
            DisplayItems();
            hidden = false; //there are items so UI should be showing
        }

        collectEffect = Instantiate(collectEffectPrefab, Vector2.zero, Quaternion.identity);
        collectEffect.SetActive(false);
    }

    private void Start()
    {
        display.localPosition = hidden ? hiddenOffset : Vector2.zero; //move the display to a hidden position if hidden
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
