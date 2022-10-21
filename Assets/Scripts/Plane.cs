using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : BaseMove
{
    public List<BezierSpline> spline;

    private void Awake()
    {
        this.transform.position = spline[0].GetStartPoint();
        movingSpline = spline[0]; //random spline
    }

    private void Update()
    {
        MoveOnSpline();
    }
}
