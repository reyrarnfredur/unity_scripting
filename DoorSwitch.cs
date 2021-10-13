using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DoorSwitch : MonoBehaviour
{

    public Door relatedDoor;

    private void OnDrawGizmos() {
        if(relatedDoor.closed){
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine( transform.position, relatedDoor.transform.position );
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(relatedDoor != null) relatedDoor.Open();
        }
    }

}
