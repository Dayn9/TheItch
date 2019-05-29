using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Page : MonoBehaviour, ILevelData
{
    private bool collected = false;

    public bool State { get { return collected; } }
    public string Name { get { return gameObject.name; } }

    private static Books books;

    private void Awake()
    {
        if (!books) { books = FindObjectOfType<Books>(); }

        //select a random sprite
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

    public void OnLevelLoad(bool state)
    {
        collected = state;
        gameObject.SetActive(collected);
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!collected && coll.CompareTag("Player"))
        {
            Inventory.PlayCollectionEffectAt(transform.position);

            books.CollectPage();

            collected = true;
            gameObject.SetActive(false);
        }
    }
}
