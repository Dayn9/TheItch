using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable] //Serializable so that it can be created in custom inspector
public class BasicFullTile : Tile {

    [SerializeField] private Sprite[] sprites;

    /// <summary>
    /// called whenever a new tile is placed
    /// </summary>
    /// <param name="position">position of the tile</param>
    /// <param name="tilemap">current tilemap being edited</param>
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        //check the surrounding tiles
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int pos = new Vector3Int(position.x + j, position.y + i, position.z);
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
        int index = GetAdjIndex((byte)mask);

        //check if index is valid
        if (index >= 0 && index < 9)
        {
            tileData.sprite = sprites[index]; //set the sprite
            
            //check if inside corner tile
            if (index == 4)
            {
                //create a new unique mask based on DIAGONAL tiles
                /* 1   2
                 *   -  
                 * 4   8 */
                mask = 0;
                mask += CheckTile(position + new Vector3Int(-1, 1, 0), tilemap) ? 1 : 0;   //top left
                mask += CheckTile(position + new Vector3Int(1, 1, 0), tilemap) ? 2 : 0;    //top right
                mask += CheckTile(position + new Vector3Int(-1, -1, 0), tilemap) ? 4 : 0;  //bottom left
                mask += CheckTile(position + new Vector3Int(1, -1, 0), tilemap) ? 8 : 0;   //bottom right
                index = GetDiagIndex((byte)mask);

                if (index >= 9 && index < sprites.Length)
                {
                    tileData.sprite = sprites[index]; //set sprite to inside corner tile
                }
            }
        }
        else
        {
            tileData.sprite = sprites[4]; //default to center tile
        }
    }

    /// <summary>
    /// convert calculated mask number into sprites index
    /// </summary>
    /// <param name="mask">calculated number from ADJACENT tiles</param>
    /// <returns>sprites index</returns>
    private int GetAdjIndex(byte mask)
    {
        switch (mask)
        {
            case 12: return 0;
            case 14: return 1;
            case 10: return 2;
            case 13: return 3;
            case 15: return 4;
            case 11: return 5;
            case 5: return 6;
            case 7: return 7;
            case 3: return 8;
            default: return -1;
        }

    }

    /// <summary>
    /// convert calculated mask number into sprites index
    /// </summary>
    /// <param name="mask">calculated number from DIAGONAL tiles</param>
    /// <returns>sprites index</returns>
    private int GetDiagIndex(byte mask)
    {
        switch (mask)
        {
            case 14: return 9;
            case 13: return 10;
            case 11: return 11;
            case 7: return 12;
            default: return -1;
        }

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
    [MenuItem("Assets/Create/BasicFullTile")]
    public static void CreateBasicFullTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save BasicFullTile", "New BasicFullTile", "asset", "Save BasicFull Tile", "Assets");
        if (path == "") { return; }

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BasicFullTile>(), path);
    }
#endif


#if UNITY_EDITOR
    //custom editor for BasicFullTile
    [CustomEditor(typeof(BasicFullTile))]
    public class BasicFullTileEditor : Editor
    {
        private BasicFullTile tile { get { return (target as BasicFullTile); } }

        public void OnEnable()
        {
            if (tile.sprites == null || tile.sprites.Length != 13)
                tile.sprites = new Sprite[13];
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Place sprites based on configuration");
            EditorGUILayout.LabelField("If there are no inside corners, use BasicTile");
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
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

            if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(tile); }

            base.OnInspectorGUI();
        }
    }
#endif

    #endregion
}
