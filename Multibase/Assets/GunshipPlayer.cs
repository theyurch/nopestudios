﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;


public class GunshipPlayer : NetworkBehaviour
{
    GameObject body;
    Rigidbody bodyRigid;
    Transform bodyPos;
    Rigidbody liftPoint;
    Transform liftLoc;
    public Vector3 moveDir;

    float x, z;
    bool y;
    bool r;
    bool fire3;


    public Vector3 CameraRotation = Vector3.zero;

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
    void Start()
    {
        /*
         body = transform.Find("PlayerCube").gameObject;
         bodyPos = body.GetComponent<Transform>();
         bodyRigid = body.GetComponent<Rigidbody>();
         */
        body = this.gameObject;
        //body = transform.Find("Body").gameObject;
        //bodyPos = body.GetComponent<Transform>();
        bodyRigid = body.GetComponent<Rigidbody>();
        //cubeCamera = transform.Find("Camera").gameObject;
        //bodyPos = transform.Find("Body").gameObject.GetComponent<Transform>();
        bodyPos = this.gameObject.GetComponent<Transform>();
        //liftPoint = transform.Find("LiftPoint").gameObject.GetComponent<Rigidbody>();

        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CameraScript>().target = bodyPos;
        }

    }
    /*
    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            transform.position = Vector3.zero;
        }
    }
    */
    void Relocate()
    {
        if (isLocalPlayer)
        {
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        y = Input.GetButton("Jump");
        r = Input.GetKey("r");
        fire3 = Input.GetButton("Fire3");



        Camera.main.GetComponent<CameraScript>().camModify = new Vector3(0, Input.GetAxis("Right Horizontal") * 80, 0/*Input.GetAxis("Right Vertical") * 80*/);
        //Debug.Log(new Vector3(Input.GetAxis("Right Vertical"), 0, Input.GetAxis("Right Horizontal")));

        if (r || fire3)
        {
            Relocate();
          // RpcRespawn();
        }
      //  var vectFor = new Vector3(x, z, 0);
      //  moveDir = bodyPos.rotation * vectFor;

        // moveDir = Vector3.Cross(vectFor, new Vector3(0, 1, 0));
        //   moveDir.z *= z;
        //   moveDir.x *= x;
        // moveDir = new Vector3(x, 0, z);

        //offset to simulate rotor 'tipping'
        /*
        liftPoint.AddForceAtPosition
            (moveDir.normalized * rotorHorzOffset,
            liftLoc.position + (bodyPos.rotation * new Vector3(0, rotorVertOffset, 0)),
            ForceMode.Acceleration);
        */
        //constant upwards at rotor
    }
    private void FixedUpdate()
    {
        bodyRigid.AddRelativeTorque((new Vector3(z, 0, -x)).normalized, ForceMode.Acceleration);
        /*
        bodyRigid.AddForceAtPosition
            (moveDir.normalized * rotorHorzOffset,
            bodyPos.position + (bodyPos.rotation * new Vector3(0, 0, -rotorVertOffset)),
            ForceMode.Acceleration);
        */
        //constant uprighting
        bodyRigid.AddRelativeForce
                (new Vector3(0, 1, 0) * 9.0f,
                ForceMode.Acceleration);
                
        if (y)
        {
            bodyRigid.AddRelativeForce
                    (new Vector3(0, 1, 0) * 3.0f,
                    ForceMode.Acceleration);

            /*
            bodyRigid.AddForceAtPosition
                ((bodyPos.rotation * new Vector3(0, 1, 0)) * 3,
                bodyRigid.worldCenterOfMass,
                ForceMode.Acceleration);
                */
        }

    }


    void OnDrawGizmos()
    {
        // if ()
        // {
        Gizmos.color = Color.green;
        //Gizmos.DrawRay(bodyPos.position + (bodyPos.rotation * new Vector3(0, 0, -rotorVertOffset)), moveDir * 10);
        // }

    }
}
