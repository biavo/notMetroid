using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMove : MonoBehaviour
{
    float timer;
    GameObject mainCam;
    Transform target;
    public Vector3 targetV3;
    bool lockedon;
    public float missileSpeed;                      //projectile speed and turning speed;
    public float rotationSpeed;
    Quaternion turnRotation;
    Vector3 turnDirection;
    bool hasTarget;
    bool buttonReleased;
    public float spinSpeed;

    void Awake(){
        mainCam = GameObject.Find("Main Camera");
        if(Input.GetButton("Fire2")){
            lockedon = true;
        }
    }
    void OnTriggerEnter (Collider collision){
        Destroy(gameObject);
    }
    void Update (){
        transform.Rotate(new Vector3(0,0,spinSpeed), Space.Self);
        if(Input.GetButtonUp("Fire2")){
            buttonReleased = true;
        }
        if(lockedon){
            if(!mainCam.GetComponent<MouseLook>().hasTarget){
                if(Input.GetButton("Fire2") && !buttonReleased){
                    targetV3 = mainCam.GetComponent<MouseLook>().hitPoint;
                    print("locked but no target");                                  //this code was a nightmare
                    turnDirection = (targetV3 - transform.position).normalized;     //missiles follow mouse when locked on but have no target
                    turnRotation = Quaternion.LookRotation(turnDirection);          //but go in straight line based on previous direction when let go of mouse2
                    transform.rotation = Quaternion.Slerp(transform.rotation, turnRotation, Time.deltaTime * rotationSpeed);
                    transform.Translate(0,0,missileSpeed, Space.Self);
                } else {
                transform.Translate(0,0,missileSpeed, Space.Self);                  //this is the straight line code
                }
            } else {
                if(!hasTarget){                                                     //if locked on and have a target the missile will follow target until death
                    target = mainCam.GetComponent<MouseLook>().target;              //turning over time instead of instantly facing the target
                    hasTarget = true;
                    print("locked and has target");
                } else{
                    turnDirection = (target.position - transform.position).normalized;
                    turnRotation = Quaternion.LookRotation(turnDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, turnRotation, Time.deltaTime * rotationSpeed);
                    transform.Translate(0,0,missileSpeed, Space.Self);
                }
            }
        } else if(!lockedon){
            if(!hasTarget){
                targetV3 = mainCam.GetComponent<MouseLook>().hitPoint;
                hasTarget = true;                                                   //if no target and no lockon then it flies straight 
                print("no lock");                                                   //needed a vector3 instead of transform because it's picky
                transform.LookAt(targetV3);
            } else{
                transform.Translate(0,0,missileSpeed, Space.Self);
            }
        }
        timer += 0.3f * Time.deltaTime;
        if (timer >= 2){
            GameObject.Destroy (gameObject);
        }
    }
}