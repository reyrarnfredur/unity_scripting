using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {

    public Vector3 Position { get => transform.position; set => transform.position = value; }
    public bool locked = false;

    public Point( bool locked = false ) {
        this.locked = locked;
    }

    private void OnDestroy() {
        Debug.Log( $"Point has been destroyed!" );
    }

}
