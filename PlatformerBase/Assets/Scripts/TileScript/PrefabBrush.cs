using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Prfab Brush", menuName = "Brushes/Prefab Brush")]
[CustomGridBrush(false, true, false, "Prefab Brush")] // https://docs.unity3d.com/Manual/Tilemap-ScriptableBrushes.html
public class PrefabBrush : GridBrushBase
{
    /// <summary>
    /// Based off https://github.com/Unity-Technologies/2d-extras/blob/master/Assets/Tilemap/Brushes/Prefab%20Brush/Scripts/Editor/PrefabBrush.cs
    /// </summary>

    [SerializeField] private GameObject prefab; //prefab to create
    [SerializeField] private bool item; //true if prefab is an item

    private static GameObject itemsParent; //ref to the items parent gameobject

    //get / find / create the itemsParent game object
    [ExecuteInEditMode]
    private GameObject ItemsParent
    {
        get
        {
            if(itemsParent != null) { return itemsParent; } //ref exists 
            itemsParent = GameObject.Find("Items"); //need to find ref
            if (itemsParent != null) { return itemsParent; }
            itemsParent = Instantiate(new GameObject()); //need to create ref
            itemsParent.name = "Items";
            itemsParent.AddComponent<AudioPlayer>();
            return itemsParent;
        }
    }

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        // Do not allow editing palettes
        if (brushTarget.layer == 31)
            return;

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        if(instance != null)
        {
            //set the undo properties
            Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
            Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");
            instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(position + new Vector3(.5f, .5f, .5f)));
            if (item)
            {
                instance.transform.SetParent(ItemsParent.transform);
                //instance.name = ItemName();
                return;
            }
            instance.transform.SetParent(brushTarget.transform);
            
        }
    }

    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        // Do not allow editing palettes
        if (brushTarget.layer == 31)
            return;

        Transform erased = GetObjectInCell(gridLayout, brushTarget.transform, position);
        if(erased != null)
        {
            Undo.DestroyObjectImmediate(erased.gameObject);
        }
    }

    private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
    {
        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
        Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
        Bounds bounds = new Bounds((max + min) * 0.5f, max - min);

        int children = parent.childCount;
        for (int i = 0; i < children; i++)
        {
            Transform child = parent.GetChild(i);
            if (bounds.Contains(child.position)) { return child; }
        }
        return null;
    }

    [ExecuteInEditMode]
    private string ItemName()
    {
        int childCount = ItemsParent.transform.childCount;
        int highestItemNum = -1;
        for (int i = 0; i < childCount; i++)
        {
            string name = itemsParent.transform.GetChild(i).name;
            if(name.Substring(0, prefab.name.Length) == prefab.name)
            {
                int childNum = int.Parse(name);
                if(childNum > highestItemNum) { highestItemNum = childNum; }
            }
        }
        Debug.Log(prefab.name + (highestItemNum + 1));
        return prefab.name + (highestItemNum + 1);
    }

    #if UNITY_EDITOR
    //create asset menu for prefab brush
    [MenuItem("Assets/Create/PrefabBrush")]
    public static void CreatePrefabBrush()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save PrefabBrush", "New PrefabBrush", "asset", "Save PrefabBrush", "Assets");
        if (path == "") { return; }

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PrefabBrush>(), path);
    }
    #endif
}
