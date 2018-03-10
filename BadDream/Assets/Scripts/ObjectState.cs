using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ObjectState
{

    protected PhaseController player;

    public ObjectState(PhaseController _player)
    {
        player = _player;
    }

    public abstract void Enter();

    public abstract void EarlyUpdate();

    public abstract void HandleInput();

    public abstract void Update();

    public abstract void FixedUpdate();
}
