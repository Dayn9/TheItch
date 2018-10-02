using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum DeadzoneCameraType {
    Regular, //keep follow inside the bounds
    Follow, //move towards follow at constant speed + Regular
    LerpFollow, //lerp to follow position + Regular
}

public class DeadzoneCamera : PixelPerfectCamera
{
    #region private fields
    [SerializeField] private Transform follow; //reference to the transform of the object following 

    //bounds of the deadzone box
    [SerializeField] private float rightBound;
    [SerializeField] private float leftBound;
    [SerializeField] private float topBound;
    [SerializeField] private float bottomBound;

    [SerializeField] private DeadzoneCameraType cameraType; //type of camera (see DeadzoneCameraType enum)
    [Range(0, 10.0f)] [SerializeField] private float followSpeed; //percentage of distance to move towards follow

    [SerializeField] private bool snapToPixel;

    private const float minMoveDistance = 0.01f; //minimum distance LerpFollow will travel
    # endregion

    protected override void Start()
    {
        base.Start();
        
        transform.position = follow.position + Vector3.forward * transform.position.z; //snap to follow                
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraType != DeadzoneCameraType.Regular)
        {
            //move towards object following
            Vector3 moveVector = new Vector3(follow.position.x - transform.position.x, follow.position.y - transform.position.y, 0);

            switch (cameraType)
            {
                case DeadzoneCameraType.Follow:
                    if (moveVector.magnitude * Time.deltaTime < followSpeed) { transform.position += moveVector * Time.deltaTime; } //move to follow
                    else { transform.position += moveVector.normalized * followSpeed * Time.deltaTime; } //move towards follow
                    break;
                case DeadzoneCameraType.LerpFollow:
                    if(moveVector.magnitude > minMoveDistance)
                    {
                        transform.position = new Vector3(Mathf.Lerp(transform.position.x, follow.position.x, followSpeed * Time.deltaTime), //Lerp to x position
                                                         Mathf.Lerp(transform.position.y, follow.position.y, followSpeed * Time.deltaTime), //Lerp to y position
                                                         transform.position.z);                                                             //Maintain z position
                    }
                    else
                    {
                        transform.position = follow.position + Vector3.forward * transform.position.z; //snap to player when within min move distance
                    }
                    break;
            }
        }

        //calculate distance to player
        float xDiff = follow.position.x - transform.position.x;
        float yDiff = follow.position.y - transform.position.y;

        //calculate move vector
        Vector3 change = Vector3.zero;
        if (xDiff > rightBound) { change.x = xDiff - rightBound; }
        else if (xDiff < leftBound) { change.x = xDiff - leftBound; }
        if (yDiff > topBound) { change.y = yDiff - topBound; }
        else if (yDiff < bottomBound) { change.y = yDiff - bottomBound; }

        //moveCamera
        transform.position += change;

        if (snapToPixel)
        {
            //round to nearest pixel position
            transform.position = new Vector3(RoundToPixel(transform.position.x), RoundToPixel(transform.position.y), transform.position.z);
        }

        StayInLimits();
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

    /// <summary>
    /// draw the deadzone box in the scene 
    /// </summary>
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        
        //find positions of corners
        Vector2 topLeft = new Vector2(leftBound + transform.position.x, topBound + transform.position.y);
        Vector2 topRight = new Vector2(rightBound + transform.position.x, topBound + transform.position.y);
        Vector2 bottomLeft = new Vector2(leftBound + transform.position.x, bottomBound + transform.position.y);
        Vector2 bottomRight = new Vector2(rightBound + transform.position.x, bottomBound + transform.position.y);

        //draw lines between corners
        Gizmos.DrawLine(topLeft, topRight); //top
        Gizmos.DrawLine(topRight,bottomRight); //right
        Gizmos.DrawLine(bottomRight, bottomLeft); //bottom
        Gizmos.DrawLine(bottomLeft, topLeft); //left
    }
}
