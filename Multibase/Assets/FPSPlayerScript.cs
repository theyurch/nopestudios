using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class FPSPlayerScript : NetworkBehaviour
{
    float x, z, rx, rz;
    bool y, r;
    bool fire3;

    GameObject body;
    Rigidbody bodyRigid;
    Transform bodyPos;
    Vector3 rotVect = Vector3.zero;
    Vector3 moveVect = Vector3.zero;

    public float maxMoveForce = 10;
    public float balanceTorque = 1;
    public float turnFactor = 10;

    public Vector3 CameraRotation = Vector3.zero;

    // Use this for initialization
    void Start () {

        body = this.gameObject;
        bodyRigid = body.GetComponent<Rigidbody>();
        bodyPos = body.GetComponent<Transform>();

        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CameraScript>().target = bodyPos;
        }
    }

    void Relocate()
    {
        if (isLocalPlayer)
        {
            body.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        }
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        y = Input.GetButton("Jump");
        r = Input.GetKey("r");

        //Input.GetAxisRaw()
        rx = Input.GetAxis("Right Horizontal");
        rz = Input.GetAxis("Right Vertical");

        rx += Input.GetAxis("Mouse X");
        rz -= Input.GetAxis("Mouse Y");

        rotVect += new Vector3(rz, 0, -rx);
        moveVect += new Vector3(x, 0, z);
        x = 0;
        z = 0;
        rx = 0;
        rz = 0;

        if (r || fire3)
        {
            Relocate();
            // RpcRespawn();
        }

    }

    void FixedUpdate()
    {

        //        bodyRigid.AddRelativeForce(new Vector3(x, 0, z).normalized * maxMoveForce, ForceMode.Acceleration);


        bodyRigid.AddRelativeTorque((rotVect.normalized) * turnFactor, ForceMode.Acceleration);
        
        rotVect = Vector3.zero;
        bodyRigid.AddTorque(Vector3.Cross(bodyPos.up, Vector3.up) * balanceTorque, ForceMode.Force);
        bodyRigid.AddRelativeForce((moveVect).normalized * maxMoveForce, ForceMode.Force);
        moveVect = Vector3.zero;
        /*
        vfinal ^ 2 = vinit ^ 2 + 2 * a * d;
        */


        //lookit this thing!
        //bodyRigid.MovePosition() Time.fixedDeltaTime
    }
    void OnDrawGizmos()
    {
         if (bodyPos)
         {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(bodyPos.position, bodyPos.forward);
         }

    }
}
