using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phases
{
    Normal,
    Running
}


public class Phase
{
    private ObjectState actualState;
    protected PhaseController player;

    public Phase(PhaseController _player)
    {
        player = _player;
    }

    public virtual void Enter()
    {

    }


    public virtual void Update()
    {
        actualState.EarlyUpdate();
        actualState.HandleInput();
        actualState.Update();
    }

    protected void CreateState(PlayerStates _state)
    {
        switch(_state)
        {
            case PlayerStates.Move:
                actualState = new MoveState(player);
                break;
            default:
                actualState = new MoveState(player);
                break;
        }
    }
}
