using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileInline : MonoBehaviour
{
    private Material mat;

    void Awake()
    {
        mat = GetComponent<TilemapRenderer>().material;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (mat != null)
        {
            Graphics.Blit(src, dst, mat);
        }
    }
}
