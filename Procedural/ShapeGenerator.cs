using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PathCreation;
using NaughtyAttributes;

public struct Shape {

    public GameObject obj;
    public Transform top;
    public Transform sides;
    public Transform bottom;

    public Shape( GameObject obj, Transform top, Transform sides, Transform bottom ) {
        this.obj = obj;
        this.top = top;
        this.sides = sides;
        this.bottom = bottom;
    }

}

[RequireComponent(typeof(PathCreator))]
public class ShapeGenerator : MonoBehaviour {

    public float depth = 100;
    public float height = 0;
    public bool random = false;
    public float randomization = 3;
    public int resolution = 100;

    [Space(15)]

    public Material bottomMaterial;
    public Material topMaterial;
    public Material sidesMaterial;
    public Shape shape;

    private PolyExtruder extruder;
    private PathCreator pathCreator;
    private Vector2[] vertices;

    [Button]
    public void Generate(){
        pathCreator = GetComponent<PathCreator>();

        pathCreator.bezierPath = RandomizePoints( pathCreator.bezierPath );
        vertices = GetPointsAlongPath( pathCreator.path, resolution );

        shape = GenerateShape( vertices, height, depth );

        shape.bottom.GetComponent<Renderer>().material = bottomMaterial;
        shape.top.GetComponent<Renderer>().material = topMaterial;
        shape.sides.GetComponent<Renderer>().material = sidesMaterial;

        shape.bottom.GetComponent<MeshCollider>().enabled = true;
        shape.top.GetComponent<MeshCollider>().enabled = true;
        shape.sides.GetComponent<MeshCollider>().enabled = true;

        shape.top.gameObject.layer = 3;
    }

    private BezierPath RandomizePoints(BezierPath bezier){
        BezierPath path = bezier;
        for (int i = 0; i < path.NumPoints; i++) {
            Vector3 random = new Vector3(
                Random.Range( path.GetPoint( i ).x - randomization, path.GetPoint( i ).x + randomization ),
                0,
                Random.Range( path.GetPoint( i ).z - randomization, path.GetPoint( i ).z + randomization )
            );
            path.MovePoint( i, random );
        }
        return path;
    }

    private Vector2[] GetPointsAlongPath(VertexPath path, int resolution){
        Vector2[] vertices = new Vector2[resolution];
        float t = 0;
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] = new Vector2( path.GetPointAtDistance( t ).x, path.GetPointAtDistance( t ).z );
            t += path.length / resolution;
        }
        return vertices;
    }   

    private Shape GenerateShape(Vector2[] vertices, float height = 0, float depth = 100, string name = "ShapeObject"){

        GameObject shapeObject = new GameObject();
        shapeObject.transform.parent = this.transform;
        shapeObject.name = name;
        //shapeObject.transform.localPosition = Vector3.zero;

        extruder = shapeObject.AddComponent<PolyExtruder>();
        extruder.createPrism( shapeObject.name, depth, vertices, Color.gray, true );

        Transform bottom = shapeObject.transform.GetChild( 0 );
        bottom.transform.position = Vector3.zero;

        Transform top = shapeObject.transform.GetChild( 1 );
        top.transform.position = Vector3.zero;

        Transform sides = shapeObject.transform.GetChild( 2 );
        sides.transform.position = Vector3.zero;

        Vector3 p = shapeObject.transform.localPosition;
        p.y = shapeObject.transform.localPosition.y - (depth - height);
        shapeObject.transform.localPosition = p;

        return new Shape( shapeObject, top, sides, bottom ); ;
    }



}
