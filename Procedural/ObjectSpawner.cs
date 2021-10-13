using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ObjectSpawner : MonoBehaviour
{

    public LayerMask terrainLayer;
    public LayerMask spawnPropLayer;
    public int spawnLayerID = 6;

    public float spacing = 2.5f;
    public int numberOf = 25;

    [HideInInspector] public Vector2 area;
    [HideInInspector] public GameObject container;

    public Transform[] details;

    private Vector3 origin = Vector3.zero;

    private void Awake() {
        container = new GameObject( "Objects" );
        container.transform.parent = this.transform;
        container.transform.localPosition = Vector3.zero;
    }

    [Button]
    public void Spawn(){
        Clear();
        for (int i = 0; i < numberOf; i++) {
            SpawnDetail();
        }
    }

    private void Clear() {
        for (int i = 0; i < container.transform.childCount; i++) {
            GameObject.Destroy( container.transform.GetChild( i ).gameObject );
        }
    }

    private void SpawnDetail(){
        bool found = false;
        int tries = 0;

        while(!found){

            tries++;

            origin = FindRandomPoint();
            Debug.DrawRay( origin, Vector2.down * 100, Color.red, 1 );
            RaycastHit hit;
            bool raycheck = Physics.Raycast( origin, Vector3.down, out hit, 500, terrainLayer );

            if (raycheck) {
                if (!CheckOverlap( hit.point, spacing )) {
                    Transform t = Transform.Instantiate( details[Random.Range(0, details.Length-1)], hit.point, Quaternion.identity );
                    t.gameObject.layer = spawnLayerID;
                    t.parent = container.transform;
                    found = true;
                }
            }

            if(tries >= 10){
                found = true;
            }
            
        }
        

    }

    private Vector3 FindRandomPoint(){
        Vector2 randomPoint = new Vector2( Random.Range( -area.x / 2, area.x / 2 ), Random.Range( -area.y, area.y ) );
        return new Vector3( randomPoint.x, transform.position.y + 100, randomPoint.y );
    }

    private bool CheckOverlap(Vector3 pos, float radius) {
        Collider[] colliders = Physics.OverlapSphere( pos, radius, spawnPropLayer);
        return colliders.Length != 0;
    }

}
