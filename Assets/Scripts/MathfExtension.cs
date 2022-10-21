using UnityEngine;

public static class MathfExtension
{
    public static float Pow2(float num)
    {
        return num * num;
    }

    public static int Pow2(int num)
    {
        return num * num;
    }

    public static float Pow3(float num)
    {
        return num * num * num;
    }

    public static int Pow3(int num)
    {
        return num * num * num;
    }

    public static float GetVectorLength(Vector3 firstPoint, Vector3 secondPoint)
    {
        return Mathf.Sqrt(MathfExtension.Pow2(secondPoint.x - firstPoint.x) +
                          MathfExtension.Pow2(secondPoint.y - firstPoint.y) +
                          MathfExtension.Pow2(secondPoint.z - firstPoint.z));
    }
}
