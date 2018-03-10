using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounder: MonoBehaviour {

    public Collider2D footCollider;
    public LayerMask layer; 
    public float offSet;

    private bool grounded = false;
    private bool onMovingObject = false;
    private Vector2 objectVelocity;

    public Grounder()
    {
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    private void Update()
    {
        if (footCollider == null) Debug.LogError("Grounder collider is not initialized");
        Vector2 rayStart;

        rayStart = new Vector2(footCollider.bounds.center.x, footCollider.bounds.center.y - footCollider.bounds.size.y/2);

        Vector2 size = new Vector2 (footCollider.bounds.size.x + 2*offSet, 2*offSet);
        RaycastHit2D hit = Physics2D.BoxCast(rayStart, size, 0, Vector2.right, 0f,layer.value);
        if (hit)
        {
            if (hit.collider.tag == "Moving")
            {
                objectVelocity = hit.collider.GetComponent<Rigidbody2D>().velocity; 
                onMovingObject = true;
            }
            else onMovingObject = false;
            grounded = true;

        }
        else
        {
            grounded = false;
            onMovingObject = false;
        }
    }

    public bool IsOnMovingObject()
    {
        return onMovingObject;
    }

    public Vector2 MovingObjectVelocity()
    {
        return objectVelocity;
    }
}
