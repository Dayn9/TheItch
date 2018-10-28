using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueBox))]
[RequireComponent(typeof(SpriteRenderer))]
public class TextboxEvent : EventTrigger
{
    [SerializeField] private EventTrigger[] evTrigs; //eventTrigger 

    [SerializeField] private Vector3 animatedOffset;
    [SerializeField] private float speed;

    private Vector3 visiblePosition; //position to reset to
    private Vector3 targetPos;
    private Vector2 moveVector; //temp vector to the target 

    [SerializeField] private Color initialColor;
    [SerializeField] private Color finalColor;

    private bool moveIn = false;
    private bool moveOut = false;

    private SpriteRenderer render;

    [SerializeField] private bool useTimer;
    [SerializeField] private float maxTime;
    private float timer;

    void Start()
    {
        foreach(EventTrigger evTrig in evTrigs)
        {
            evTrig.Before += new triggered(MoveIn);
            evTrig.After += new triggered(MoveIn);
        }        
        render = GetComponent<SpriteRenderer>();

        visiblePosition = transform.localPosition;
        transform.localPosition = visiblePosition + animatedOffset;
        timer = maxTime + 1;
    }

    protected override void Update()
    {
        if (!paused || true)
        {
            if (moveIn || moveOut)
            {
                moveVector = targetPos - transform.localPosition;
                //snap into position when close enough
                if (moveVector.magnitude < speed * Time.deltaTime)
                {
                    transform.localPosition = targetPos;
                    if (moveIn)
                    {
                        CallBefore();
                        timer = 0;
                    }
                    moveIn = false;
                    if (moveOut)
                    {
                        CallAfter();
                        GetComponent<DialogueBox>().Reset();
                        transform.localPosition = visiblePosition + animatedOffset; //reset the localPosition
                    }
                    moveOut = false;
                    SetAlpha(1.0f);
                }
                else
                {
                    transform.localPosition += (Vector3)(moveVector.normalized * speed * Time.deltaTime);//move at speed along moveVector

                    SetAlpha(1 - (((Vector2)(transform.localPosition - targetPos)).magnitude / animatedOffset.magnitude));
                }
            }
            else if(useTimer && timer < maxTime)
            {
                timer += Time.deltaTime;
                //temp brake trigger
                if (timer >= maxTime)
                {
                    MoveOut();
                }
            }
        }
    }

    /// <summary>
    /// starts the process of moving onto screen
    /// </summary>
    public void MoveIn()
    {
        targetPos = visiblePosition;
        transform.localPosition = visiblePosition + animatedOffset;

        moveIn = true;
    }

    /// <summary>
    /// starts the process of moving off of screen
    /// </summary>
    public void MoveOut()
    {
        targetPos = visiblePosition + animatedOffset;
        moveOut = true;
    }

    private void SetAlpha(float percentage)
    {
        Color tempColor = render.color;
        tempColor.a = (percentage * finalColor.a) + ((1 - percentage) * initialColor.a);
        render.color = tempColor;
    }
}
