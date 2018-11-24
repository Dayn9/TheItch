using UnityEngine;

public class SettingsButton : GenericButton {

    private Settings settings;

    [SerializeField] private bool display;

    protected override void Awake()
    {
        base.Awake();
        settings = FindObjectOfType<Settings>();
    }

    protected override void OnClick()
    {
        settings.DisplaySettings(display);
    }

}
