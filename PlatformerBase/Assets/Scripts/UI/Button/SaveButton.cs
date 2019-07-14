using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveButton : GenericButton
{
    [SerializeField] private Sprite savedText;
    [SerializeField] private Sprite errorText;

    [SerializeField] private SpriteRenderer message;

    private Color targetColor;

    protected override void Awake()
    {
        base.Awake();

        message.color = new Color(1, 1, 1, 0);
    }

    protected override void OnClick()
    {
        bool gameSave = GameSaver.SaveGameData();
        bool levelSave = GameSaver.SaveLevelData();

        message.sprite = gameSave && levelSave ? savedText : errorText;
        targetColor = Color.white;
    }

    protected override void Update()
    {
        base.Update();

        message.color = Color.Lerp(message.color, targetColor, 0.2f);
        if (message.color == Color.white) { targetColor = new Color(1,1,1,0); }
    }

}
