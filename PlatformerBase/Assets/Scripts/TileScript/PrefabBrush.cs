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
                //mak a new Items gameObject
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
        for(int i = 0; i< children; i++)
        {
            Transform child = parent.GetChild(i);
            if (bounds.Contains(child.position)) { return child; }
        }
        return null;
    }
}

[CustomEditor(typeof(PrefabBrush))]
public class PrefabBrushEditor : GridBrushEditorBase
{
    private PrefabBrush prefabBrush { get { return target as PrefabBrush; } }

    private SerializedProperty Prefab;
    private SerializedObject SerializedObject;
    private SerializedProperty Item;

    protected void OnEnable()
    {
        SerializedObject = new SerializedObject(target);
        Prefab = SerializedObject.FindProperty("Prefab");
        Item = SerializedObject.FindProperty("Item");
    }

    public override void OnPaintInspectorGUI()
    {
        SerializedObject.UpdateIfRequiredOrScript();
        EditorGUILayout.PropertyField(Prefab, true);
        EditorGUILayout.PropertyField(Item, true);
        SerializedObject.ApplyModifiedPropertiesWithoutUndo();
    }
}
