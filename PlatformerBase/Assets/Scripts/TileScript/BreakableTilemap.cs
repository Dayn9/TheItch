using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(BreakParticles))]
public class BreakableTilemap : MonoBehaviour
{
    private Tilemap tilemap;
    private BreakParticles breakPart;

    [SerializeField] private TileBase brokenTile;

    private List<Vector2Int> broken;

    public List<Vector2Int> Broken
    {
        get { return broken; }
        set {
            if (broken == null || broken.Count == 0) {
                broken = value;
                tilemap = GetComponent<Tilemap>();
                //break all the tiles that should be broken
                foreach(Vector2Int broke in broken)
                {
                    tilemap.SetTile((Vector3Int)broke, brokenTile);
                }
            }
        }
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        breakPart = GetComponent<BreakParticles>();

        if (broken == null) { broken = new List<Vector2Int>(); };
    }

    public void BreakTile(Vector2 pos)
    {
        Vector3Int projectilePosition = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), 0);

        //determine all the tile positions
        Vector3Int[] tilePositions = new Vector3Int[] {
            //TOP 
            projectilePosition - Vector3Int.right + Vector3Int.up, 
            projectilePosition + Vector3Int.up,
            projectilePosition + Vector3Int.right + Vector3Int.up,
            //MIDDLE
            projectilePosition - Vector3Int.right,
            projectilePosition,
            projectilePosition + Vector3Int.right,
            //BOTTOM
            projectilePosition - Vector3Int.right - Vector3Int.up,
            projectilePosition - Vector3Int.up,
            projectilePosition + Vector3Int.right - Vector3Int.up,

        };

        //destroy any tiles that are active
        for (int i = 0; i < tilePositions.Length; i++)
        {
            if (tilemap.GetTile(tilePositions[i]))
            {
                tilemap.SetTile(tilePositions[i], brokenTile);
                breakPart.BreakAt(tilePositions[i]); //spawn particles

                broken.Add((Vector2Int)tilePositions[i]);
            }
        }
    }
}
