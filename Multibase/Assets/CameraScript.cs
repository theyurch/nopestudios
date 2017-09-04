using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform target;
    Quaternion lastRot;
    public Vector3 offset;
    
    Vector3 lastLoc;
    public float delayFactor = 0.05f;
    public Vector3 camModify;
    Quaternion offRot;

    // Use this for initialization
    void Start () {
        lastRot = Quaternion.identity;
        camModify = Vector3.zero;
}
	
	// Update is called once per frame
	void LateUpdate () {
        
        if (target)
        {
            //lastRot = Quaternion.Lerp(lastRot, Quaternion.LookRotation(target.position - transform.position), delayFactor);
            lastRot = target.rotation;
            transform.rotation = lastRot;
            lastLoc = Vector3.Lerp(lastLoc, target.position, delayFactor);

            //lastLoc = target.position;
           
            offRot = Quaternion.Euler(camModify);
            transform.position = lastLoc + ((offRot * target.rotation) * offset);
            

        }
        
	}
}
