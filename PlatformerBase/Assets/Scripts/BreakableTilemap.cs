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
        Vector3Int tilePosition = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), 0);

        Debug.Log(tilePosition);
        Debug.Log(tilemap.GetTile(tilePosition).name);

        tilemap.SetTile(tilePosition, null);

    }
}
