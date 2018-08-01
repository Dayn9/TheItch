using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TO DO: create a custom inspector to only show height and width if poly
//TO DO: create an object class with buffer as a shared constant

/// <summary>
/// removes the buffer around an object for better collision
/// </summary>
public class DebufferCollider : MonoBehaviour {

    [SerializeField] private float buffer; //amount to remove

	void Start () {
        Debuffer();
    }

    public void Debuffer()
    {
        //box collider
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            box.offset += new Vector2(buffer / 2, buffer / 2);
            box.size -= new Vector2(buffer, buffer);
        }

        //circle collider
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        if (circle != null)
        {
            circle.offset += new Vector2(buffer / 2, buffer / 2);
            circle.radius -= buffer;
        }

        //polygon collider
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        if (poly != null)
        {
            Vector2[] newPoints = new Vector2[poly.points.Length]; //temporary array of point locations (SetPath requires Vector2[])
            for (int i = 0; i < poly.points.Length; i++)
            {
                //set new point to old point move [buffer] distance towards center
                newPoints[i] = poly.points[i] - poly.points[i].normalized * (buffer / 2);
                //newPoints[i] = new Vector2(poly.points[i].x - ((buffer / 2) * Mathf.Sign(poly.points[i].x)), poly.points[i].y - ((buffer / 2) * Mathf.Sign(poly.points[i].y)));
            }
            poly.SetPath(0, newPoints);
        }
    }
}
