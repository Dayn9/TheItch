using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Tilemaps;

//SOURCE : From the unity 2D Extras https://github.com/Unity-Technologies/2d-extras/blob/master/Assets/Tilemap/Tiles/Animated%20Tile/Scripts/AnimatedTile.cs

public class UnityAnimatedTile : TileBase {

    public Sprite[] m_AnimatedSprites;
    public float m_MinSpeed = 1f;
    public float m_MaxSpeed = 1f;
    public float m_AnimationStartTime;
    public Tile.ColliderType m_TileColliderType;

    public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
    {
        tileData.transform = Matrix4x4.identity;
        tileData.color = Color.white;
        if (m_AnimatedSprites != null && m_AnimatedSprites.Length > 0)
        {
            tileData.sprite = m_AnimatedSprites[m_AnimatedSprites.Length - 1];
            tileData.colliderType = m_TileColliderType;
        }
    }

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
    {
        if (m_AnimatedSprites.Length > 0)
        {
            tileAnimationData.animatedSprites = m_AnimatedSprites;
            tileAnimationData.animationSpeed = UnityEngine.Random.Range(m_MinSpeed, m_MaxSpeed);
            tileAnimationData.animationStartTime = m_AnimationStartTime;
            return true;
        }
        return false;
    }
}
