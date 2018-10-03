using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelChange : MonoBehaviour {

    [SerializeField] string levelName;
    [SerializeField] LoadSceneMode mode;

    private void Start()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(levelName, mode);
    }
}
