using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Grandma : PhysicsObject {

    [SerializeField] private float moveSpeed; //how fast the object can move
    [SerializeField] private float minDistanceToPlayer; //distance at which the object will stop moving towards the player
    [SerializeField] private Transform player; //ref to transform component of player object 

    private float distanceToPlayer; //distance from object to player
    private float width = 0; //width of the object
    private float height = 0; //height of the object

    private void Awake()
    {
        Vector2[] points = GetComponent<PolygonCollider2D>().points;
        if(points != null)
        {
            Vector2 min = Vector2.zero;
            Vector2 max = Vector2.zero;
            for (int i = 0; i < points.Length; i++)
            {
                min.x = points[i].x < min.x ? points[i].x : min.x;
                min.y = points[i].y < min.y ? points[i].y : min.y;
                max.x = points[i].x > max.x ? points[i].x : max.x;
                max.y = points[i].y > max.y ? points[i].y : max.y;
            }
            width = Mathf.Abs(max.x - min.x);
            height = Mathf.Abs(max.y - min.y);
        }
    }

    // Update is called once per frame
    void Update () {
        moveVelocity = Vector2.zero; //reset moveVelocity every update
        distanceToPlayer = player.position.x - transform.position.x; //find distance to player
        //move towards player horizontally
        if (Mathf.Abs(distanceToPlayer) > minDistanceToPlayer)
        {
            float sign = Mathf.Sign(distanceToPlayer);
            Vector2 castVector = new Vector2((width * sign), (-height));
            Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + castVector);
            float numCollisions = rb2D.Cast(castVector, filter, hits, castVector.magnitude);
            if(numCollisions > 0)
            {
                moveVelocity = Vector2.right * sign * moveSpeed * Time.deltaTime;
            }
        }
    }
}
