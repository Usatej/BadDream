using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectState
{

    protected PhaseController player;

    public ObjectState(PhaseController _player)
    {
        player = _player;
    }

    public virtual void Enter()
    {

    }

    public virtual ObjectState EarlyUpdate()
    {
        return new ObjectState(player);
    }

    public virtual ObjectState HandleInput()
    {
        return new ObjectState(player);
    }

    public virtual ObjectState Update()
    {
        return new ObjectState(player);
    }
}
