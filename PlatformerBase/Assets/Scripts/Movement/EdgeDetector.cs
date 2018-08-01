using UnityEngine;

public delegate void LedgeDetectedMethod(bool right); 

[RequireComponent(typeof(Collider2D))]
public class EdgeDetector : MonoBehaviour {

    private int collisions = 0; //number of objects currently colliding with
    public event LedgeDetectedMethod LedgeDetected; //event triggered when edge detected
    private bool right; //true when detector is on the right side of object

    private void Awake()
    {
        right = GetComponent<Collider2D>().offset.x > 0; //check if on right of player
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collisions == 0 && LedgeDetected != null)
        {
            LedgeDetected(right);
        }
        collisions++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisions--;
        if (collisions == 0 && LedgeDetected != null)
        {
            LedgeDetected(right);
        }
    }
}
