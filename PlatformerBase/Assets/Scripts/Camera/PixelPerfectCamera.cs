using UnityEngine;

// modified from: https://github.com/cmilr/DeadSimple-Pixel-Perfect-Camera

public class PixelPerfectCamera : MonoBehaviour {

    [SerializeField] protected int pixelsPerUnit = 8;
    [SerializeField] private int verticalUnitsOnScreen = 20;

    void Start()
    {
        gameObject.GetComponent<Camera>().orthographicSize = Screen.height / (GetNearestMultiple(Screen.height / verticalUnitsOnScreen, pixelsPerUnit) * 2.0f);
    }

    int GetNearestMultiple(int value, int multiple)
    {
        int rem = value % multiple;
        int result = value - rem;
        if (rem > (multiple / 2))
            result += multiple;

        return result;
    }
}
