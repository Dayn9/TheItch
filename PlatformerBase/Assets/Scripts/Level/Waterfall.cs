using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
public class Waterfall : MonoBehaviour
{
    [SerializeField] private Material fadeMat;

    void Awake()
    {
        TilemapRenderer render = GetComponent<TilemapRenderer>();
        render.material = fadeMat;
        render.material.SetFloat("_StartY", transform.localPosition.y + 5);
    }
}
