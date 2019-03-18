using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Assertions;

public class BreakableTilemap : MonoBehaviour
{
    private Tilemap tilemap;

    private static GridInformation gridInfo;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        if (gridInfo == null)
        {
            gridInfo = FindObjectOfType<GridInformation>();
            Assert.IsNotNull(gridInfo, "Projectile couldn't find gridInfo");
        }
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
        for(int i = 0; i < tiles.Length; i++)
        {
            if (tilemap.GetTile(tiles[i]))
            {
                gridInfo.ErasePositionProperty(tiles[i], "Destroyed");
                gridInfo.SetPositionProperty(tiles[i], "Destroyed", 1);

                //tilemap.SetTile(tiles[i], null);
            }
        }
    }
}
