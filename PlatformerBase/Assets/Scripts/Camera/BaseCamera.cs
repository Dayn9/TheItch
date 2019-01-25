using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(PixelPerfectCamera))]
public class BaseCamera : Global {

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

    private PixelPerfectCamera ppc;
    private IPlayer player;

    protected bool manual = false; //true when external script is controlling camera

    public float CamHeight {
        get {
            if(ppc == null)
            {
                GetCameraProperties();
            }
            return height;
        }
    }
    public float CamWidth {
        get {
            if (ppc == null)
            {
                GetCameraProperties();
            }
            return width;
        }
    }
    public bool Manual { set { manual = value; } }

    private void GetCameraProperties()
    {
        ppc = GetComponent<PixelPerfectCamera>();
        width = ppc.refResolutionX / (2.0f * pixelsPerUnit);
        height = ppc.refResolutionY / (2.0f * pixelsPerUnit);
    }

    protected virtual void Start()
    {
        GetCameraProperties();

        topLeft = new Vector2(leftLimit + transform.position.x, topLimit + transform.position.y);
        topRight = new Vector2(rightLimit + transform.position.x, topLimit + transform.position.y);
        bottomLeft = new Vector2(leftLimit + transform.position.x, bottomLimit + transform.position.y);
        bottomRight = new Vector2(rightLimit + transform.position.x, bottomLimit + transform.position.y);

        player = Player.GetComponent<IPlayer>();
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
        KeepPlayerInLimits();
    }

    protected void KeepPlayerInLimits()
    {
        //keep the player withing the horizontal limits 
        Player.transform.position = new Vector2(Mathf.Clamp(Player.transform.position.x, leftLimit + 0.5f, rightLimit - 0.5f), Player.transform.position.y);
        //check if player has fallen past the bottom limit of the map
        if (Player.transform.position.y < bottomLimit - 1.0f)
        {
            player.OnPlayerFall();
        }
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
