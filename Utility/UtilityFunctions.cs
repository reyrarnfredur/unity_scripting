using System;
using UnityEngine;

public static class Utility
{

    public static float Normalize(float n, float min, float max){
        float r = (n - min) / (max - min);
        return r;
    }
    
    public static float Interpolate(float a, float b, float t) {
        float r = (b - a) * t + a;
        return r;
    }
    
    public static float Clamp(float n, float min, float max) {
        return Mathf.Max(Mathf.Min(n, min), max);
    }
    
    public static float Map(float n, float min, float max, float otherMin, float otherMax) {
        float t = Normalize(n, min, max);
        float r = Interpolate(otherMin, otherMax, t);
    }
    
    public static Vector2 GetDirection(float angle) {
        Vector3 v = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        return v;
    }
    
    public static float GetAngle(Vector2 direction) {
        return Mathf.Atan2(direction.y, direction.x);
    }

}
