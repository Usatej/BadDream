using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class IKController : MonoBehaviour {

    [System.Serializable]
    public class IKInfo
    {
        public Transform ik;
        public Transform centerPoint;
        public float maxDistance;
    }

    public IKInfo leftArm;
    public IKInfo rightArm;
    public IKInfo leftLeg;
    public IKInfo rightLeg;

    public void DeactivateArmIKs()
    {
        leftArm.ik.gameObject.SetActive(false);
        rightArm.ik.gameObject.SetActive(false);
    }
}
