using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TouchManager: MonoBehaviour { 
    
    public enum TouchTypes
    {
        tap = 1,
        swipeLeft = 2,
        swipeRight = 4,
        swipeUp = 8,
        swipeDown = 16,
        areaTap = 32,
        areaSwipeLeft = 64,
        areaSwipeRight = 128,
        areaSwipeUp = 256,
        areaSwipeDown = 512,
        areaHolding = 1028
    }

    private class TouchInfo
    {
        public Vector2 start;
        public Vector2 end;
        public float startTime = 0;
        public float endTime = 0;
    }
    
    public float minLength = Screen.width / 4;
    public float maxTime = 0.5f;

    Dictionary<int, TouchInfo> actTouches;


    bool tap, swipeLeft, swipeRight, swipeUp, swipeDown, areaTap, areaSwipeLeft, areaSwipeRight, areaSwipeUp, areaSwipeDown, areaHolding;

    public bool Tap { get { return tap; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    public bool AreaTap { get { return areaTap; } }
    public bool AreaSwipeLeft { get { return areaSwipeLeft; } }
    public bool AreaSwipeRight { get { return areaSwipeRight; } }
    public bool AreaSwipeUp { get { return areaSwipeUp; } }
    public bool AreaSwipeDown { get { return areaSwipeDown; } }
    public bool AreaHolding { get { return areaHolding; } }


    void Start () {
        actTouches = new Dictionary<int, TouchInfo>();
    }
    
    
	void Update () {
        ResetManager();
        HandleTouches();
	}

    void HandleTouches()
    {       
        foreach (Touch t in Input.touches)
        {
            if (TestPositionInArea(t.position)) areaHolding = true;
            if (t.phase == TouchPhase.Began)
            {
                TouchInfo tmp = new TouchInfo();
                tmp.start = t.position;
                tmp.startTime = Time.time;
                if (actTouches.ContainsKey(t.fingerId))
                {
                    actTouches[t.fingerId] = tmp;
                }
                else
                {
                    actTouches.Add(t.fingerId, tmp);
                }
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                actTouches[t.fingerId].end = t.position;
                actTouches[t.fingerId].endTime = Time.time;
                DecideByTouchInfo(actTouches[t.fingerId]);
                actTouches.Remove(t.fingerId);
            }
        }
    }

    private void DecideByTouchInfo(TouchInfo t)
    {
        float deltaTime = t.endTime - t.startTime;
        if (deltaTime > maxTime) return;
        Vector2 delta = t.start - t.end;
        if (delta.magnitude < minLength)
        {
            if (TestPositionInArea(t.start)) areaTap = true;
            else tap = true;
        } else
        {
            if(Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x < 0) swipeRight = areaSwipeRight = true;
                else swipeLeft = areaSwipeLeft = true;
            } else
            {
                if (delta.y < 0) swipeUp = areaSwipeUp = true;
                else swipeDown = areaSwipeDown = true;
            }
            if (!TestPositionInArea(t.start)) areaSwipeLeft = areaSwipeRight = areaSwipeUp = areaSwipeDown = false;
            else areaHolding = false;
        }
    }

    private bool TestPositionInArea(Vector2 pos)
    {
        return pos.x >= (Screen.width / 2);
    }

    public void ResetManager()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = areaTap = areaSwipeLeft = areaSwipeRight = areaSwipeUp = areaSwipeDown = areaHolding = false;
    }
}
