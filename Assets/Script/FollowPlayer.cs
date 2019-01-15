using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    private Transform playerTransform;
    //public Vector3 offset;
	void Start () {
    //offset = new Vector3(0f,0f,80f);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform ;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.forward * playerTransform.position.z; //+ offset; 
	}
}
