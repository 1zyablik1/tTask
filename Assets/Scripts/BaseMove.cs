using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected BezierSpline movingSpline;

    public int curveCounter = 0;
    public int segmentCounter = 0;

    protected float distanceOffset = 0.01f;


    void Start()
    {
        
    }

    protected void MoveOnSpline()
    {
        if (curveCounter < movingSpline.bezierCurves.Count)
        {
            if (segmentCounter < movingSpline.bezierCurves[curveCounter].curvePos.Count)
            {
                transform.position = Vector3.MoveTowards(transform.position, movingSpline.bezierCurves[curveCounter].curvePos[segmentCounter], Time.deltaTime * speed);
                if (Vector3.Distance(transform.position, movingSpline.bezierCurves[curveCounter].curvePos[segmentCounter]) < distanceOffset)
                {
                    segmentCounter++;
                    if (segmentCounter == movingSpline.bezierCurves[curveCounter].curvePos.Count)
                    {
                        curveCounter++;
                        segmentCounter = 0;
                    }
                }
            }

        }
    }

    public void SetStartSegment(int segment)
    {
        segmentCounter = segment;
    }

    public void SetStartCurve(int curve)
    {
        curveCounter = curve;
    }

    public void SetStartCurveSegment(int curve, int segment)
    {
        SetStartCurve(curve);
        SetStartSegment(segment);
    }
}
