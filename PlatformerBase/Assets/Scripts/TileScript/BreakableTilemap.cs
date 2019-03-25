using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(BreakParticles))]
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
                tilemap.SetTile(tilePositions[i], null);
                breakPart.BreakAt(tilePositions[i]); //spawn particles
            }
        }
    }
}
