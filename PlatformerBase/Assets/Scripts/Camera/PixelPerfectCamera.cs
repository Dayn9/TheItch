using UnityEngine;

// modified from: https://github.com/cmilr/DeadSimple-Pixel-Perfect-Camera

public class PixelPerfectCamera : Global {
    [SerializeField] private int verticalUnitsOnScreen = 20;

    void Start()
    {
        MainCamera.GetComponent<Camera>().orthographicSize = Screen.height / (GetNearestMultiple(Screen.height / verticalUnitsOnScreen, pixelsPerUnit) * 2.0f);
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
}
