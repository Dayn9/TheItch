using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelChange : Inventory {

    [SerializeField] private string levelName;
    [SerializeField] private LoadSceneMode mode;
    [SerializeField] private Vector2 playerStart;

    private void Start()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //change scene when colliding with player
        if (collision.CompareTag("Player"))
        {
            startPosition = playerStart;
            //detatch items from inventory so they can be passed on to the next scene
            foreach(GameObject item in Items.Values)
            {
                item.transform.parent = null;
                DontDestroyOnLoad(item);
            }
            SceneManager.LoadScene(levelName, mode);
        }
    }
}
