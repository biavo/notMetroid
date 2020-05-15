using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform movingCube;
    public Transform place1;
    public Transform place2;
    public Transform target;
    public bool direction;
    public float speed = 5;
    
    void Start()
    {
        movingCube = transform.GetChild(0);
        place1 = transform.GetChild(1);
        place2 = transform.GetChild(2);
        target = place1;
    }

    // Update is called once per frame
    void Update()
    {
        float step =  speed * Time.deltaTime; // calculate distance to move
        movingCube.position = Vector3.MoveTowards(movingCube.position, target.position, step);
        if(movingCube.position == target.position){
            direction = !direction;
        }
        if(direction){
            target = place2;
        } else{
            target = place1;
        }
    }
}
