using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LevelGenerator : MonoBehaviour
{
    [Header( "Config" )]
    [MinMaxSlider(1, 100)] public Vector2 generationArea;
    [MinMaxSlider( 0.5f, 10 )] public Vector2 height;
    [MinMaxSlider( 2, 10 )] public Vector2 landNumber;
    [MinMaxSlider( 0.1f, 10 )] public Vector2 scaleStepMultiplier;
    [MinMaxSlider( 0.1f, 10 )] public Vector2 baseScale;

    [Space(15)]
    public Material sidesMaterial;
    public Material topMaterial;

    [Space(15)]
    public Transform[] chooseFromList;
    public Vector3[] landPositions;
    public Transform[] landShapes;
    [Space( 15 )]
    public int resolution = 100;
    public float depth = 50;

    private GameObject container;
    private UnityEngine.Random.State previousState;

    private void OnGUI() {
        
        if(GUI.Button( new Rect( 24, 24, 150, 24 ), "Generate All" )){
            this.Generate();
        }

        if(GUI.Button( new Rect( 24, 48, 150, 24 ), "Generate Colors" )){
            PaletteGenerator palette = GetComponent<PaletteGenerator>();
            palette.Generate();
        }

        if(GUI.Button( new Rect( 24, 72, 150, 24 ), "Generate Spawn" )){
            ObjectSpawner spawner = GetComponent<ObjectSpawner>();
            spawner.Spawn();
        }
        
    }

    private void Awake() {
        container = new GameObject("Land");
        container.transform.parent = this.transform;
        container.transform.localPosition = Vector3.zero;
        Generate();
    }

    [Button]
    private void Generate() {
        StopCoroutine( _Generate() );
        StartCoroutine( _Generate() );
    }

    private IEnumerator _Generate() {
        previousState = UnityEngine.Random.state;
        Clear();
        landPositions = FindLandPositions( generationArea, height, landNumber );
        landShapes = FindLandShapes( landPositions.Length, chooseFromList, false );
        GenerateLand( landPositions, landShapes );
        yield return new WaitForEndOfFrame();
        ObjectSpawner spawner = GetComponent<ObjectSpawner>();
        spawner.area = generationArea * 2;
        spawner.Spawn();
    }

    [Button]
    private void LoadPreviousState(){
        UnityEngine.Random.state = previousState;
        Generate();
    }
        

    private void Clear() {
        for (int i = 0; i < container.transform.childCount; i++) {
            GameObject.Destroy( container.transform.GetChild( i ).gameObject );
        }
    }

    private Vector3[] FindLandPositions(Vector2 area, Vector2 heightRange, Vector2 numberRange){
        int numberOf = UnityEngine.Random.Range( (int)numberRange.x, (int)numberRange.y + 1 );
        Vector3[] list = new Vector3[numberOf];
        float lastY = 0;
        for (int i = 0; i < numberOf; i++){

            //First element (base) has xy values fixed
            float x = i == 0 ? transform.position.x : UnityEngine.Random.Range( -area.x / 2, area.x / 2 );
            float z = i == 0 ? transform.position.z : UnityEngine.Random.Range( -area.y / 2, area.y / 2 );
            float y = lastY;

            list[i] = new Vector3( x, y, z );
            lastY += UnityEngine.Random.Range( heightRange.x, heightRange.y );
        }
        return list;
    }

    private Transform[] FindLandShapes(int numberOf, Transform[] shapeList, bool repeat = false){

        Transform[] shapes = new Transform[numberOf];
        for (int i = 0; i < numberOf; i++){
            bool foundShape = false;
            int tries = 0;
            while(!foundShape){
                Transform randomShape = shapeList[UnityEngine.Random.Range( 0, shapeList.Length - 1 )];

                // Avoid repeating shapes
                if (!Array.Exists( shapes, x => x == randomShape )) {
                    shapes[i] = randomShape;
                    foundShape = true;
                }

                //Stop after 10 tries
                if(tries >= 10){
                    shapes[i] = randomShape;
                    foundShape = true;
                }

                tries++;
            }
        }

        return shapes;

    }

    private void GenerateLand(Vector3[] positions, Transform[] shapes){

        float scaleMultiplier = UnityEngine.Random.Range( baseScale.x, baseScale.y );
        Vector3 lastScale = Vector3.one * scaleMultiplier;
        lastScale.y = depth;
        Color lastColor = Color.white;

        for (int i = 0; i < positions.Length; i++){

            Transform land = GameObject.Instantiate( 
                shapes[i],
                new Vector3(positions[i].x, 0, positions[i].z),
                Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-180, 180), 0)),
                container.transform
            );

            land.name = "Land" + i;

            ShapeGenerator generator = land.GetComponent<ShapeGenerator>();
            generator.height = positions[i].y;
            generator.resolution = resolution;
            generator.depth = depth;
            generator.Generate();

            generator.shape.obj.transform.localScale = lastScale;
            lastScale *= UnityEngine.Random.Range(scaleStepMultiplier.x, scaleStepMultiplier.y);
            lastScale.y = depth; // Y scale is always shape depth

            PaletteGenerator palette = GetComponent<PaletteGenerator>();
            palette.Generate();

            generator.shape.sides.GetComponent<MeshRenderer>().material = sidesMaterial;
            generator.shape.top.GetComponent<MeshRenderer>().material = topMaterial;

        }
    }

    public Color DarkenColor(Color color, float ammount){
        float h, s, v;
        Color.RGBToHSV( color, out h, out s, out v );
        v -= ammount;
        return Color.HSVToRGB( h, s, v );
    }

}
