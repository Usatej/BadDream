using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeController : MonoBehaviour {

    private Transform foot;
    private PhaseController pc;
    

    private void Start()
    {
        pc = pc = GameObject.Find("Player").GetComponent<PhaseController>();
        foot = pc.GetComponent<BodyParts>().rightFoot;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(!pc.grounder.IsGrounded())
            {
                
            }
        }
    }
}
