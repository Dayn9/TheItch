using UnityEngine;


//Adapted from: https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
public class CameraPanEvent : Pause
{
    [SerializeField] private EventTrigger evTrig; //eventTrigger 

    [Header("Before/True  -  After/False")]
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    private Vector3 origin; //position moving from
    [SerializeField] private Vector3 final; //position moving to
    [SerializeField] private bool useTarget;
    [SerializeField] private Transform target;

    [SerializeField] [Range(1, 5)] private float holdTime = 3f;
    private float currentHoldTime;

    private bool move = false;
    private bool movingOut = true;
    private BaseCamera camController; //ref to camera controller 

    [SerializeField] [Range(1, 5)] private float lerpTime = 3f;
    private float currentLerpTime;

    void Start()
    {
        camController = MainCamera.GetComponent<BaseCamera>();

        if (evTrig.State)
        {
            gameObject.SetActive(false);
        }

        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += Move;
        }
        else
        {
            evTrig.After += Move;
        }
    }

    //called by event, starts the movement
    private void Move()
    {
        move = true;
        //movingOut = true;

        origin = camController.transform.localPosition;
        currentLerpTime = 0f;
        if (useTarget) {
            final = target.transform.position;
            final.z = origin.z;
        }

        camController.Manual = true; //take over control
        PauseGame(true);
    }

    private void Update()
    {
        if (!menuPaused)
        {
            if (move && currentLerpTime < lerpTime)
            {
                //increment timer once per frame
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime > lerpTime)
                {
                    currentLerpTime = lerpTime;
                }

                float p = currentLerpTime / lerpTime; //calculate percentage
                if (p == 1)
                {
                    if (movingOut)
                    {
                        camController.transform.localPosition = final;
                        movingOut = false;
                        currentHoldTime = 0;
                    }
                    else
                    {
                        camController.transform.localPosition = origin;
                        move = false;
                        camController.Manual = false;
                        PauseGame(false);
                    }
                }
                else
                {
                    p = p * p * p * (p * (6f * p - 15f) + 10f); //smootherstep!
                    if (movingOut)
                    {
                        camController.transform.localPosition = Vector3.Lerp(origin, final, p);
                    }
                    else
                    {
                        camController.transform.localPosition = Vector3.Lerp(final, origin, p);
                    }
                    
                }
            }
            else if (move && currentHoldTime < holdTime)
            {
                currentHoldTime+= Time.deltaTime;
                if (currentHoldTime > holdTime)
                {
                    currentLerpTime = 0; //reset the timer and start moving back out
                }
            }
        }
    }
}
