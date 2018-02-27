using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPhase : Phase
{
    private TouchManager touchManager;


    public NormalPhase(PhaseController _player) : base(_player)
    {
        
        touchManager = player.touchManager;
        Enter();
        CreateState(PlayerStates.Move);
    }

    public override void Enter()
    {

    }

    private void HandleInput()
    {
        if((touchManager.AreaTap || Input.GetKeyDown(KeyCode.K)) && !player.inAction && player.viableAction != null)
        {
            CreateState(PlayerStates.Action);
        }
    }

    protected override void EarlyUpdate()
    {
        HandleInput();
    }
}
