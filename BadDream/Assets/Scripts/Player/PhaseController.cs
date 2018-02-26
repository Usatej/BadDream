using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Attributes
{
    public float moveSpeed = 250;
    public float friction = 0.8f;
    
    [Range(1,10)]
    public float jumpForce = 10;
    [Range(0, 1)]
    public float doubleJumpMultiplier = 0.6f;
    public float pushPower = 150;

}



public class PhaseController : MonoBehaviour
{
    [Header("Inputs")]
    public TouchManager touchManager;
    public VirtualJoystick joystick;

    [HideInInspector]
    public Grounder grounder;

    [Header("Start phases")]
    public Phases startPhase;

    [HideInInspector]
    public Phase actualPhase;

    [Header("Player attributes")]
    public Attributes moveAttributes;

    [HideInInspector]
    public ObjectAction actualAction = null;
    [HideInInspector]
    public bool inAction = false;


    void Start()
    {
        CreatePhase(startPhase);
        grounder = GetComponent<Grounder>();
    }


    void Update()
    {
        actualPhase.Update();
    }

    public Phase CreatePhase(Phases _phase)
    {
        switch(_phase)
        {
            case Phases.Normal:
                return actualPhase = new NormalPhase(this);
            case Phases.Running:
                return actualPhase = null;
            default:
                return actualPhase = null;
        }
    }
}
