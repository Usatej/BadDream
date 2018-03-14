using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public void QuitApp()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}
