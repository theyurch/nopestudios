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
    //GameObject cubeCamera;
    /*
    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        if (isLocalPlayer)
        { 
            Camera.main.GetComponent<CameraScript>().target = transform;
        }
    }
    */
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
        //cubeCamera = transform.Find("Camera").gameObject;

        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CameraScript>().target = transform;
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var vectFor = new Vector3(z, 0, -x);
        vectFor = Camera.main.GetComponent<Transform>().rotation * vectFor;
        moveDir = Vector3.Cross(vectFor, new Vector3(0, 1, 0));
     //   moveDir.z *= z;
     //   moveDir.x *= x;
        // moveDir = new Vector3(x, 0, z);

        bodyRigid.AddForceAtPosition
            (moveDir.normalized * 7, 
            bodyPos.position, 
            ForceMode.Acceleration);
        
        //transform.Translate(x, 0, z);
    }
    void OnDrawGizmos()
    {
       // if ()
       // {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(bodyPos.position, moveDir * 10);
       // }

    }
}
