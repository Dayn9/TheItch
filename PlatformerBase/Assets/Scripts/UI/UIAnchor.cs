using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TO DO: comment

public enum Anchor { topLeft, topCenter, topRight, middleLeft, middleCenter, middleRight, bottomLeft, bottomCenter, bottomRight }

[ExecuteInEditMode] //save position 
public class UIAnchor : Global {

    [SerializeField] private Anchor anchor;
    [SerializeField] private Vector2 offset;
    [SerializeField] private bool snapToPixel;

    /// <summary>
    /// set the fields for the Anchor
    /// </summary>
    /// <param name="anc">Anchor Type</param>
    /// <param name="cam">Main Camera</param>
    /// <param name="off">Offset from Anchor</param>
    /// <param name="snap">snap to pixel grid?</param>
    /// <param name="ppu">Pixels per (unity) unit</param>
    public void Set(Anchor anc, Vector2 off, bool snap)
    {
        anchor = anc;
        offset = off;
        snapToPixel = snap;

        SetPosition();
    }

    public void SetPosition()
    {
        float cameraHeight = MainCamera.GetComponent<Camera>().orthographicSize;
        float cameraWidth = Screen.height != 0 ? Screen.width * cameraHeight / Screen.height : cameraHeight;

        transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, transform.position.z); //move to center of the screen
        //adjust x position of UI element
        switch (anchor)
        {
            //left
            case Anchor.topLeft:
            case Anchor.middleLeft:
            case Anchor.bottomLeft:
                transform.position += Vector3.right * (-cameraWidth + offset.x);
                break;
            //right
            case Anchor.topRight:
            case Anchor.middleRight:
            case Anchor.bottomRight:
                transform.position += Vector3.right * (cameraWidth + offset.x);
                break;
            //center
            default:
                transform.position += Vector3.right * offset.x;
                break;
        }

        //adjust y position of UI element
        switch (anchor)
        {
            //up
            case Anchor.topLeft:
            case Anchor.topCenter:
            case Anchor.topRight:
                transform.position += Vector3.up * (cameraHeight + offset.y);
                break;
            //down
            case Anchor.bottomLeft:
            case Anchor.bottomCenter:
            case Anchor.bottomRight:
                transform.position += Vector3.up * (-cameraHeight + offset.y);
                break;
            //middle
            default:
                transform.position += Vector3.up * offset.y;
                break;
        }

        if (snapToPixel)
        {
            //round to nearest pixel position
            transform.position = new Vector3(RoundToPixel(transform.position.x), RoundToPixel(transform.position.y), transform.position.z);
        }
    }

    void Awake () {
        SetPosition();
    }

    /// <summary>
    /// round to nearest value divisible by pixel size
    /// </summary>
    /// <param name="origional">value to round</param>
    /// <returns>rounded value</returns>
    private float RoundToPixel(float origional)
    {
        float pixelSize = 1.0f / pixelsPerUnit;
        return origional - (origional % pixelSize) + (origional % pixelSize > pixelSize / 2 ? pixelSize : 0);
    }
}
