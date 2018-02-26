using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAction : ObjectAction
{

    public struct IntersectPoint {
        public Vector2 point;
        public float k;
    }

    private GameObject bigBox;
    private DistanceJoint2D dJoint;
    private IKController iks;

    private float minHandPos,maxHandPos;
    private float bottomHolder, topHolder;

    private float distanceFromBox = 0.75f;
        

    private SpriteRenderer handArea;

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
        bigBox = actionObject.transform.parent.gameObject;
        iks = player.GetComponent<IKController>();
        handArea = actionObject.GetComponent<SpriteRenderer>();
        player.GetComponent<Animator>().Play("Push");
        player.transform.position = new Vector3(actionObject.transform.position.x - distanceFromBox, player.transform.position.y, 0);
        iks.leftArmPull.ik.transform.gameObject.SetActive(true);
        iks.rightArmPull.ik.transform.gameObject.SetActive(true);
        return true;
    }

    public override bool HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Bounds bd = handArea.bounds;
            Vector2 x = CalculatePoints(bd.center.x, bd.max.y, bd.center.x, bd.center.y, actionObject.transform.eulerAngles.z);
            Vector2 y = CalculatePoints(bd.center.x, bd.min.y, bd.center.x, bd.center.y, actionObject.transform.eulerAngles.z);
            Vector2 tmp = new Vector2(iks.rightArmPull.centerPoint.transform.position.x, iks.rightArmPull.centerPoint.transform.position.y);
            List<IntersectPoint> points = CalculateIntersectPoints(x, y, tmp, iks.rightArmPull.maxDistance);
            HandsIKPosition(points, x, y);
        }
        return true;
    }

    public override bool Update()
    {
        
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

        if (d < 0) return intersectPoints;
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
        if (inPoints.Count == 0) return false;
        if (inPoints.Count == 1 && PointIsBetweenPoints(botP, topP, inPoints[0].point))
        {
            iks.rightArmPull.ik.transform.position = iks.leftArmPull.ik.transform.position = inPoints[0].point - new Vector2(iks.rightArmPull.offSet, 0);
            return true;
        }
        if (inPoints.Count == 2)
        {
            if(PointIsBetweenPoints(botP, topP, inPoints[0].point) && PointIsBetweenPoints(botP, topP, inPoints[1].point))
            {
                Vector2 holderMid = (botP + topP) / 2;
                Vector2 handsMid = (inPoints[0].point + inPoints[1].point) / 2;
                iks.rightArmPull.ik.transform.position = iks.leftArmPull.ik.transform.position = ((holderMid + handsMid) / 2) - new Vector2(iks.rightArmPull.offSet,0);
            }
        }
        return true;
    }

    private bool PointIsBetweenPoints(Vector2 point, Vector2 a, Vector2 b)
    {
        float dist1 = (point - a).magnitude;
        float dist2 = (point - b).magnitude;
        float dist = (a - b).magnitude;
        return ((dist - dist1 + dist2) < 0.0000001f);
    }
}
