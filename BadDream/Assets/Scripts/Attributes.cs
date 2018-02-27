using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attributes
{
    public float moveSpeed = 250;
    public float friction = 0.8f;

    [Range(1, 10)]
    public float jumpForce = 10;
    [Range(0, 1)]
    public float doubleJumpMultiplier = 0.6f;
    public float pushSpeed = 150;

}
