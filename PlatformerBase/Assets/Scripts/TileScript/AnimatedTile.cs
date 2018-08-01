using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable] //Serializable so that it can be created in custom inspector
public class AnimatedTile : Tile
{
    private Sprite[] frames;
    private float speed;

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        if (frames.Length > 0)
        {
            tileAnimationData.animatedSprites = frames;
            tileAnimationData.animationSpeed = speed;
            tileAnimationData.animationStartTime = 0;
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

    #if UNITY_EDITOR
    [CustomEditor(typeof(AnimatedTile))]
    public class AnimatedTileEditor : Editor
    {
        private AnimatedTile tile { get { return (target as AnimatedTile); } }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            int count = EditorGUILayout.DelayedIntField("Frame Count", tile.frames != null ? tile.frames.Length : 0);
            if(count < 0) { count = 0; } 
            if (tile.frames == null || tile.frames.Length != count) { Array.Resize<Sprite>(ref tile.frames, count); } //resize frames array to match count
            if(count == 0) { return; }

            EditorGUILayout.Space();

            for (int i = 0; i < count; i++)
            {
                tile.frames[i] = (Sprite)EditorGUILayout.ObjectField("Frame " + (i + 1), tile.frames[i], typeof(Sprite), false, null);
            }
            tile.speed = EditorGUILayout.FloatField("Speed", tile.speed);
            if(tile.speed < 0) { tile.speed = 0; }

            if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(tile); }
            base.OnInspectorGUI();
        }
    }
    #endif
}
