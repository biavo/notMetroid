using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    float speed = 12f;
    public float morphSpeed = 10f;
    public Vector3 velocity;
    float gravityBase = -19.62f;
        // ground, water, wall jumping, and checking for the roof all use
        // the same style of checking, but with different sized areas to 
        // check in.
    public bool isGrounded;
    Transform groundCheck;
    float groundDistance = 0.2f;
    public LayerMask groundMask;
    bool isWatered;
    Transform waterCheck;
    float waterDistance = 0.4f;
    public LayerMask waterMask;
    bool isCamWatered;
    Transform waterCamCheck;
    float waterCamDistance = 0.01f;
    GameObject waterCam;
    bool isRoof;
    Transform roofChecker;
    float roofDistance = 0.1f;
    public LayerMask roofMask;
    float jumpHeight = 3f;
        //maximum jumps and dashes can be modified, but are currently set
        //to 2 of each until upgrades are worked on
    public int jumpsMax;
    public int jumpsLeft;
    public int dashesMax;
    public int dashesLeft;
    bool chargingDash;
    float dashChargeClock;
    float dashChargeTimer = 1f;
    bool dashing;
    float dashTimer = .2f;
    float dashClock;
        //these are so you can see how many dash/jumps you have on the screen
    GameObject jumpText;
    GameObject dashText;
    GameObject mainCamera;
    public bool isMorph;
        //mercy jump is adding a tiny bit of time after falling off a ledge
        //while keeping the maximum jumps
    bool groundedMercyJump;
    float gMJTimer;
    public LayerMask wallJumpMask;
    float wallJumpSphere = 1.2f;
    float wallJumpLeavingSphere = 2.4f;
    public bool canWallJump;
    public bool touchingWall;
    public bool walljumped;
    public bool grabbingWall;



    void Awake(){
            //setting all check objects to their proper place
        dashesLeft = dashesMax;
        groundCheck = GameObject.Find("GroundCheck").transform;
        waterCheck = GameObject.Find("WaterCheck").transform;
        roofChecker = GameObject.Find("RoofCheck").transform;
        waterCamCheck = GameObject.Find("WaterCamCheck").transform;
        waterCam = GameObject.Find("WaterCamera");
        mainCamera = GameObject.Find("Main Camera");
        jumpText = GameObject.Find("JumpText");
        dashText = GameObject.Find("DashText");
        isMorph = false;
    }

    void Update()
    {
        if(Input.GetButtonDown("Morph")){
            if(isMorph){
                    //!morphball is just setting scale of player to .5 
                    //and scaling back to full when needed
                isMorph = false;
                transform.localScale = new Vector3(1,1,1);
                mainCamera.GetComponent<GunShooter>().gun.transform.localScale = new Vector3(.4f,1,.4f);
            } else { 
                isMorph = true;
                transform.localScale = new Vector3(1,0.5f,1);
                mainCamera.GetComponent<GunShooter>().gun.transform.localScale = new Vector3(.4f, 1, .8f);
            }
        }
        jumpText.GetComponent<Text>().text = "Jumps left: " + ((jumpsLeft).ToString());
        dashText.GetComponent<Text>().text = "Dashes left: " + ((dashesLeft).ToString());
        
            //checking if physics checks are active or not
        isCamWatered = Physics.CheckSphere(waterCamCheck.position, waterCamDistance, waterMask);
        isWatered = Physics.CheckSphere(waterCheck.position, waterDistance, waterMask);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isRoof = Physics.CheckSphere(roofChecker.position, roofDistance, roofMask);
        canWallJump = Physics.CheckSphere(groundCheck.position, wallJumpSphere, wallJumpMask);
        touchingWall = Physics.CheckSphere(groundCheck.position, wallJumpLeavingSphere, wallJumpMask);

        if(isGrounded && velocity.y <0){    //y velocity while grounded is slightly lower than 0 to keep the player grounded
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");  //setting wasd to modifiers for later use
        float z = Input.GetAxis("Vertical");    //also works on controller for movement but camera doesn't
        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 verticalMove = transform.right * x + transform.up * z;

        if(isGrounded){     //all jumps and dashes regen while grounded, jumps are instant
            jumpsLeft = jumpsMax;
            gMJTimer = 0;
            groundedMercyJump = true;
            if(!chargingDash && (dashesLeft < dashesMax)){
                chargingDash = true;
                dashChargeClock = Time.time;
            }
        }
        if(!isGrounded && groundedMercyJump){   //mercy jump code
            gMJTimer += 1f * Time.deltaTime;
            if(gMJTimer >= .15f){
                jumpsLeft--;
                groundedMercyJump = false;
            }
        }

        if(Input.GetButtonDown("Run") && !dashing && dashesLeft >0){    //dashing underwater is slower so it uses same value as regular walking
            dashing = true;
            dashesLeft --;
            dashClock = Time.time;
        }
        if(dashing){
            if(isWatered){
                controller.Move(move * speed * Time.deltaTime);
                if(dashClock <= (Time.time - dashTimer)){
                    dashing = false;
                }
            } else {
                controller.Move(move * speed * Time.deltaTime * 3);
                if(dashClock <= (Time.time - dashTimer)){
                    dashing = false;
                }
            }
        }
        if(chargingDash && dashChargeClock <= (Time.time - dashChargeTimer)){ //if dash charge timer runs out then more dash
            dashesLeft ++;
            chargingDash = false;
        }
        if(Input.GetButtonDown("Jump") && jumpsLeft !=0 && !isMorph){   //jump
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityBase);
            jumpsLeft --;
            groundedMercyJump = false;
            grabbingWall = false;
        }
        if(canWallJump && touchingWall && !walljumped && jumpsLeft < jumpsMax){ //jump into a walljump surface and it regens 1 jump
            jumpsLeft ++;
            walljumped = true;
        }
        if(!touchingWall){
            walljumped = false;
        } 
        if(canWallJump && (Input.GetButtonDown("Wall Grab"))){      //y velocity set to 0 and jumps set to max while grabbing wall
            grabbingWall = true;
            jumpsLeft = jumpsMax;
            velocity.y = (0);
        } else if (!canWallJump){
            grabbingWall = false;
        }
        if(isRoof){         //hit a roof and you fall down
            velocity.y = (0);
        }
        if(isWatered){                                                  //all movement happens in here
            controller.Move(move * (speed * 0.7f) * Time.deltaTime);    //water makes you slow
            velocity.y +=gravityBase/2 * Time.deltaTime;                //grappling brings you to the lockedon target
            controller.Move(velocity/3 * Time.deltaTime);               //grabbing wall moves you up/down with w/s
        } else if(mainCamera.GetComponent<MouseLook>().grapple){        //otherwise you move regularly
            velocity.y +=gravityBase * 0 * Time.deltaTime;
        } else if (grabbingWall){
            velocity.y +=gravityBase * 0 * Time.deltaTime;
            controller.Move(verticalMove * speed * Time.deltaTime);
        }else{
            controller.Move(move * speed * Time.deltaTime);
            velocity.y +=gravityBase * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        if(isCamWatered){                       //water filter on camera when underwater
            waterCam.SetActive(true);
        } else { 
            waterCam.SetActive(false);
        }
    }
}