using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HangTypes
{
    Ledge,
    Rope,
    Pipe,
}

public abstract class HangObject {

    public GameObject hangPoint;
    protected PhaseController player;

	public HangObject(GameObject _hangPoint, GameObject _player)
    {
        hangPoint = _hangPoint;
        player = _player.GetComponent<PhaseController>();
    }

    abstract public bool Validate();

    abstract public void Enter();

    abstract public void HandleInput();

    abstract public void Update();
}
