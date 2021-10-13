using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform targetObject;
    [SerializeField] float lerpTime = 0.2f;

    Vector3 velocity = Vector3.zero;

    void LateUpdate() {
        float distance = Vector3.Distance(transform.position, targetObject.position);
        transform.position = Vector3.SmoothDamp(transform.position, targetObject.position, ref velocity, lerpTime);
    }
}
