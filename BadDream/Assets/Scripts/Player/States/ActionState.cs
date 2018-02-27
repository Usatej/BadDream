using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : ObjectState
{

    private bool left = false;

    public ActionState(PhaseController _player) : base(_player)
    {
        player.actualAction = player.viableAction;
        Enter();
    }

    public override void Enter()
    {
        if (!player.actualAction.CanEnter())
        {
            Leave();
        }
        else
        {
            player.actualAction.Enter();
        }
    }

    public override void EarlyUpdate()
    {

    }

    public override void HandleInput()
    {
        if (!left && !player.actualAction.HandleInput()) Leave();
    }

    public override void Update()
    {
        if (!left && !player.actualAction.Update()) Leave();
    }

    private void Leave()
    {
        player.actualPhase.SendRequestToCreateState(PlayerStates.Move);
        left = true;
        player.actualAction = null;
    }
}
