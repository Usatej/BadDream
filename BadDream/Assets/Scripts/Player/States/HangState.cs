using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangState : ObjectState
{
    

    public HangState(PhaseController pl): base(pl)
    {
        player.viableHangs.Clear();
        Enter();
    }

    public override void EarlyUpdate()
    {

    }

    public override void Enter()
    {
        player.actualHang.Enter();
    }

    public override void FixedUpdate()
    {
        player.actualHang.FixedUpdate();
    }

    public override void HandleInput()
    {
        player.actualHang.HandleInput();
    }

    public override void Update()
    {
        player.actualHang.Update();
    }
}
