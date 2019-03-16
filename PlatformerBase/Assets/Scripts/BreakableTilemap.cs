using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakableTilemap : MonoBehaviour
{
    private Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void BreakTile(Vector2 pos)
    {
        Vector3Int roundedPos = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0);

        Vector3Int[] tiles = new Vector3Int[] {
            roundedPos,
            roundedPos - Vector3Int.right,
            roundedPos - Vector3Int.up,
            roundedPos - Vector3Int.one
        };

        float rand = Random.value;

        for(int i = 0; i< tiles.Length; i++)
        {
            if (tilemap.GetTile(tiles[i]))
            {
                Debug.Log("tile at " + i + " " + rand);
                tilemap.SetTile(tiles[i], null);
            }
        }
        

    }
}
