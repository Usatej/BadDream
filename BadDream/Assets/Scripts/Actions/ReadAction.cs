using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadAction: ObjectAction {

    private Image plane;
    private TouchManager tm;

	public ReadAction(GameObject obj, GameObject pl) : base(obj, pl)
    {
    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool Enter()
    {
        tm = player.GetComponent<PhaseController>().touchManager;
        plane = actionObject.GetComponent<ReadController>().plane;
        plane.gameObject.SetActive(true);
        tm.ResetManager();
        return true;
    }

    public override bool HandleInput()
    {
        if(tm.AreaTap || Input.GetKeyDown(KeyCode.L))
        {
            plane.gameObject.SetActive(false);
            return false;
        }
        return true;
    }

    public override bool Update()
    {
        return true;
    }
}
