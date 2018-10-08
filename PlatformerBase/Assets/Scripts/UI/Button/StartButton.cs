using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : Button
{
    [SerializeField] private string levelName;
    [SerializeField] private LoadSceneMode mode;

    protected override void OnActive()
    {
        buttonRender.sprite = inactive;
    }

    protected override void OnClick()
    {
        SceneManager.LoadScene(levelName, mode);
    }

    protected override void OnEnter()
    {
        buttonRender.sprite = active;
    }
}
