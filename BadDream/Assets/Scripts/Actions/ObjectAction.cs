using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectAction {

    public GameObject actionObject;
    public GameObject player;

    public ObjectAction(GameObject obj, GameObject _player)
    {
        actionObject = obj;
        player = _player;
    }

    abstract public bool CanEnter();

    abstract public bool Enter();

    abstract public bool HandleInput();

    abstract public bool Update();
}
