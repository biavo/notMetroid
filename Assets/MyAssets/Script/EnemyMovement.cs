using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] points;
    public int randomIndex;
    public Transform destinationPoint;                      //the only thing this script does is make the box move from one
    public float navspeed;                                  //capsule to another randomly choosing the next location from a list
    

    void Update(){
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
    if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            randomIndex = Random.Range(0, points.Length);
            destinationPoint = points[randomIndex];
            transform.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = destinationPoint.position;
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed = navspeed;
        }
    }
}
