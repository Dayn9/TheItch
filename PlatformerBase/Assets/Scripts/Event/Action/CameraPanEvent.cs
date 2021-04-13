using System.Collections;
using UnityEngine;


//Adapted from: https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
public class CameraPanEvent: MonoBehaviour
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

    private BaseCamera camController; //ref to camera controller 

    [SerializeField] [Range(1, 5)] private float lerpTime = 3f;
    private float currentLerpTime;

    void Start()
    {
        camController = Global.MainCamera.GetComponent<BaseCamera>();

        if (evTrig.State)
        {
            gameObject.SetActive(false);
        }

        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += StartMove;
        }
        else
        {
            evTrig.After += StartMove;
        }
    }

    //called by event, starts the movement
    private void StartMove()
    {
        origin = camController.transform.localPosition;
        currentLerpTime = 0f;
        if (useTarget) {
            final = target.transform.position;
            final.z = origin.z;
        }

        camController.Manual = true; //take over control
        Pause.PauseGame(true);

        StartCoroutine(Move());
    }

    private float Smootherstep(float p) => p * p * p * (p * (6f * p - 15f) + 10f);

    private IEnumerator Move()
    {
        //Move Out
        while(currentLerpTime < lerpTime)
        {
            yield return new WaitForEndOfFrame();
            if (!Pause.menuPaused) //increment timer once per frame
            {
                currentLerpTime += Time.deltaTime;
            }
            if (currentLerpTime >= lerpTime)
            {
                camController.transform.localPosition = final;
                currentHoldTime = 0;
                break;
            }
            else
            {
                float p = currentLerpTime / lerpTime; //calculate percentage
                p = Smootherstep(p); //smootherstep!
                camController.transform.localPosition = Vector3.Lerp(origin, final, p);
            }
        }

        //Hold
        while(currentHoldTime < holdTime)
        {
            yield return new WaitForEndOfFrame();
            if (!Pause.menuPaused) //increment timer once per frame
            {
                currentHoldTime += Time.deltaTime;
            }
            if (currentHoldTime >= holdTime)
            {
                currentLerpTime = 0; //reset the timer and start moving back out
                break;
            }
        }

        //Move Back In
        while (currentLerpTime < lerpTime)
        {
            yield return new WaitForEndOfFrame();
            
            if (!Pause.menuPaused) //increment timer once per frame
            {
                currentLerpTime += Time.deltaTime;
            }
            
            if (currentLerpTime >= lerpTime)
            {
                camController.transform.localPosition = origin;
                camController.Manual = false;
                Pause.PauseGame(false);
                break;
            }
            else
            {
                float p = currentLerpTime / lerpTime; //calculate percentage
                p = Smootherstep(p); //smootherstep!
                camController.transform.localPosition = Vector3.Lerp(final, origin, p);
            }
        }
    }  
}
