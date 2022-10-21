using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor;
using System.Collections.Generic;

[Serializable]
public class BezierPoint
{
    public Vector3 startPoint;
    public Vector3 key1Point;
    public Vector3 key2Point;
    public Vector3 endPoint;

    //[HideInInspector]
    public List<Vector3> curvePos;

    public enum PointsName
    {
        startPoint,
        key1Point, 
        key2Point,
        endPoint,
        none
    }

    public BezierPoint()
    {
        startPoint = Vector3.zero;
        key1Point = Vector3.zero;
        key2Point = Vector3.zero;
        endPoint = Vector3.zero;

        curvePos = new List<Vector3>();
    }

    public BezierPoint(Vector3 startPoint, Vector3 key1Point, Vector3 key2Point, Vector3 endPoint, float t)
    {
        this.startPoint = startPoint;
        this.key1Point = key1Point;
        this.key2Point = key2Point;
        this.endPoint = endPoint;

        curvePos = new List<Vector3>();
        ReCalculatePoints((int)t);
    }

    public void ReCalculatePoints(int step)
    {
        step--;
        curvePos.Clear();

        Vector3 prevPos = GetBezierPosition(startPoint, key1Point, key2Point, endPoint, (float)0 / step);

        curvePos.Add(prevPos);

        for (int i = 1; i <= step; i++)
        {
            var newPos = GetBezierPosition(startPoint, key1Point, key2Point, endPoint, (float)i / step);

            prevPos = newPos;

            curvePos.Add(prevPos);
        }
    }

    public Vector3 GetBezierPosition(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }

    public Vector3 GetBezierPosition(float t)
    {
       return GetBezierPosition(startPoint, key1Point, key2Point, endPoint, t);
    }

    public void DrawGizmos(int step)
    {
        Vector3 prevPos = GetBezierPosition(startPoint, key1Point, key2Point, endPoint, (float)0 / step);

        for(int i = 1; i <= step; i++)
        {
            var newPos = GetBezierPosition(startPoint, key1Point, key2Point, endPoint, (float)i / step);

            Handles.DrawLine(prevPos, newPos);
            prevPos = newPos;
        }
    }
}
