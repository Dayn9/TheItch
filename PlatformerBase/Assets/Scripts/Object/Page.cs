using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Page : Inventory, ILevelData
{
    private bool collected = false;

    public bool State { get {
            Debug.Log(Name + " " + collected);
            return collected; } }
    public string Name { get { return gameObject.name; } }

    private static Books books;
    private SpriteRenderer render;


    [SerializeField] private Sprite[] pagesImages;

    private void Awake()
    {
        if (!books) { books = FindObjectOfType<Books>(); }


        //select a random sprite
        render = GetComponent<SpriteRenderer>();
        render.sprite = pagesImages[Random.Range(0, pagesImages.Length)];
    }
    private void Start()
    {
        render.enabled = !collected;
    }

    private void Update()
    {
        
    }

    public void OnLevelLoad(bool state)
    {
        collected = state;
        if (collected)
        {
            books.CollectPage();
        }
        render.enabled = !collected;
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!collected && coll.CompareTag("Player"))
        {
            PlayCollectionEffectAt(transform.position);

            books.CollectPage();

            collected = true;
            render.enabled = false;
        }
    }
}
