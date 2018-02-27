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

    public IKInfo leftArmPull;
    public IKInfo rightArmPull;
}
