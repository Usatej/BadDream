using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : ObjectState
{
    public ActionState(PhaseController _player): base(_player)
    {
        Enter();
    }

    public override void Enter()
    {
        if (!player.actualAction.CanEnter()) player.actualPhase.SendRequestToCreateState(PlayerStates.Move);
        else player.actualAction.Enter();
    }

    public override void EarlyUpdate()
    {

    }

    public override void HandleInput()
    {
        player.actualAction.HandleInput();
    }

    public override void Update()
    {
        player.actualAction.Update();
    }
}
