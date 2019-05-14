
public class GenericButton : Button
{
    /// <summary>
    /// used for buttons that hava an active and inactive sprite
    /// </summary>
    
    protected override void OnActive()
    {
        render.sprite = inactive;
    }

    protected override void OnClick()
    {
        //nothing happens
    }

    protected override void OnEnter()
    {
        render.sprite = active;
    }
}
