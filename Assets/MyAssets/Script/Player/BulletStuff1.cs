﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStuff1 : MonoBehaviour
{
    float timer;

	
	void OnCollisionEnter (Collision collision){
        Destroy(gameObject);
    }
	void FixedUpdate (){
        this.transform.Translate(0,0,1f, Space.Self);// moves bullet forward
		timer += 1.0F * Time.deltaTime;
		if (timer >= 2){
			GameObject.Destroy (gameObject);
		}
    }
}