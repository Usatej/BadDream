using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangController : MonoBehaviour {

    public HangTypes typeOfHang;
    
    private bool isIn = false;

    private HangObject actHang;

    private PhaseController player;

    private int cnt = 0;

	void Start () {
        player = GameObject.Find("Player").GetComponent<PhaseController>();
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(cnt == 0)
        {
            Debug.Log("Add");
            player.viableHangs.Add(CreateHang(coll.gameObject.transform.root));
            isIn = true;
        }
        cnt++;
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        cnt--;
        if (cnt == 0)
        {
            Debug.Log("Remove");
            player.viableHangs.Remove(actHang);
            isIn = false;
        }
    }

    private HangObject CreateHang(Transform player)
    {
        switch(typeOfHang)
        {
            case HangTypes.Ledge:
                return actHang = new Ledge(gameObject, player.gameObject);
            default:
                return actHang = new Ledge(gameObject, player.gameObject);
        }
    }
}
