using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {

    bool isHere = false;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(!isHere && coll.tag == "Player")
        {
            isHere = true;
            coll.GetComponent<PhaseController>().actualAction = new BoxAction(this.gameObject, coll.gameObject);
            Debug.Log("LOL");
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (isHere && coll.tag == "Player")
        {
        //    isHere = false;
        //    coll.GetComponent<PhaseController>().actualAction = null;
            //Debug.Log("DONE");
        }
    } 
}
