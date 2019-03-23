using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif


[Serializable] //Serializable so that it can be created in custom inspector
[CreateAssetMenu(fileName = "New Destructable Tile", menuName = "Tiles/Destructable Tile")]
public class DestructableTile : TileBase
{
    public Sprite[] frames;
    public float speed;

    public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
    {
        tileData.transform = Matrix4x4.identity;
        tileData.color = Color.white;
        if (frames != null && frames.Length > 0)
        {
            tileData.sprite = frames[0];
        }

        tileData.colliderType = Tile.ColliderType.None;
    }

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        if (frames != null && frames.Length > 0)
        {
            tileAnimationData.animatedSprites = frames;
            tileAnimationData.animationSpeed = speed;
            tileAnimationData.animationStartTime = Time.frameCount + 1;
            return true;
        }
        return false;
    }


#if UNITY_EDITOR
    //create a scriptable AnimatedTile object
    [MenuItem("Assets/Create/Destructable")]
    public static void CreateDestructableTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save DestructableTile", "New DestructableTile", "asset", "Save DestructableTile", "Assets");
        if (path == "") { return; }

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DestructableTile>(), path);
    }
    #endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(DestructableTile))]
public class DestructableTileEditor : Editor
{
    private DestructableTile tile { get { return (target as DestructableTile); } }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        int count = EditorGUILayout.DelayedIntField("Frame Count", tile.frames != null ? tile.frames.Length : 0);
        if (count < 0) { count = 0; }

        if (tile.frames == null || tile.frames.Length != count) { Array.Resize<Sprite>(ref tile.frames, count); } //resize frames array to match count

        if (count == 0) { return; }


        EditorGUILayout.Space();

        for (int i = 0; i < count; i++)
        {
            tile.frames[i] = (Sprite)EditorGUILayout.ObjectField("Frame " + (i + 1), tile.frames[i], typeof(Sprite), false, null);
        }
        tile.speed = EditorGUILayout.FloatField("Speed", tile.speed);
        if (tile.speed < 0) { tile.speed = 0; }

        if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(tile); }
    }
}
#endif