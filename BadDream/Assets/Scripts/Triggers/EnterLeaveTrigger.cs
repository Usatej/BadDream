using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterLeaveTrigger : MonoBehaviour {
    
    public UnityEvent[] eventsEnter;
    public UnityEvent[] eventsLeave;

    private int cnt = 0;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (cnt == 0)
        {
            DoAction();
        }
        cnt++;
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        cnt--;
        if (cnt == 0)
        {
            CancelAction();
        }
    }

    private void DoAction()
    {
        foreach(UnityEvent x in eventsEnter)
        {
            x.Invoke();
        }
    }

    private void CancelAction()
    {
        foreach (UnityEvent x in eventsLeave)
        {
            x.Invoke();
        }
    }

}
