using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletRope : MonoBehaviour
{

    [Range( 1, 10 )] public int length = 5;
    [Range( 0, 2 )] public float stickLength = 0.5f;
    [Range( 0, 1 )] public float stretchFactor = 0.05f;

    public bool generated = false;

    [SerializeField] private bool generateOnAwake = false;
    [SerializeField] private bool update = false;

    [HideInInspector] public Point[] points;
    [HideInInspector] public Stick[] sticks;

    private void Awake() {

        if(generateOnAwake) {
            Generate();
        }

    }

    private void Update() {
        
        if(update) {
            Simulate();
        }

    }

    public void Generate() {

        points = new Point[length];
        sticks = new Stick[length - 1];

        // Create verlet points
        for (int i = 0; i < length; i++) {

            GameObject p = new GameObject( $"Point {i}" );
            p.transform.position += p.transform.right * stickLength * i;
            p.AddComponent<Point>();
            Point point = p.GetComponent<Point>();

            Debug.Log( length );
            Debug.Log( points.Length );

            points[i] = point;

        }

        // Create verlet sticks
        for (int i = 0; i < length - 1; i++) {
            sticks[i] = new Stick( points[i], points[i + 1], stretchFactor );
        }

        generated = true;

    }

    public void Simulate() {

        VerletSimulation.Simulate( ref points, ref sticks, 100 );

    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.red;

        for (int i = 0; i < sticks.Length; i++) {
            Gizmos.DrawLine( sticks[i].pointA.transform.position, sticks[i].pointB.transform.position );
        }

        for (int i = 0; i < points.Length; i++) {
            Gizmos.DrawWireSphere( points[i].transform.position, 0.1f );
        }

    }

}
