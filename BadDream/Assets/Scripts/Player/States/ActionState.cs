using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : ObjectState
{

    private bool left = false;

    public ActionState(PhaseController _player) : base(_player)
    {
        float min = Mathf.Infinity;
        ObjectAction best = player.viableAction[0];
        foreach(ObjectAction x in player.viableAction)
        {
            Vector2 p = player.transform.position;
            Vector2 o = x.actionObject.transform.position;
            float tmp = (p.x - o.x)* (p.x - o.x) + (p.y - o.y) * (p.y - o.y);
            if (tmp < min) {
                min = tmp;
                best = x;
            }
        }
        player.actualAction = best;
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
