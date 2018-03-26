using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeActivation : MonoBehaviour {

    public BoxCollider2D coll;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i<transform.childCount;i++)
        {
            Transform x = transform.GetChild(i);
            Vector3 pos = x.position;
            pos = new Vector3(pos.x, pos.y, coll.transform.position.z);
            if (coll.bounds.Contains(pos))
            {
                x.gameObject.SetActive(true);
            } else x.gameObject.SetActive(false);
        }
	}
}
