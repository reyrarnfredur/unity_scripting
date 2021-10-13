using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrarium 
{

    [RequireComponent( typeof (VerletRope) )]
    public class VerletGravity : MonoBehaviour {

        public bool applyGravity = false;
        public Vector3 gravityDirection = -Vector3.up;
        public float gravityForce = Mathf.Abs(Physics.gravity.y);

        private VerletRope verletRope;

        private void Awake() {
            verletRope = GetComponent<VerletRope>();
        }

        private void Update() {
            
            if(applyGravity) {
                for (int i = 0; i < verletRope.points.Length; i++) {
                    verletRope.points[i].transform.position += gravityDirection * gravityForce * Time.deltaTime;
                }
            }

        }

    }

}


