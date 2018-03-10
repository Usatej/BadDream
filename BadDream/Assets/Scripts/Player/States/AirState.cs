using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : ObjectState
{
    private bool doubleJumped = false;
    private float fallMultiplier = 2.5f;

    private Animator anim;
    private Rigidbody2D rb;

    public AirState(PhaseController _player) : base(_player)
    {
        anim = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
        Enter();
    }

    public override void Enter()
    {
        anim.SetBool("grounded", false);
    }

    public override void EarlyUpdate()
    {
        if (!player.grounder.IsGrounded())
        {
            anim.SetFloat("vSpeed", -rb.velocity.y);
            rb.gravityScale = (rb.velocity.y < 0) ? fallMultiplier : 1;
            
        }
        else
        {
            player.actualPhase.SendRequestToCreateState(PlayerStates.Move);
        }
    }

    public override void HandleInput()
    {
        if ((player.touchManager.AreaSwipeUp || Input.GetKeyDown(KeyCode.Space)) && !doubleJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, player.attributes.jumpForce * player.attributes.doubleJumpMultiplier);
            doubleJumped = true;
        }
        if(player.touchManager.AreaHolding || Input.GetKey(KeyCode.F))
        {
            HangUpdate();
        }
    }

    public override void Update()
    {
    }

    private void HangUpdate()
    {
        if (player.viableHangs.Count != 0)
        {
            float min = Mathf.Infinity;
            HangObject best = null;
            foreach (HangObject x in player.viableHangs)
            {
                if (x.Validate())
                {
                    float tmp = (player.transform.position - x.hangPoint.transform.position).magnitude;
                    if (tmp < min)
                    {
                        min = tmp;
                        best = x;
                    }
                }
            }
            if (best != null)
            {
                player.actualHang = best;
                player.actualPhase.SendRequestToCreateState(PlayerStates.Hang);
            }
        }
    }
}
