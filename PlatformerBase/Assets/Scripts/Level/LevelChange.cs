using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelChange : MonoBehaviour {

    [SerializeField] private string levelName;
    [SerializeField] private LoadSceneMode mode;

    private void Start()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //change scene when colliding with player
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(levelName, mode);
        }
    }
}
