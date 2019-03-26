using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable] //Serializable so that it can be created in custom inspector
public class FullTile : Tile
{

    [SerializeField] private Sprite[] sprites;

    /// <summary>
    /// called whenever a new tile is placed
    /// </summary>
    /// <param name="position">position of the tile</param>
    /// <param name="tilemap">current tilemap being edited</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        //check the surrounding tiles
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int pos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (CheckTile(pos, tilemap))
                {
                    tilemap.RefreshTile(pos); //refresh nearby tiles
                }
            }
        }
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //create a unique number based on ADJACENT tiles 
        /*   1
         * 2 - 4 
         *   8  */
        int mask = 0;
        mask += CheckTile(position + new Vector3Int(0, 1, 0), tilemap) ? 1 : 0;     //top
        mask += CheckTile(position + new Vector3Int(-1, 0, 0), tilemap) ? 2 : 0;    //left
        mask += CheckTile(position + new Vector3Int(1, 0, 0), tilemap) ? 4 : 0;     //right
        mask += CheckTile(position + new Vector3Int(0, -1, 0), tilemap) ? 8 : 0;    //bottom

        tileData.sprite = sprites[mask];
    }


    /// <summary>
    /// check if tile at position is the same type of tile as this one
    /// </summary>
    /// <param name="position">position checking at</param>
    /// <param name="tilemap">current tilemap being edited </param>
    /// <returns></returns>
    private bool CheckTile(Vector3Int position, ITilemap tilemap)
    {
        return tilemap.GetTile(position) == this;
    }

    #region Editor

#if UNITY_EDITOR
    //create a scriptable BasicFullTile object
    [MenuItem("Assets/Create/Tiles/FullTile")]
    public static void CreateBasicFullTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save FullTile", "New FullTile", "asset", "Save Full Tile", "Assets");
        if (path == "") { return; }

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FullTile>(), path);
    }
#endif


#if UNITY_EDITOR
    //custom editor for BasicFullTile
    [CustomEditor(typeof(FullTile))]
    public class FullTileEditor : Editor
    {
        private FullTile tile { get { return (target as FullTile); } }

        public void OnEnable()
        {
            if (tile.sprites == null || tile.sprites.Length != 16)
                tile.sprites = new Sprite[16];
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Place sprites based on configuration");
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            /*
            tile.sprites[0] = (Sprite)EditorGUILayout.ObjectField("TopLeft", tile.sprites[0], typeof(Sprite), false, null);
            tile.sprites[1] = (Sprite)EditorGUILayout.ObjectField("TopCenter", tile.sprites[1], typeof(Sprite), false, null);
            tile.sprites[2] = (Sprite)EditorGUILayout.ObjectField("TopRight", tile.sprites[2], typeof(Sprite), false, null);
            tile.sprites[3] = (Sprite)EditorGUILayout.ObjectField("MiddleLeft", tile.sprites[3], typeof(Sprite), false, null);
            tile.sprites[4] = (Sprite)EditorGUILayout.ObjectField("MiddleCenter", tile.sprites[4], typeof(Sprite), false, null);
            tile.sprites[5] = (Sprite)EditorGUILayout.ObjectField("MiddleRight", tile.sprites[5], typeof(Sprite), false, null);
            tile.sprites[6] = (Sprite)EditorGUILayout.ObjectField("BottomLeft", tile.sprites[6], typeof(Sprite), false, null);
            tile.sprites[7] = (Sprite)EditorGUILayout.ObjectField("BottomCenter", tile.sprites[7], typeof(Sprite), false, null);
            tile.sprites[8] = (Sprite)EditorGUILayout.ObjectField("BottomRight", tile.sprites[8], typeof(Sprite), false, null);

            tile.sprites[9] = (Sprite)EditorGUILayout.ObjectField("InsideTopLeft", tile.sprites[9], typeof(Sprite), false, null);
            tile.sprites[10] = (Sprite)EditorGUILayout.ObjectField("InsideTopRight", tile.sprites[10], typeof(Sprite), false, null);
            tile.sprites[11] = (Sprite)EditorGUILayout.ObjectField("InsideBottomLeft", tile.sprites[11], typeof(Sprite), false, null);
            tile.sprites[12] = (Sprite)EditorGUILayout.ObjectField("InsideBottomRight", tile.sprites[12], typeof(Sprite), false, null);
            */
            if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(tile); }

            base.OnInspectorGUI();
        }
    }
#endif

    #endregion
}
