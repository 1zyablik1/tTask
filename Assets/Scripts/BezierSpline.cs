using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static BezierPoint;

public class BezierSpline : MonoBehaviour
{
    public List<BezierPoint> bezierCurves;
    [Range(2, 300)] public int bezierParts = 10;

    public Vector3 GetStartPoint()
    {
        return bezierCurves[0].startPoint;
    }

    public void AddCurve()
    {
        Vector3 startPos = Vector3.zero;
        if (bezierCurves.Count != 0)
            startPos = bezierCurves[bezierCurves.Count - 1].endPoint;

        BezierPoint newCurve = new BezierPoint(startPos, Vector3.up, Vector3.up, Vector3.up, bezierParts);
        bezierCurves.Add(newCurve);
    }

    public void ReCalculateSpline()
    {
        foreach(var curve in bezierCurves)
        {
            curve.ReCalculatePoints(bezierParts);
        }
    }
}

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineEditor : Editor
{
    private const float handleSize = 0.3f;
    private const float pickSize = 0.1f;

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private PointsName selectedPoint = PointsName.none;
    private int selectedCurveId = -1;

    private void OnSceneGUI()
    {
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;


        for (int i = 0; i < spline.bezierCurves.Count; i++)
        {  
            Vector3 p0 = ShowPoint(i, PointsName.startPoint);
            Vector3 p1 = ShowPoint(i, PointsName.key1Point);
            Vector3 p2 = ShowPoint(i, PointsName.key2Point);
            Vector3 p3 = ShowPoint(i, PointsName.endPoint);

            spline.bezierCurves[i].DrawGizmos(spline.bezierParts);
        }
    }

    private Vector3 ShowPoint(int curvePointId, PointsName pointName)
    {
        Vector3 point = Vector3.zero;
        switch (pointName)
        {
            case PointsName.startPoint:
                point = ShowElement(handleTransform.TransformPoint(spline.bezierCurves[curvePointId].startPoint), curvePointId, pointName);
                break;
            case PointsName.key1Point:
                point = ShowElement(handleTransform.TransformPoint(spline.bezierCurves[curvePointId].key1Point), curvePointId, pointName);
                break;
            case PointsName.key2Point:
                point = ShowElement(handleTransform.TransformPoint(spline.bezierCurves[curvePointId].key2Point), curvePointId, pointName);
                break;
            case PointsName.endPoint:
                point = ShowElement(handleTransform.TransformPoint(spline.bezierCurves[curvePointId].endPoint), curvePointId, pointName);
                break;
        }

        return point;   
    }

    private Vector3 ShowElement(Vector3 point, int curveId, PointsName pointName)
    {
        if (Handles.Button(point, handleRotation, handleSize, pickSize, Handles.DotHandleCap))
        {
            selectedPoint = pointName;
            selectedCurveId = curveId;
        }

        if (selectedPoint == pointName && selectedCurveId == curveId)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);

                switch (pointName)
                {
                    case PointsName.startPoint:
                        spline.bezierCurves[curveId].startPoint = handleTransform.InverseTransformDirection(point);
                        break;
                    case PointsName.key1Point:
                        spline.bezierCurves[curveId].key1Point = handleTransform.InverseTransformDirection(point);
                        break;
                    case PointsName.key2Point:
                        spline.bezierCurves[curveId].key2Point = handleTransform.InverseTransformDirection(point);
                        break;
                    case PointsName.endPoint:
                        spline.bezierCurves[curveId].endPoint = handleTransform.InverseTransformDirection(point);
                        break;
                }
            }
        }
        return point;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        spline = target as BezierSpline;

        if(GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
        if(GUILayout.Button("Recalculate Spline"))
        {
            Undo.RecordObject(spline, "Recalculate spline");
            spline.ReCalculateSpline();
            EditorUtility.SetDirty(spline);
        }
    }
}
