using UnityEngine;

public class CursorChange : MonoBehaviour
{
    /// <summary>
    /// Handles all the cursor setting
    /// </summary>

    //Possible cursor textures
    [SerializeField] private Texture2D normal;
    [SerializeField] private Texture2D hover;
    [SerializeField] private Texture2D left;
    [SerializeField] private Texture2D right;
    [SerializeField] private Texture2D both;

    //set by the transfer trigger when hovering over an available transfer point
    private bool hovering = false;
    public bool Hovering { set { hovering = value; } }

    private void Update()
    {
        //get mouse input
        bool leftMouse = Input.GetMouseButton(0);
        bool rightMouse = Input.GetMouseButton(1);

        //evalute cursor based on mouse input then hovering then default
        if(leftMouse && rightMouse)
        {
            Cursor.SetCursor(both, Vector2.zero, CursorMode.Auto);
        }
        else if (leftMouse && !rightMouse)
        {
            Cursor.SetCursor(left, Vector2.zero, CursorMode.Auto);
        }
        else if(rightMouse && !leftMouse)
        {
            Cursor.SetCursor(right, Vector2.zero, CursorMode.Auto);
        }
        else if (hovering) 
        {
            Cursor.SetCursor(hover, Vector2.zero, CursorMode.Auto);
        }
        else //defaul to normal
        {
            Cursor.SetCursor(normal, Vector2.zero, CursorMode.Auto);
        }
    }
}
