using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Assertions;

public class BreakableTilemap : MonoBehaviour
{
    private Tilemap tilemap;
    private BreakParticles breakPart; 

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        breakPart = GetComponent<BreakParticles>();
    }

    public void BreakTile(Vector2 pos)
    {
        Vector3Int roundedPos = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0);

        //determine all the tile positions
        Vector3Int[] tiles = new Vector3Int[] {
            roundedPos,
            roundedPos - Vector3Int.right,
            roundedPos - Vector3Int.up,
            roundedPos - Vector3Int.right - Vector3Int.up
        };

        //destroy any tiles that are active
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tilemap.GetTile(tiles[i]))
            {
                tilemap.SetTile(tiles[i], null);
                breakPart.BreakAt(tiles[i]);
            }
        }
    }
}
