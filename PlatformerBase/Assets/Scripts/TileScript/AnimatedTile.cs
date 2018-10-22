using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

//modified from https://github.com/Unity-Technologies/2d-techdemos/blob/master/Assets/Tilemap/Tiles/Animated%20Tile/Scripts/AnimatedTile.cs

[Serializable] //Serializable so that it can be created in custom inspector
[CreateAssetMenu(fileName = "New Animated Tile", menuName = "Tiles/Animated Tile")]
public class AnimatedTile : TileBase 
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
    }

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        if (frames != null && frames.Length > 0)
        {
            tileAnimationData.animatedSprites = frames;
            tileAnimationData.animationSpeed = speed;
            tileAnimationData.animationStartTime = UnityEngine.Random.Range(0, frames.Length);
            return true;
        }
        return false;
    }

    #if UNITY_EDITOR
    //create a scriptable AnimatedTile object
    [MenuItem("Assets/Create/AnimatedTile")]
    public static void CreateAnimatedTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save AnimatedTile", "New AnimatedTile", "asset", "Save AnimatedTile", "Assets");
        if (path == "") { return; }

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AnimatedTile>(), path);
    }
    #endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(AnimatedTile))]
public class AnimatedTileEditor : Editor
{
    private AnimatedTile tile { get { return (target as AnimatedTile); } }

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
