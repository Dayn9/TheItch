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

    [SerializeField] private EventTrigger evTrig;

    private void Start()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        evTrig.After += new triggered(Advance);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //change scene when colliding with player
        if (collision.CompareTag("Player"))
        {
            //start the transition
            CallBefore();
            
        }
    }

    private void Advance()
    {
        if (resetToMenu)
        {
            GetComponent<Reseter>().ResetGame();
            return;
        }

        startPosition = playerStart;
        //detatch items from inventory so they can be passed on to the next scene
        foreach (GameObject item in Items.Values)
        {
            item.transform.parent = null;
            DontDestroyOnLoad(item);
        }
        SceneManager.LoadScene(levelName, mode);
    }

}
