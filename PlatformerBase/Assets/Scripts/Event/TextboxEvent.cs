using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueBox))]
[RequireComponent(typeof(SpriteRenderer))]
public class TextboxEvent : EventTrigger
{
    [SerializeField] private ZoneDialogueTrigger evTrig; //eventTrigger 

    [SerializeField] private Vector3 animatedOffset;
    [SerializeField] private float speed;

    private Vector3 targetPos;
    private Vector2 moveVector; //temp vector to the target 

    [SerializeField] private Color initialColor;
    [SerializeField] private Color finalColor;

    private bool moveIn = false;
    private bool moveOut = false;

    private SpriteRenderer render;

    [SerializeField] private float maxTime;
    private float timer;

    void Start()
    {
        evTrig.Before += new triggered(MoveIn);
        render = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        if (!paused)
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
            else if(timer < maxTime)
            {
                timer += Time.deltaTime;
                //temp brake trigger
                if (timer >= maxTime || Input.GetAxis("Vertical") < -buffer)
                {
                    MoveOut();
                }
            }
        }
    }

    /// <summary>
    /// called by event, sets acivation state
    /// </summary>
    private void MoveIn()
    {
        targetPos = transform.localPosition;
        transform.localPosition += animatedOffset;

        moveIn = true;
    }

    private void MoveOut()
    {
        targetPos = transform.localPosition + animatedOffset;
        moveOut = true;
    }

    private void SetAlpha(float percentage)
    {
        Color tempColor = render.color;
        tempColor.a = (percentage * finalColor.a) + ((1 - percentage) * initialColor.a);
        render.color = tempColor;
    }
}
