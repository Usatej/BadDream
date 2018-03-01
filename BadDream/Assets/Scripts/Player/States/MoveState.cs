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

    private IKController iks;

    public MoveState(PhaseController _player): base(_player)
    {
        anim = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
        iks = player.GetComponent<IKController>();

        attr = player.attributes;
        touchManager = player.touchManager;
        joystick = player.joystick;
        Enter();
    }

    public override void Enter()
    {
    }

    public override void EarlyUpdate()
    {
        if (!player.grounder.IsGrounded())
        {
            anim.SetBool("grounded", false);
            anim.SetFloat("vSpeed", -rb.velocity.y);
            rb.gravityScale = (rb.velocity.y < 0) ? fallMultiplier : 1;
        }
        else
        {
            anim.SetBool("grounded", true);
            doubleJumped = jumped = false;
        }
    }

    public override void HandleInput()
    {

        //Movement Horizontal
        if(joystick.IsThereInput() && player.grounder.IsGrounded())
        {
            float x = joystick.Input.x;

            if (player.grounder.IsOnMovingObject())
            {
                Vector2 tmp = player.grounder.MovingObjectVelocity();
                rb.velocity = new Vector2(attr.moveSpeed * x + tmp.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(attr.moveSpeed * x, rb.velocity.y);
            }

            if (x > 0)
            {
                player.transform.localScale = new Vector3(1, 1, 1);
                
            } else if (x < 0) {
                player.transform.localScale = new Vector3(-1, 1, 1);
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
            } else if(inAir && !doubleJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x, attr.jumpForce * attr.doubleJumpMultiplier);
                doubleJumped = true;
            }
        }
    }

    public override void Update()
    {
        Vector2 handPos = Vector2.zero;
        if(FindPushObject(out handPos) && player.grounder.IsGrounded())
        {
            iks.rightArmPull.ik.gameObject.SetActive(true);
            iks.leftArmPull.ik.gameObject.SetActive(true);
            iks.rightArmPull.ik.transform.position = iks.leftArmPull.ik.transform.position = handPos;
            anim.SetBool("pulling", true);
            anim.SetFloat("speed", Mathf.Abs(joystick.Input.x) * -1);
        } else
        {
            iks.rightArmPull.ik.gameObject.SetActive(false);
            iks.leftArmPull.ik.gameObject.SetActive(false);
            anim.SetBool("pulling", false);
        }
    }

    private bool FindPushObject(out Vector2 pos)
    {
        
        Vector3 rayStart = iks.rightArmPull.centerPoint.position;
        Debug.DrawRay(rayStart, Vector3.right * player.transform.localScale.x);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector3.right * player.transform.localScale.x, 0.7f, player.boxLayer.value);
        if (hit)
        {
            pos = hit.point;
            return true;
        }
        pos = Vector2.zero;
        return false;
    }
}
