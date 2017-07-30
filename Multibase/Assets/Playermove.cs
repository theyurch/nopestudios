using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class Playermove : NetworkBehaviour
{
    GameObject body;
    Rigidbody bodyRigid;
    Transform bodyPos;
    public Vector3 moveDir;

    // Use this for initialization
    void Start () {
       /*
        body = transform.Find("PlayerCube").gameObject;
        bodyPos = body.GetComponent<Transform>();
        bodyRigid = body.GetComponent<Rigidbody>();
        */
        body = this.gameObject;
        bodyPos = body.GetComponent<Transform>();
        bodyRigid = body.GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        moveDir = new Vector3(x, 0, z);
        
        bodyRigid.AddForceAtPosition
            (moveDir.normalized * 6, 
            bodyPos.position, 
            ForceMode.Acceleration);
        
        //transform.Translate(x, 0, z);
    }
}
