using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangAction : ObjectAction
{
    public HangAction(GameObject obj, GameObject pl) : base(obj, pl)
    {

    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool Enter()
    {
        return true;
    }

    public override bool HandleInput()
    {
        return true;
    }

    public override bool Update()
    {
        return true;
    }
}
