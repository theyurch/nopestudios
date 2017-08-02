using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform target;
    Quaternion lastRot;

	// Use this for initialization
	void Start () {
        lastRot = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            lastRot = Quaternion.Lerp(lastRot, Quaternion.LookRotation(target.position - transform.position), 0.05f);
            transform.rotation = lastRot;
        }
        
	}
}
