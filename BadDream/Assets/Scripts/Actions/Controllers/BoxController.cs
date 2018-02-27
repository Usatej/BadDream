using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {

    bool isHere = false;
    PhaseController pc;

    private void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PhaseController>();
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        
        if (!isHere && coll.tag == "Hands" && pc.actualAction == null)
        {
                float xBox = transform.position.x;
                float xPl = coll.transform.root.position.x;
                if (Mathf.Sign(coll.transform.root.localScale.x) > 0)
                {
                    if (xBox > xPl)
                    {
                        isHere = true;
                    Debug.Log("LoL");
                       pc.viableAction = new BoxAction(this.gameObject, coll.transform.root.gameObject);
                    }
                }
                else
                {
                    if (xPl > xBox)
                    {
                    Debug.Log("LoL2");
                    isHere = true;
                        pc.viableAction = new BoxAction(this.gameObject, coll.transform.root.gameObject);
                    }
                }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (isHere && coll.tag == "Player")
        {
            isHere = false;
            pc.viableAction = null;
        }
    } 
}
