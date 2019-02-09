using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhysicsObject))]
public class CharAnimator : MonoBehaviour
{
    //compnenets in gameobject
    private Animator anim;
    private PhysicsObject physObj;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        physObj = GetComponent<PhysicsObject>();
    }

    private void Update()
    {
        //update the animator based on the physicsObject
        anim.SetBool("grounded", physObj.Grounded);
        anim.SetFloat("verticalVel", physObj.GravityVelocity.magnitude * (Vector2.Angle(physObj.Gravity, physObj.GravityVelocity) > 90 ? 1 : -1));
        anim.SetFloat("horizontalMove", physObj.MoveVelocity.magnitude * (Vector2.Dot(transform.right, physObj.MoveVelocity) < 0 ? 1 : -1));
    }
}
