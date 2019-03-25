using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Assertions;

public class BreakableTilemap : MonoBehaviour
{
    private Tilemap tilemap;
    private Tilemap backing;

    [SerializeField] private DestructableTile destructable;

    private const float sps = 1.5f; //samples per second
    private float animationTime;

    private Dictionary<Vector3Int, float> destructTimers;

    private static bool origional = true;

    private void Awake()
    {
        if (origional)
        {
            origional = false;

            GameObject back = Instantiate(gameObject, transform);
            Destroy(back.GetComponent<BreakableTilemap>());
            Destroy(back.GetComponent<CompositeCollider2D>());
            Destroy(back.GetComponent<Rigidbody>());
            Destroy(back.GetComponent<TilemapCollider2D>());
            backing = back.GetComponent<Tilemap>();

            destructTimers = new Dictionary<Vector3Int, float>();
            animationTime = (destructable.frames.Length - 1) / (sps * destructable.speed);
            tilemap = GetComponent<Tilemap>();
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
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tilemap.GetTile(tiles[i]) && !destructTimers.ContainsKey(tiles[i]))
            {
                backing.SetTile(tiles[i], destructable);
                destructTimers.Add(tiles[i], 0);
                tilemap.SetTile(tiles[i], null);


            }
        }
    }

    private void Update()
    {
        if(destructTimers.Count > 0)
        {
            Dictionary<Vector3Int, float> newdestructTimers = new Dictionary<Vector3Int, float>();
            foreach (Vector3Int pos in destructTimers.Keys)
            {
                if (destructTimers[pos] + Time.deltaTime < animationTime)
                {
                    newdestructTimers.Add(pos, destructTimers[pos] + Time.deltaTime);
                }
                else
                {
                    backing.SetTile(pos, null);
                }
            }
            destructTimers = newdestructTimers;
        }
        
    }
}
