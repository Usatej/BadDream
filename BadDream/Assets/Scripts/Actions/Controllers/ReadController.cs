using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadController : MonoBehaviour
{

    public Image plane;

    private PhaseController pc;
    private bool isHere = false;

    private int cnt = 0;
    private ReadAction act;

    private void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PhaseController>();
        plane.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Enter");
        if (cnt == 0)
        {
            Debug.Log("Add");
            act = new ReadAction(this.gameObject, pc.gameObject);
            pc.viableAction.Add(act);
            Debug.Log("Added");
        }
        cnt++;
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        cnt--;
        if (cnt == 0)
        {
            Debug.Log("Leave");
            pc.viableAction.Remove(act);
        }
    }
}

