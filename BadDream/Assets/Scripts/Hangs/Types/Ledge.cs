using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : HangObject
{

    public LayerMask layer;

    private IKController iks;

    private Animator anim;

    private Rigidbody2D rbHang;
    private Rigidbody2D rbP;
    private DistanceJoint2D dj;

    public Ledge(GameObject obj, GameObject pl): base(obj,pl)
    {
        layer = ~((1 << 9) | (1 << 12));
    }

    public override void Enter()
    {
        anim = player.GetComponent<Animator>();
        rbP = player.GetComponent<Rigidbody2D>();
        anim.SetBool("hanged", true);

        iks = player.GetComponent<IKController>();
        iks.leftArm.ik.gameObject.SetActive(true);
        iks.rightArm.ik.gameObject.SetActive(true);


        rbHang = player.actualHang.hangPoint.AddComponent<Rigidbody2D>();
        rbHang.isKinematic = true;
        dj = player.actualHang.hangPoint.AddComponent<DistanceJoint2D>();
        dj.connectedBody = rbP;
        dj.autoConfigureDistance = false;
        dj.maxDistanceOnly = true;
        dj.autoConfigureConnectedAnchor = false;
        dj.distance = 0.9f;
    }

    public override void HandleInput()
    {
        if (player.touchManager.AreaSwipeDown || Input.GetKeyDown(KeyCode.S))
        {
            LeaveLedge();
            player.actualPhase.SendRequestToCreateState(PlayerStates.Air);
        }
        if (player.touchManager.AreaSwipeUp || Input.GetKeyDown(KeyCode.W))
        {
            LeaveLedge();
            rbP.isKinematic = true;
            anim.SetBool("climb",true);
            anim.Play("ClimbUp_LongHang");
        }
    }

    public override void Update()
    {
        iks.leftArm.ik.transform.position = iks.rightArm.ik.transform.position = hangPoint.transform.position;
    }

    public override bool Validate()
    {

        int look = (int)Mathf.Sign(hangPoint.transform.position.x - player.transform.position.x);
        int plook = (int)Mathf.Sign(player.transform.localScale.x);
        if (look == plook)
        {
            Vector2 size = new Vector2(0.8f, 0.5f);
            Vector2 rayStart = new Vector2(hangPoint.transform.position.x - look * (0.1f + size.x / 2), hangPoint.transform.position.y);
            DrawBoxCast(rayStart, size, Color.red);
            RaycastHit2D hit = Physics2D.BoxCast(rayStart, size, 0, Vector2.down, 2.5f, layer.value);
            if (hit) return false;
            rayStart = new Vector2(hangPoint.transform.position.x + look * (0.1f + size.x / 2), hangPoint.transform.position.y + (0.1f + size.y / 2));
            DrawBoxCast(rayStart, size, Color.red);
            hit = Physics2D.BoxCast(rayStart, size, 0, Vector2.up, 2.3f, layer.value);
            if (hit) return false;
        }
        else return false;
        return true;
    }

    private void LeaveLedge()
    {
        GameObject.Destroy(dj);
        GameObject.Destroy(rbHang);
      //  iks.leftArm.ik.gameObject.SetActive(false);
     //   iks.rightArm.ik.gameObject.SetActive(false);
        //anim.SetBool("hanged", false);
    }

    private void DrawBoxCast(Vector2 origin, Vector2 size, Color color)
    {
        Vector2 downLeft = new Vector2(origin.x - size.x / 2, origin.y - size.y / 2);
        Vector2 downRight = new Vector2(origin.x + size.x / 2, origin.y - size.y / 2);
        Vector2 upLeft = new Vector2(origin.x - size.x / 2, origin.y + size.y / 2);
        Vector2 upRight = new Vector2(origin.x + size.x / 2, origin.y + size.y / 2);
        Debug.DrawLine(downLeft,downRight, color);
        Debug.DrawLine(downRight, upRight, color);
        Debug.DrawLine(upRight, upLeft, color);
        Debug.DrawLine(upLeft, downLeft, color);
    }
}
