using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public Attributes attributes;

    [Header("Layers options")]

    public LayerMask boxLayer;

    [HideInInspector]
    public List<ObjectAction> viableAction;
    [HideInInspector]
    public ObjectAction actualAction = null;
    [HideInInspector]
    public bool inAction = false;

    [HideInInspector]
    public List<HangObject> viableHangs;
    [HideInInspector]
    public HangObject actualHang;


    void Start()
    {
        CreatePhase(startPhase);
        grounder = GetComponent<Grounder>();
        viableAction = new List<ObjectAction>();
        viableHangs = new List<HangObject>();   
    }

    public void FixedUpdate()
    {
        actualPhase.FixedUpdate();
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
