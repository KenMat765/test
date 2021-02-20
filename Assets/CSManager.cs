using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class CSManager : MonoBehaviour
{
    public static bool isTouching;

    //↓　LeftStickとBlast以外でのスワイプのみ検知　↓
    public  bool swipeUp;
    public  bool swipeDown;
    public  bool swipeRight;
    public  bool swipeLeft;
    int detectableNum = 4;
    Touch[] currentTouches;
    Vector2[] startPoses;
    Vector2[] diffPoses;
    float[] swipeSpeeds;

    void Start()
    {
        currentTouches = new Touch[detectableNum];
        startPoses = new Vector2[detectableNum];
        diffPoses = new Vector2[detectableNum];
        swipeSpeeds = new float[detectableNum];
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            float currentTouchCount = Input.touchCount;
            isTouching = true;
            for(int i = 0; i < Mathf.Min(currentTouchCount, detectableNum); i ++)
            {
                Touch touch = Input.GetTouch(i);
                int Id = touch.fingerId;
                if(Id < detectableNum)
                {
                    currentTouches[Id] = touch;
                }
            }
            foreach(Touch currentTouch in currentTouches)
            {
                switch(currentTouch.phase)
                {
                    case TouchPhase.Began:
                    startPoses[Array.IndexOf(currentTouches, currentTouch)] = currentTouch.position;
                    break;

                    case TouchPhase.Moved:
                    diffPoses[Array.IndexOf(currentTouches, currentTouch)] = currentTouch.position - startPoses[Array.IndexOf(currentTouches, currentTouch)];
                    swipeSpeeds[Array.IndexOf(currentTouches, currentTouch)] = currentTouch.deltaPosition.magnitude/currentTouch.deltaTime;
                    break;

                    case TouchPhase.Stationary:
                    swipeSpeeds[Array.IndexOf(currentTouches, currentTouch)] = 0;
                    break;

                    case TouchPhase.Ended:
                    startPoses[Array.IndexOf(currentTouches, currentTouch)] = Vector2.zero;
                    diffPoses[Array.IndexOf(currentTouches, currentTouch)] = Vector2.zero;
                    swipeSpeeds[Array.IndexOf(currentTouches, currentTouch)] = 0;
                    break;
                }
            }
        }
        else
        {
            isTouching = false;
            for(int i = 0; i < detectableNum; i ++)
            {
                startPoses[i] = Vector2.zero;
                diffPoses[i] = Vector2.zero;
                swipeSpeeds[i] = 0;
            }
        }
        SwipeCheker();
    }
    void SwipeCheker()
    {
        float swipeThresh = 500;
        if(swipeSpeeds.Any(s => s > swipeThresh))
        {
            Touch[] swipes = currentTouches.Where(t => t.deltaPosition.magnitude/t.deltaTime > swipeThresh).ToArray();
            foreach(Touch swipe in swipes)
            {
                if(Vector2.SignedAngle(Vector2.up, diffPoses[swipe.fingerId]) >= -45 && Vector2.SignedAngle(Vector2.up, diffPoses[swipe.fingerId]) < 45)
                {
                    swipeUp = true;
                }
                else if(Vector2.SignedAngle(Vector2.up, diffPoses[swipe.fingerId]) >= 45 && Vector2.SignedAngle(Vector2.up, diffPoses[swipe.fingerId]) < 135)
                {
                    swipeRight = true;
                }
                else if(Vector2.SignedAngle(Vector2.up, diffPoses[swipe.fingerId]) >= -135 && Vector2.SignedAngle(Vector2.up, diffPoses[swipe.fingerId]) < -45)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeDown = true;
                }
            }
        }
        else
        {
            swipeUp = false;
            swipeDown = false;
            swipeRight = false;
            swipeLeft = false;
        }
    }
    /*public Touch? TouchGetter(int fingerId)
    {
        foreach(Touch currentTouch in currentTouches)
        {
            if(currentTouch.fingerId == fingerId)
            {
                return currentTouch;
            }
            else
            {
                return null;
            }
        }
    }*/
}
