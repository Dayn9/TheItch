using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PhysicsObject))]
public class XPathFollow : MonoBehaviour
{
    private PhysicsObject physObj;

    [SerializeField] private PathPosition[] path;
    private int pathIndex = 0;

    private Vector2 moveVector;
    [SerializeField] private int moveSpeed;

    private float pauseTimer;

    // Start is called before the first frame update
    void Awake()
    {
        physObj = GetComponent<PhysicsObject>();
        moveVector = Vector2.zero; 
        
        //make sure path has at least on point
        Assert.IsTrue(path.Length > 0, gameObject.name + "'s path must contain at least one position");
    }

    // Update is called once per frame
    void Update()
    {
        int targetX = path[pathIndex].XCoord;
        moveVector.x = targetX - transform.position.x;
        if (Mathf.Abs(moveVector.x) > 1)
        {
            physObj.InputVelocity = moveVector.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            physObj.InputVelocity = Vector2.zero;
            pauseTimer += Time.deltaTime;
            if(pauseTimer > path[pathIndex].PauseTime)
            {
                pauseTimer = 0; //reset the timer
                pathIndex = (pathIndex + 1) % path.Length; //send to next position in the path
            }
        }
    }
}

[System.Serializable]
public struct PathPosition
{
    [SerializeField] private int xCoord;
    [SerializeField] private float pauseTime;

    public int XCoord { get { return xCoord; } }
    public float PauseTime { get { return pauseTime; } }
}
