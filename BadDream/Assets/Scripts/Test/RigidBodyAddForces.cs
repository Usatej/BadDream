using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyAddForces : MonoBehaviour {

    Rigidbody2D rb;

    public Vector2 force;
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.J))
        {
            rb.AddForce(force * -1);
        }

        if (Input.GetKey(KeyCode.L))
        {
            rb.AddForce(force);
        }
    }
}
