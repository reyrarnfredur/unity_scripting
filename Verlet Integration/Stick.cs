using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stick {

    public float MinimumLength { get => fixedLength - stretchTolerance; }
    public float MaximumLength { get => fixedLength + stretchTolerance; }

    public Point pointA;
    public Point pointB;
    public float fixedLength;
    public float stretchTolerance;
    [SerializeField] public float variableLength;

    [SerializeField] private float minLength;
    [SerializeField] private float maxLength;

    public Stick( Point pointA, Point pointB, float stretchTolerance = 0.5f ) {
        this.pointA = pointA;
        this.pointB = pointB;
        this.fixedLength = Vector3.Distance(pointA.Position, pointB.Position);
        this.stretchTolerance = stretchTolerance;
        this.minLength = fixedLength - stretchTolerance;
        this.maxLength = fixedLength + stretchTolerance;
    }

    public void RecalculateFixedLength() {
        fixedLength = Vector3.Distance( pointA.Position, pointB.Position );
    }

    private void OnDestroy() {
        Debug.Log( $"Stick has been destroyed!" );
    }

}
