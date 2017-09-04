using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class FPSMovementRegular : NetworkBehaviour
{
    float x, z, rx, rz;
    bool y, r;
    bool fire3;
    bool jump;
    float vertVelocity = 0;
    GameObject body;
    // Rigidbody bodyRigid;
    CharacterController bodyController;
    Transform bodyPos;
   // Vector3 rotVect = Vector3.zero;
    Vector3 showVect = Vector3.zero;

    public float jumpVel = 10.0f;
    public float moveFactor = 10;
    public float turnFactor = 10;

    

    public Vector3 CameraRotation = Vector3.zero;


    Animator animControl;

    // Use this for initialization
    void Start()
    {

        body = this.gameObject;
      //  bodyRigid = body.GetComponent<Rigidbody>();
        bodyPos = body.GetComponent<Transform>();
        bodyController = body.GetComponent<CharacterController>();
        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CameraScript>().target = bodyPos;
        }

        animControl = GetComponent<Animator>();
    }


    void Relocate()
    {
        if (isLocalPlayer)
        {
            body.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

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

        //Input.GetAxisRaw()
        rx = Input.GetAxis("Right Horizontal");
        rz = Input.GetAxis("Right Vertical");

        rx += Input.GetAxis("Mouse X");
        rz -= Input.GetAxis("Mouse Y");

        var rotVect = new Vector3(rz, rx, 0);
        //var moveVect = new Vector3(x, 0, z);
        var moveVect = Vector3.zero;
        moveVect += transform.forward.normalized * z;
        moveVect += transform.right.normalized * x;

        if (moveVect.magnitude < 1)
        {
            moveVect = moveVect.magnitude * moveVect.normalized;
        }
        else
        {
            moveVect = moveVect.normalized;
        }

        if (moveVect.magnitude > 0.001)
        {
            animControl.SetBool("isMoving", true);
        }
        else
        {
            animControl.SetBool("isMoving", false);
        }
        x = 0;
        z = 0;
        rx = 0;
        rz = 0;


        if (r || fire3)
        {
            Relocate();
            // RpcRespawn();
        }
        

        moveVect = moveVect * Time.deltaTime * moveFactor;
        vertVelocity += Time.deltaTime * Physics.gravity.y;
        moveVect.y += vertVelocity * Time.deltaTime;
        transform.Rotate(rotVect);
        if (bodyController.isGrounded)
        {

            moveVect.y = 0;
            vertVelocity = 0;
            if(y)
            {
                vertVelocity = jumpVel;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), Time.deltaTime * 10);
            //transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        }
        showVect = moveVect;
        bodyController.Move(moveVect);
        
        
        
        

        
                
    }
    void OnDrawGizmos()
    {
        if (bodyPos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(bodyPos.position, showVect * 100.0f);
        }
    }

}
