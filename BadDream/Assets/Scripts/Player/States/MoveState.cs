using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : ObjectState
{
    private Attributes attr;

    private TouchManager touchManager;
    private VirtualJoystick joystick;

    private Animator anim;
    private Rigidbody2D rb;

    private bool jumped = false;
    private bool doubleJumped = false;
    private float fallMultiplier = 2.5f;

    public MoveState(PhaseController _player): base(_player)
    {
        anim = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();

        attr = player.moveAttributes;
        touchManager = player.touchManager;
        joystick = player.joystick;
        Enter();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override ObjectState EarlyUpdate()
    {
        if (!player.grounder.IsGrounded())
        {
            anim.SetBool("grounded", false);
            anim.SetFloat("vSpeed", -rb.velocity.y);
            rb.gravityScale = (rb.velocity.y < 0) ? fallMultiplier : 1;
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x * attr.friction, rb.velocity.y);
            anim.SetBool("grounded", true);
            doubleJumped = jumped = false;
        }
        return null;
    }

    public override ObjectState HandleInput()
    {

        //Movement Horizontal
        if(joystick.IsThereInput())
        {
            float x = joystick.Input.x;
            Debug.Log(x);
            if(x > 0)
            {
                player.transform.localScale = new Vector3(1, 1, 1);
                rb.velocity = new Vector2(attr.moveSpeed * x * Time.deltaTime, rb.velocity.y);
            } else if (x < 0) {
                player.transform.localScale = new Vector3(-1, 1, 1);
                rb.velocity = new Vector2(attr.moveSpeed * x * Time.deltaTime, rb.velocity.y);
            }
            anim.SetFloat("speed", Mathf.Abs(x));
        } else
        {
            anim.SetFloat("speed", 0);
        }

        //Jumping
        if((touchManager.AreaSwipeUp || Input.GetKeyDown(KeyCode.Space)))
        {
            bool inAir = !player.grounder.IsGrounded();
            if (!inAir && !jumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, attr.jumpForce);
                jumped = true;
                Debug.Log("Jump");
            } else if(inAir && !doubleJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, attr.jumpForce * attr.doubleJumpMultiplier);
                doubleJumped = true;
                Debug.Log("DoubleJump");
            }
        }

        return null;
    }

    public override ObjectState Update()
    {
        return null;
    }
}
