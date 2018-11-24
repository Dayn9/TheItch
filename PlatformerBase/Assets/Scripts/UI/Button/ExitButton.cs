using UnityEngine;

public class ExitButton : GenericButton
{
    protected override void OnClick()
    {
        Application.Quit();
    }
}