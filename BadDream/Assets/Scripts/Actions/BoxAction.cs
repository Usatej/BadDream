using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAction : ObjectAction
{
    private GameObject bigBox;
    private DistanceJoint2D dJoint;
    private IKController iks;

    private float distanceFromBox = 0.75f; 

    private bool firstLoopWait = true;

    private SpriteRenderer handArea;

    private GameObject p1, p2, cen;

    private VirtualJoystick joystick;
    private Rigidbody2D rb;
    private Rigidbody2D bigBoxRb;
    private DistanceJoint2D dj;
    private Attributes attrs;

    private Animator anim;

    private float pushDistance = 0.2f;
    private float pullDistance = 0.5f;

    private Grounder grounder;

    public BoxAction(GameObject obj, GameObject pl): base(obj,pl)
    {
    }

    public override bool CanEnter()
    {
        float xBox= actionObject.transform.position.x;
        float xPl = player.transform.position.x;
        if (Mathf.Sign(player.transform.localScale.x) > 0) return (xBox > xPl);
        else return (xPl > xBox);
    }

    public override bool Enter()
    {
        firstLoopWait = true;
        bigBox = actionObject.transform.parent.gameObject;
        iks = player.GetComponent<IKController>();
        handArea = actionObject.GetComponent<SpriteRenderer>();
        PhaseController pc = player.GetComponent<PhaseController>();
        joystick = pc.joystick;
        attrs = pc.attributes;
        grounder = pc.grounder;
        rb = player.GetComponent<Rigidbody2D>();
        bigBoxRb = bigBox.GetComponent<Rigidbody2D>();
        anim = player.GetComponent<Animator>();
        distanceFromBox = (player.transform.localScale.x > 0) ? -distanceFromBox : distanceFromBox;
        player.transform.position = new Vector3(actionObject.transform.position.x + distanceFromBox, player.transform.position.y, 0);
        iks.leftArm.ik.transform.gameObject.SetActive(true);
        iks.rightArm.ik.transform.gameObject.SetActive(true);
        anim.SetBool("pulling", true);
        CreateJoint();
        return true;
    }

    public override bool HandleInput()
    {
        if(joystick.IsThereInput() && grounder.IsGrounded())
        {
            float y = joystick.Input.y;
            float x = joystick.Input.x / Mathf.Abs(joystick.Input.x);
            //y = (y < 0) ? y*attrs.pushSpeed : -5f + rb.velocity.y;
            rb.velocity = new Vector2(x * attrs.pushSpeed, rb.velocity.y);
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            StopHolding();
            return false;
        }
        return true;
    }

    public override bool Update()
    {
        if (firstLoopWait)
        {
            firstLoopWait = false;
            return true;
        }

        dj.enabled = false;

        if (Mathf.Abs(bigBoxRb.velocity.x) > 1.5)
        {
            bigBoxRb.velocity = new Vector2(1.5f * Mathf.Sign(bigBoxRb.velocity.x),bigBoxRb.velocity.y);
        }
        if(!joystick.IsThereInput()) rb.velocity = new Vector2(rb.velocity.x * attrs.friction , rb.velocity.y);
        anim.SetFloat("speed", (-player.transform.localScale.x) * joystick.Input.x);
//        if(joystick.Input.)


        Bounds bd = handArea.bounds;
        Vector2 x = CalculatePoints(bd.center.x, bd.max.y, bd.center.x, bd.center.y, actionObject.transform.eulerAngles.z);
        Vector2 y = CalculatePoints(bd.center.x, bd.min.y, bd.center.x, bd.center.y, actionObject.transform.eulerAngles.z);
        Vector2 tmp = new Vector2(iks.rightArm.centerPoint.transform.position.x, iks.rightArm.centerPoint.transform.position.y);
        List<IntersectPoint> points = CalculateIntersectPoints(x, y, tmp, iks.rightArm.maxDistance);
        dj.connectedAnchor = player.transform.InverseTransformPoint(iks.rightArm.centerPoint.transform.position);
        HandsIKPosition(points, x, y);



        dj.enabled = true;
        return true;
    }

    private Vector2 CalculatePoints(float x, float y, float cx, float cy, float rotation)
    {
        float theta = Mathf.Deg2Rad * rotation;

        float tempX = x - cx;
        float tempY = y - cy;

        // now apply rotation
        float rotatedX = tempX * Mathf.Cos(theta) - tempY * Mathf.Sin(theta);
        float rotatedY = tempX * Mathf.Sin(theta) + tempY * Mathf.Cos(theta);

        // translate back
        x = rotatedX + cx;
        y = rotatedY + cy;

        return new Vector2(x, y);
    }

    private List<IntersectPoint> CalculateIntersectPoints(Vector2 point1, Vector2 point2, Vector2 center, float radius)
    {
        List<IntersectPoint> intersectPoints = new List<IntersectPoint>();
        Vector2 u = point2 - point1;

        float a = u.x * u.x + u.y * u.y;
        float b = 2 * (u.x * (point1.x - center.x) + u.y * (point1.y - center.y));
        float c = (point1.x - center.x) * (point1.x - center.x) + (point1.y - center.y) * (point1.y - center.y) - radius * radius;

        //Determinant
        float d = b * b - 4 * a * c;

        if (d < 0) {
            return intersectPoints;
        }
        if (d == 0)
        {
            float k = -b / (2 * a);
            IntersectPoint inPoint;
            inPoint.point = point1 + k * u;
            inPoint.k = k;
            intersectPoints.Add(inPoint);
        } else if(d > 0)
        {
            float k = (-b + Mathf.Sqrt(d)) / (2 * a);
            IntersectPoint inPoint;
            inPoint.point = point1 + k * u;
            inPoint.k = k;
            intersectPoints.Add(inPoint);
            k = (-b - Mathf.Sqrt(d)) / (2 * a);
            inPoint.point = point1 + k * u;
            inPoint.k = k;
            intersectPoints.Add(inPoint);
        }
        return intersectPoints;
    }

    private bool HandsIKPosition(List<IntersectPoint> inPoints, Vector2 topP, Vector2 botP)
    {
        Vector3 anchors = Vector3.zero;
        if (inPoints.Count == 0)
        {
            return false;
        }
        if (inPoints.Count == 1 && PointIsBetweenPoints(botP, topP, inPoints[0].point))
        {
            anchors = iks.rightArm.ik.transform.position = iks.leftArm.ik.transform.position = inPoints[0].point;
            return true;
        }
        if (inPoints.Count == 2)
        {
            bool first = PointIsBetweenPoints(botP, topP, inPoints[0].point);
            bool second = PointIsBetweenPoints(botP, topP, inPoints[1].point);
            if (first && second)
            {
                Vector2 handsMid = (inPoints[0].point + inPoints[1].point) / 2;
                Vector2 holderMid = (botP + topP) / 2;
                Vector2 handsPos = (handsMid + holderMid)/ 2;
                if (PointIsBetweenPoints(inPoints[0].point, inPoints[1].point, handsPos))
                {
                    anchors = iks.rightArm.ik.transform.position = iks.leftArm.ik.transform.position = handsPos;
                }
                else
                {
                    anchors = iks.rightArm.ik.transform.position = iks.leftArm.ik.transform.position = GetClosestPoint(inPoints[0].point, inPoints[1].point, handsPos);
                }
            }
            else if (first && !second)
            {
                Vector2 n = GetClosestPoint(botP, topP, inPoints[1].point);
                Vector2 holderMid = (inPoints[0].point + n) / 2;
                anchors = iks.rightArm.ik.transform.position = iks.leftArm.ik.transform.position = holderMid;

            }
            else if (!first && second)
            {
                Vector2 n = GetClosestPoint(botP, topP, inPoints[0].point);
                Vector2 holderMid = (inPoints[1].point + n) / 2;
                anchors = iks.rightArm.ik.transform.position = iks.leftArm.ik.transform.position = holderMid;
            } else if(PointIsBetweenPoints(inPoints[0].point,inPoints[1].point,topP) && PointIsBetweenPoints(inPoints[0].point, inPoints[1].point, botP)) {
                Vector2 handsMid = (inPoints[0].point + inPoints[1].point) / 2;
                Vector2 holderMid = (botP + topP) / 2;
                Vector2 handsPos = (handsMid + holderMid) / 2;
                if (PointIsBetweenPoints(topP, botP, handsPos))
                {
                    anchors = iks.rightArm.ik.transform.position = iks.leftArm.ik.transform.position = handsPos;
                }
                else
                {
                    anchors = iks.rightArm.ik.transform.position = iks.leftArm.ik.transform.position = GetClosestPoint(topP,botP, handsPos);
                }
            }
            else {
                return false;
            }
        }
        anchors = dj.transform.InverseTransformPoint(anchors);
        dj.anchor = new Vector2(anchors.x,anchors.y);
        return true;
    }

    private bool PointIsBetweenPoints(Vector2 a, Vector2 b, Vector2 p)
    {
        float distA = (p - a).magnitude;
        float distB = (p - b).magnitude;
        float dist = (b - a).magnitude;
        return (Mathf.Abs(dist - distA - distB) < 0.01);
    }

    private Vector2 GetClosestPoint(Vector2 a, Vector2 b, Vector2 p)
    {
        float distA = (p.x - a.x) * (p.x - a.x) + (p.y - a.y) * (p.y - a.y);
        float distB = (p.x - b.x) * (p.x - b.x) + (p.y - b.y) * (p.y - b.y);
        if (distA <= distB) return a;
        else return b;
    }

    private void CreateJoint()
    {
        dj = bigBox.AddComponent<DistanceJoint2D>();
        dj.enabled = false;
        dj.maxDistanceOnly = true;
        dj.enableCollision = true;
        dj.autoConfigureDistance = false;
        dj.connectedBody = rb;
        dj.distance = 0.8f;
    }

    private void StopHolding()
    {
        player.GetComponent<Animator>().SetBool("pulling", false);
        iks.leftArm.ik.gameObject.SetActive(false);
        iks.rightArm.ik.gameObject.SetActive(false);
        GameObject.Destroy(dj);
    }
}
