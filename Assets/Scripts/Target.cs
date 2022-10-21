using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : BaseMove
{
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private float predictPositionOffset = 0.05f;
    [SerializeField] private Plane plane;


    private Transform planeTransform;
    private Gun gun;
    private int step = 83;
    private float checkStep = 0.05f;

    public float moveDistance = 0;

    private void Start()
    {
        planeTransform = plane.transform;
        gun = bulletSpawner.GetComponentInParent<Gun>();
        step = movingSpline.bezierParts;
        RecalculatePosition();
    }

    private void Update()
    {
        RecalculatePosition();
    }

    public void RecalculatePosition()
    {
        Vector3 prevPos = planeTransform.position;
        this.transform.position = prevPos;
        moveDistance = MathfExtension.GetVectorLength(prevPos, movingSpline.bezierCurves[plane.curveCounter].curvePos[plane.segmentCounter]);

        FindPredictSegment(prevPos);
    }

    protected void FindPredictSegment(Vector3 prevPos)
    {
        if(IsPredictSegment(prevPos))
        {
            this.transform.position = FindPredictPosition(plane.segmentCounter);
            Debug.Log("prekol");
            return;
        }

        for (int i = plane.segmentCounter; i < step; i++) //out of range if curve ended
        {
            var newPos = movingSpline.bezierCurves[curveCounter].curvePos[i];
           
            moveDistance += MathfExtension.GetVectorLength(newPos, prevPos);

            if (IsPredictSegment(newPos))
            {
                this.transform.position = FindPredictPosition(i);
                break;
            }

            prevPos = newPos;
        }
    }

    protected Vector3 FindPredictPosition(int startSegment)
    {
        Vector3 currentSegmentPosition = movingSpline.bezierCurves[curveCounter].curvePos[startSegment];
        Vector3 prevSegmentPosition = movingSpline.bezierCurves[curveCounter].curvePos[startSegment - 1];

        float segmentLen = MathfExtension.GetVectorLength(currentSegmentPosition, prevSegmentPosition);
        float distanceOfPrevSegment = moveDistance - segmentLen;

        for(float i = 0; i < segmentLen; i += checkStep)
        {
            Vector3 predictPos = prevSegmentPosition + (currentSegmentPosition - prevSegmentPosition).normalized * i;
            gun.LookAtPoint(predictPos);

            float newDistance = distanceOfPrevSegment + i;

            if(IsPredictPosition(predictPos, newDistance))
            {
                return predictPos;
            }
        }

        return movingSpline.bezierCurves[curveCounter].curvePos[startSegment];
    }

    protected bool IsPredictPosition(Vector3 predictPosition, float distance)
    {

        if(Mathf.Abs(GetTimeDif(predictPosition, distance)) < predictPositionOffset)
        {
            return true;
        }
        return false;
    }

    protected bool IsPredictSegment(Vector3 segmentPos)
    {
        if(GetTimeDif(segmentPos, moveDistance) > 0)
        {
            return true;
        }
        return false;
    }    

    protected float GetTimeDif(Vector3 newPos, float distToPlane)
    {
        float finalTime = 0;

        gun.LookAtPoint(newPos);
        float distToSpawner = MathfExtension.GetVectorLength(newPos, bulletSpawner.transform.position);

        float planeSpeed = 2f; // todo: change to scriptableOBJ;
        float bulletSpeed = 5f;

        float planeTime = distToPlane / planeSpeed;
        float bulletTime = distToSpawner / bulletSpeed;

        finalTime = planeTime - bulletTime;


        return finalTime;
    }
}
