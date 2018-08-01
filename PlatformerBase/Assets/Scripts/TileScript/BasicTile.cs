using System;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable] //Serializable so that it can be created in custom inspector
public class BasicTile : Tile
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
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int pos = new Vector3Int(position.x + j, position.y + i, position.z);
                //check if same tile on adjacent
                if (CheckTile(pos, tilemap))
                {
                    tilemap.RefreshTile(pos);
                }
            }
        }
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //create a unique number based on surrounding tiles 
        /*   1
         * 2 - 4 
         *   8  */
        int mask = 0;
        mask += CheckTile(position + new Vector3Int(0, 1, 0), tilemap) ? 1 : 0;     //top
        mask += CheckTile(position + new Vector3Int(-1, 0, 0), tilemap) ? 2 : 0;    //left
        mask += CheckTile(position + new Vector3Int(1, 0, 0), tilemap) ? 4 : 0;     //right
        mask += CheckTile(position + new Vector3Int(0, -1, 0), tilemap) ? 8 : 0;    //bottom
        int index = GetIndex((byte)mask); 

        //check if index is valid
        if(index >= 0 && index < sprites.Length)
        {
            tileData.sprite = sprites[index]; //set the sprite
        }
        else
        {
            tileData.sprite = sprites[4]; //default to center tile
        }
    }

    /// <summary>
    /// convert calculated mask number into sprites index
    /// </summary>
    /// <param name="mask">calculated number from adjacent tiles</param>
    /// <returns>sprites index</returns>
    private int GetIndex(byte mask)
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
    //create a scriptable BasicTile object
    [MenuItem("Assets/Create/BasicTile")]
    public static void CreateBasicTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save BasicTile", "New BasicTile", "asset", "Save Basic Tile", "Assets");
        if (path == "") { return; }

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BasicTile>(), path);
    }
    #endif

    
    #if UNITY_EDITOR
    //custom editor for BasicTile
    [CustomEditor(typeof(BasicTile))]
    public class BasicTileEditor : Editor
    {
        private BasicTile tile { get { return (target as BasicTile); } }

        public void OnEnable()
        {
            if (tile.sprites == null || tile.sprites.Length != 9)
                tile.sprites = new Sprite[9];
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Place sprites based on configuration");
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

            if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(tile); }

            base.OnInspectorGUI();
        }
    }
    #endif

    #endregion
}