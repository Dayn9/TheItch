using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelChange : EventTrigger
{
    [SerializeField] private string levelName;
    [SerializeField] private LoadSceneMode mode;
    [Header("don't make (1, 1)")]
    [SerializeField] private Vector2 playerStart;

    [SerializeField] private bool resetToMenu = false; //for demo builds that need to reset whole game

    private bool changing = false;

    private void Start()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;

        Transition transition = FindObjectOfType<Transition>();
        transition.After += Advance;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //change scene when colliding with player
        if (collision.CompareTag("Player"))
        {
            changing = true;
            //start the transition
            Before?.Invoke();
        }
    }

    private void Advance()
    {
        if (changing) //only one level change should happen on advance
        {
            if (resetToMenu)
            {
                GameSaver.RemoveDirectory();
                GetComponent<Reseter>().ResetGame();
                return;
            }

            startPosition = playerStart;

            Items.Clear();

            SceneManager.LoadScene(levelName, mode);
        }
    }

}
