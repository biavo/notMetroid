using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tallguymove : MonoBehaviour
{
    bool jumping;
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform player;
    float xDistance;
    float yDistance;
    float zDistance;
    public float timer;
    public float jumpTimer;
    Vector3 hitpoint;
    Vector3 pv;
    public LayerMask jumpMask;
    public float playerDistance;
    NavMeshAgent agent;
    public bool eating;
    bool aggro;
    public float wanderRadius;
    public float wanderTimer;
    public float wanderTimerRange; 

    void Awake(){
        aggro = false;
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTimer;
        wanderTimer = Random.Range(wanderTimer-wanderTimerRange, wanderTimer+wanderTimerRange);
    
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.transform == player){
            aggro = true;
        }
    }

    void Update(){
        if(!aggro){
            timer += Time.deltaTime;
            if (timer >= wanderTimer) {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
                wanderTimer = Random.Range(wanderTimer-wanderTimerRange, wanderTimer+wanderTimerRange);
            }
        } else{
            jumping = GetComponent<ParabolaController>().Animation;
            agent = GetComponent<NavMeshAgent>();
            timer += Time.deltaTime;
            if(timer >= jumpTimer && !jumping && playerDistance >= 15 && !eating){
                Jump();
            }
            if(playerDistance >3){
                eating = false;
            }
            if(playerDistance <= 3 && !eating){
                eating = true;
                timer = 0;
            }
            if(eating){
                agent.enabled = false;
                if(timer >= jumpTimer){
                    Jump();
                }
            }
            if (playerDistance < 15 && playerDistance > 3) {
                Walk();
                eating = false;
            }
            else{
            agent.enabled = false;
            }
            RaycastHit hit;
            pv = player.transform.position;
            playerDistance = Vector3.Distance(player.position, transform.position);
            xDistance = p3.position.x - p1.position.x;
            yDistance = p3.position.y - p1.position.y;
            zDistance = p3.position.z - p1.position.z;
            if(!jumping){
                p1.transform.position = transform.position;
                if(!eating){
                    p2.transform.position = new Vector3(transform.position.x + xDistance/2, transform.position.y + yDistance*2/3 + 30, transform.position.z + zDistance/2);
                } else {
                    p2.transform.position = new Vector3(transform.position.x + xDistance, transform.position.y + yDistance*2/3 + 30, transform.position.z + zDistance);
                }
                p3.transform.position = hitpoint;
            }else{
                timer = 0f;
            }
            Debug.DrawRay(player.position, Vector3.down, Color.green);
            if(Physics.Raycast(pv, Vector3.down, out hit, 100f, jumpMask)){
                hitpoint = hit.point;
            }
        }
    }

    void Walk(){
        timer = 1.5f;
        agent.enabled = true;
        agent.SetDestination(player.transform.position);
    }

    void Jump(){
        timer = 0;
        GetComponent<ParabolaController>().FollowParabola();
        agent.enabled = false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }
}