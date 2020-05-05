using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform target;
    public float xRotation = 0f;
    public float grappleSpeed;
    public Camera cam;
    public bool grapple;
    public Vector3 hitPoint;
    public Vector3 playerBodyPos;
    public float distance;
    public bool mercyJump;
    public bool groundedMercyJump;
    public LayerMask aimingLayerMask;   // set everything that the gun will aim at in here 
    public bool hasTarget;              //MAKE SURE BULLETS AREN'T IN
    public float gMJtimer;
    public bool isMorph;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot;
    
    
                        //If the mouse points at empty space then the gun will NOT follow
                        //There needs to be something that the mouse can point at to get it working
                        //Currently there is a transparent sphere covering the whole playfield 
                        //Can't see the dome but the mouse can tell it exists



    void Start(){
        Cursor.visible = true;
        hotSpot = new Vector2(cursorTexture.width/2, cursorTexture.height/2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
    void Update() {
        playerBodyPos = playerBody.position;
        isMorph = playerBody.GetComponent<PlayerMovement>().isMorph;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.red);
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);                    //black magic raycasts to let gun aim at mouse
        if(Physics.Raycast(ray, out hit, 1000, aimingLayerMask)){
            hitPoint = hit.point;
        }
        float step = grappleSpeed * Time.deltaTime;
        if(Input.GetButtonDown("Fire2")){                                       //seeing what the target is and deciding what to do
            if(hit.collider.tag.Equals("Enemy")){
                target = hit.collider.gameObject.transform;
                grapple = false;
                hasTarget = true;
            } else if(hit.collider.tag.Equals("Grapple") && !isMorph){          //no grapple while smol
                target = hit.collider.gameObject.transform;
                grapple = true;
                hasTarget = false;
            } else{
                target = null;
                hasTarget = false;
                grapple = false;
            }
        }
        if(!Input.GetButton("Fire2")){                                                  //what to do while not locked on to something
            Cursor.lockState = CursorLockMode.Locked;
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f , 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        } else {                                                                        //what to do while locked on 
            if(gameObject.transform.eulerAngles.x > 90){                                //player stares at target while the body only sort 
                xRotation = gameObject.transform.eulerAngles.x -360f;                   //of does the same thing
            } else{
                xRotation = gameObject.transform.eulerAngles.x;
            }
            if(target != null){                 //if you lock on but have no target you face in one direction while still having freeaim
                playerBody.transform.LookAt(new Vector3(target.transform.position.x, playerBody.transform.position.y, target.transform.position.z));
                transform.LookAt(target);
                Cursor.lockState = CursorLockMode.None;
                                                                //grapple move code is in mouselook instead of movement for some reason
                if(grapple && (Vector3.Distance(playerBody.transform.position, target.position) > 6f) && (Vector3.Distance(playerBody.transform.position, target.position) < 40f) ){
                    playerBody.transform.position = Vector3.MoveTowards(playerBody.transform.position, target.position, step);
                    if(playerBody.GetComponent<PlayerMovement>().jumpsLeft == 0 && mercyJump){
                        playerBody.GetComponent<PlayerMovement>().jumpsLeft ++;
                        mercyJump = false;                      //grappling while having no jumps gives you an extra as well
                    }
                } else {
                    grapple = false;
                }
            } else{
                Cursor.lockState = CursorLockMode.None;
            }
        }
        if(playerBody.GetComponent<PlayerMovement>().isGrounded){
            mercyJump = true;
        }
        if(Input.GetButtonUp("Fire2")){
            grapple = false;
         }
    }
}