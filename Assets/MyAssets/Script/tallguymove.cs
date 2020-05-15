using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tallguymove : MonoBehaviour
{
    public bool jumping;
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform player;
    public Vector3 pv;
    public Vector3 p1v;
    public Vector3 p2v;
    public Vector3 p3v;
    public float xDistance;
    public float zDistance;
    public float timer;
    public float jumpTimer;


    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= jumpTimer && !jumping){
            GetComponent<ParabolaController>().FollowParabola();
            jumping = true;
            timer = -1f;
        }
        pv = player.transform.position;
        p1v = p1.position;
        p2v = p2.position;
        p3v = p3.position;
        xDistance = p3.position.x - p1.position.x;
        zDistance = p3.position.z - p1.position.z;
        if(!jumping){
            p1.transform.position = transform.position;
            p2.transform.position = new Vector3(transform.position.x + xDistance/2, (transform.position.y + 32f), transform.position.z + zDistance/2);
            p3.transform.position = new Vector3(player.position.x, player.position.y-2f, player.position.z);
        }
        if(!GetComponent<ParabolaController>().Animation){
            jumping = false;
        }
    }

  //  void Jump(){}
}
