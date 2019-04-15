using UnityEngine;
[RequireComponent(typeof(Animator))]
public class CursorBlinker : MonoBehaviour
{
    [SerializeField] private bool right; //true = cursor Right animation / false = cursor Left animation
    void Awake() { GetComponent<Animator>().SetBool("Right", right); } //set the animator bool 'Right'
}
