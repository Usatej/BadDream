using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounder: MonoBehaviour {

    public Collider2D footCollider;
    public LayerMask layer;
    public float offSet;

    private bool grounded = false;

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
        Vector3 leftRayStart;
        Vector3 rightRayStart;

        leftRayStart = rightRayStart = footCollider.bounds.center;

        leftRayStart.x -= footCollider.bounds.extents.x / 2;
      //  leftRayStart.y -= bodyCollider.bounds.size.y / 3;
        rightRayStart.x += footCollider.bounds.extents.x / 2;
      //  rightRayStart.y -= bodyCollider.bounds.size.y / 3;

        Debug.DrawRay(leftRayStart, Vector3.down, Color.red);
        Debug.DrawRay(rightRayStart, Vector3.down, Color.blue);

        if (Physics2D.Raycast(leftRayStart, Vector3.down, (footCollider.bounds.size.y / 2) + offSet, layer.value) ||
            Physics2D.Raycast(rightRayStart, Vector3.down, (footCollider.bounds.size.y / 2) + offSet, layer.value)) grounded = true;
        else grounded = false;
    }
}
