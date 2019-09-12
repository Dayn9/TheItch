using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickMouse : MonoBehaviour
{
    public static bool Active = true;
    private static Vector2 localPos = new Vector2(0, 0);
    public static Vector2 Pos;

    private const float z = 10;

    private static SpriteRenderer render;

    private void Awake()
    {
        transform.localPosition = new Vector3(localPos.x, localPos.y, z);
        Cursor.visible = !Active;
        GetComponent<SpriteRenderer>().enabled = Active;

        Pos = transform.position;
        render = GetComponent<SpriteRenderer>();
    }

    public static void SetSprite(Sprite s)
    {
        if(render != null)
        {
            render.sprite = s;
        }
    }

    private void Update()
    {
        if(Mathf.Abs(Input.GetAxis("JoyX")) > 0 || Mathf.Abs(Input.GetAxis("JoyY")) > 0)
        {
            Cursor.visible = false;
            GetComponent<SpriteRenderer>().enabled = true;
            Active = true;
        }
        if(Mathf.Abs(Input.GetAxis("Mouse X")) > 0 || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0)
        {
            Cursor.visible = true;
            GetComponent<SpriteRenderer>().enabled = false;
            Active = false;
        }
        if (Active)
        {
            localPos = new Vector3(Mathf.Clamp(localPos.x + Input.GetAxis("JoyX"), -160.0f/8, 160.0f/8),
                              Mathf.Clamp(localPos.y + Input.GetAxis("JoyY"), -90.0f/8, 90.0f/8), 10);
            transform.localPosition = new Vector3(localPos.x, localPos.y, z);

            Pos = transform.position;
        }
    }
}
