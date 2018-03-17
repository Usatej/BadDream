using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIEvents : MonoBehaviour {

    private GameMaster gm;
	void Start () {
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
	}
	
	public void UIShow()
    {
        gm.ActivateLevelUI(true);
    }

    public void UIHide()
    {
        gm.ActivateLevelUI(false);
    }

    public void ActivateMainCam()
    {
        gm.ActivateMainMenu();
    }

    public void LeaveLevelButton()
    {
        UIHide();
        GetComponent<Animator>().Play("MoveFromBed");
    }
}
