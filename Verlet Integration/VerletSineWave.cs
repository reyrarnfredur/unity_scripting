using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrarium {

    [RequireComponent(typeof(VerletRope))]
    public class VerletSineWave : MonoBehaviour{

        [SerializeField] private int fromIndex = 0;
        [SerializeField] private int toIndex = 3;
        [SerializeField] private float wavingSpeed = 1;
        [SerializeField] private float wavingRange = 1;
        [SerializeField] private float wavingOffset = 0.2f;
        [SerializeField] private float adjustStrength = 10;
        [SerializeField] private bool adjust = false;

        private float minimumContractionLength = 0;
        private Vector3[] startPositions;
        private Transform pointT;
        private VerletRope rope;

        private void Awake() {
            rope = GetComponent<VerletRope>();
        }

        private void Start() {
            minimumContractionLength =
               (rope.points[fromIndex].transform.position - rope.points[toIndex].transform.position).sqrMagnitude / 1.5f;
        }

        private void Update() {

            for (int i = fromIndex; i < toIndex; i++) {
                
                pointT = rope.points[i].transform;
                pointT.position += pointT.up * Mathf.Sin( Time.time + i * wavingSpeed ) * wavingRange;

            }

            if(adjust) {

                pointT = rope.points[rope.length - 1].transform;

                float distanceToFirst = (pointT.position - pointT.position).sqrMagnitude;

                if (distanceToFirst < minimumContractionLength) {
                    pointT.position += pointT.right * adjustStrength * Time.deltaTime;
                }

            }

        }

    }

}

