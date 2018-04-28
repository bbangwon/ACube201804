using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour {
    static InputManager instance;
    public static InputManager Instance {
        get { return instance; }
    }

    public Action<Vector3> EventSwipe;

    public CircleCollider2D beginTrigger;

    Vector2 beginTouchPoint;
    Vector2 endTouchPoint;

    public float radius = 2f;
    float deadTime = 0.5f;
    public bool isSwiping = false;

	private void Awake()
	{
        instance = this;

        beginTrigger = GetComponent<CircleCollider2D>();
        beginTrigger.radius = radius;
	}

	private void Update()
	{
        if(isSwiping){
            deadTime -= Time.deltaTime;
        }
        if(deadTime <= 0f){
            isSwiping = false;
        }
	}

	private void OnMouseDown()
    {
        isSwiping = true;
        deadTime = 1f;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                beginTouchPoint = touch.position;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            beginTouchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }


    }

    private void OnMouseUp()
    {
        
        if(isSwiping == false){
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPoint = touch.position;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            endTouchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 diffVector = beginTouchPoint - endTouchPoint;
            if (diffVector.sqrMagnitude > 2f){
                EventSwipe(-diffVector);
            }
        }
    }
}
