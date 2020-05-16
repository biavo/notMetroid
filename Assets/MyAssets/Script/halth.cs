using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class halth : MonoBehaviour
{


    public int health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0){
            Destroy(this.gameObject);
        }
    }
    void OnCollisionEnter(Collision colider){
        print("wa");
        if(colider.gameObject.tag == "Bullet"){
            health -= colider.gameObject.GetComponent<BulletInfo>().bulletDamage;
            print("haliwodo");
        }
    }
}