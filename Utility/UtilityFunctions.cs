using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Utility
{

    //scales number with range (min...max) to (0...1) and returns float value
    public static float ScaleFloat01(float n, float min, float max){
        float r = (n - min) / (max - min);
        return r;
    }

}
