using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMover : MonoBehaviour
{
    public GameObject mainCam;
    public Vector3 target;
    public float speed = 50f;
    

    void Update(){
        target = mainCam.GetComponent<MouseLook>().hitPoint;        //gun will look at where the mouse is pointed
        Vector3 targetDirection = target - transform.position;      //free aim is only given while locked on so without
        float singleStep = speed * Time.deltaTime*4;                //the lock it will always aim at the center of screen
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}