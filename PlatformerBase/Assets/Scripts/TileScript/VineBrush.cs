using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Vine Brush", menuName = "Brushes/Vine Brush")]
[CustomGridBrush(false, true, false, "Vine Brush")]
public class VineBrush : GridBrushBase
{
    [SerializeField] private GameObject transferPrefab;

    //transform parent references 
    private static GameObject transferParent;
    private static GameObject vinesParent;

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        base.Paint(grid, brushTarget, position);
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(transferPrefab);
        if (instance != null)
        {
            //set the undo properties
            Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
            Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefabs");

            instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(position + new Vector3(.5f, .5f, .5f)));
            instance.transform.SetParent(brushTarget.transform);
        }
        
    }
}
