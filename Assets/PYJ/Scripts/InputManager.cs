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

    Vector3 beginTouchPoint;
    Vector3 endTouchPoint;

    public float radius = 4f;
    float deadTime = 1f;
    public bool isSwiping = false;

    public float inputDelay = 0.1f;

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

        if (inputDelay > 0f)
        {
            inputDelay -= Time.deltaTime;
        }

        if ((Application.platform == RuntimePlatform.WindowsEditor) ||
            (Application.platform == RuntimePlatform.WindowsPlayer) ||
            (Application.platform == RuntimePlatform.OSXEditor) ||
           (Application.platform == RuntimePlatform.OSXPlayer))
        {
            if (Input.GetMouseButtonUp(0))
            {
                endTouchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            Debug.Log("Window");
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
        else
        {
            if ((Application.platform == RuntimePlatform.WindowsEditor) ||
                (Application.platform == RuntimePlatform.WindowsPlayer) ||
                (Application.platform == RuntimePlatform.OSXEditor) ||
               (Application.platform == RuntimePlatform.OSXPlayer))
            {
                Debug.Log("hi");
                if (inputDelay <= 0f)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        beginTouchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        Vector3 diffVector = transform.position - beginTouchPoint;
                        if (diffVector.sqrMagnitude > 0.3f)
                        {
                            EventSwipe(-diffVector);
                            isSwiping = false;
                        }
                    }
                    inputDelay = 0.1f;
                }
            }
        }
    }

	private void OnMouseUp()
	{
        if (isSwiping == false)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPoint = touch.position;
                Vector3 diffVector = beginTouchPoint - endTouchPoint;
                if (diffVector.sqrMagnitude > 1000f)
                {
                    EventSwipe(-diffVector);
                    isSwiping = false;
                }
            }
        }
	}
}
