using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public Transform animatedTransform;
    private Animator animator;
    public bool locked = true;
    public bool closed = true;

    private void Awake() {
        animator = animatedTransform.GetComponent<Animator>();
    }

    public void Open(){
        closed = false;
        animator.SetBool( "opened", true );
    }

    public void Close(){
        closed = true;
        animator.SetBool( "opened", false );
    }

}
