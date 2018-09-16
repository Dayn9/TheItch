using UnityEngine;

// modified from: https://github.com/cmilr/DeadSimple-Pixel-Perfect-Camera

public class PixelPerfectCamera : Global {
    
    [SerializeField] private int verticalUnitsOnScreen = 20;

    //global limits of the camera
    [SerializeField] private int rightLimit;
    [SerializeField] private int leftLimit;
    [SerializeField] private int bottomLimit;
    [SerializeField] private int topLimit;

    private float width;
    private float height;

    //find positions of corners
    Vector2 topLeft;
    Vector2 topRight;
    Vector2 bottomLeft;
    Vector2 bottomRight;

    protected virtual void Start()
    {
        MainCamera.GetComponent<Camera>().orthographicSize = Screen.height / (GetNearestMultiple(Screen.height / verticalUnitsOnScreen, pixelsPerUnit) * 2.0f);

        height = MainCamera.GetComponent<Camera>().orthographicSize;
        width = height * Screen.width / Screen.height;

        topLeft = new Vector2(leftLimit + transform.position.x, topLimit + transform.position.y);
        topRight = new Vector2(rightLimit + transform.position.x, topLimit + transform.position.y);
        bottomLeft = new Vector2(leftLimit + transform.position.x, bottomLimit + transform.position.y);
        bottomRight = new Vector2(rightLimit + transform.position.x, bottomLimit + transform.position.y);
    }

    //black magic
    int GetNearestMultiple(int value, int multiple)
    {
        int rem = value % multiple;
        int result = value - rem;
        if (rem > (multiple / 2))
            result += multiple;
        return result;
    }

    protected void StayInLimits()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit + width, rightLimit - width),
                                         Mathf.Clamp(transform.position.y, bottomLimit + height, topLimit - height),
                                         transform.position.z);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;  

        //draw lines between corners
        Gizmos.DrawLine(topLeft, topRight); //top
        Gizmos.DrawLine(topRight, bottomRight); //right
        Gizmos.DrawLine(bottomRight, bottomLeft); //bottom
        Gizmos.DrawLine(bottomLeft, topLeft); //left
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        topLeft = new Vector2(leftLimit + transform.position.x, topLimit + transform.position.y);
        topRight = new Vector2(rightLimit + transform.position.x, topLimit + transform.position.y);
        bottomLeft = new Vector2(leftLimit + transform.position.x, bottomLimit + transform.position.y);
        bottomRight = new Vector2(rightLimit + transform.position.x, bottomLimit + transform.position.y);

        //draw lines between corners
        Gizmos.DrawLine(topLeft, topRight); //top
        Gizmos.DrawLine(topRight, bottomRight); //right
        Gizmos.DrawLine(bottomRight, bottomLeft); //bottom
        Gizmos.DrawLine(bottomLeft, topLeft); //left
    }
}