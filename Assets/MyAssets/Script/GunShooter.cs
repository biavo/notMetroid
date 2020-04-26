using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;
    public GameObject missile;
    public GameObject gun;
    public GameObject bulletSpawner;
    public GameObject currentBullet;
    public enum gunState{GUN1, GUN2, GUN3, GUN4};
    public gunState ActiveState;

    void Start(){
        ActiveState = gunState.GUN1;
    }
    void Update()
    {
        if(Input.GetButtonDown("Weapon1")){                 //push button get new bullet
            ActiveState = gunState.GUN1;
        }
        if(Input.GetButtonDown("Weapon2")){
            ActiveState = gunState.GUN2;
        }
        if(Input.GetButtonDown("Weapon3")){
            ActiveState = gunState.GUN3;
        }
        if(Input.GetButtonDown("Weapon4")){
            ActiveState = gunState.GUN4;
        }
        if(Input.GetButtonDown("Fire1")){
            Shooting(currentBullet, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
        }
        if(Input.GetButtonDown("Missile")){
            Shooting(missile, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
        }
        switch(ActiveState){
            case gunState.GUN1:
                currentBullet = bullet1;
                gun.GetComponent<Renderer>().material.SetColor("_Color", Color.red);            //gun changes color
                break;
            case gunState.GUN2:
                currentBullet = bullet2;
                gun.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                break;
            case gunState.GUN3:
                currentBullet = bullet3;
                gun.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                break;
            case gunState.GUN4:
                currentBullet = bullet4;
                gun.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                break;
            default: 
                print("What.");
                break;
            }
    }
    void Shooting(GameObject bulletType, Vector3 gunTransform, Quaternion transrotat){
            Instantiate(bulletType, gunTransform, transrotat);
    }
}