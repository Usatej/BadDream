using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPhase : Phase
{
    public NormalPhase(PhaseController _player) : base(_player)
    {
        Enter();
        CreateState(PlayerStates.Move);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }
}
