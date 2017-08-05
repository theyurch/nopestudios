using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class wingPhys : MonoBehaviour
{

    public Rigidbody wingRigidbody; //the physics actor we're going to act on
    Transform wingLocation;    //the location of the wing script
    Vector3 wingVelocity; //this will take angular velocity into account
    public Vector3 planeAngle = new Vector3(0,1,0); //What angle does this airfoil create forces on? Default is a horizontal wing orientation

    public float airDensity = 1.3f;
    public float wingArea = 0.5f;
    public float camberLift = 0.0f;
    public float liftOffset = 0.25f;
    float liftVisual;
    public float stallThreshold = 1.5f;


    float lastMag;
    public float minDrag = 0.05f;

    // Use this for initialization
    void Start()
    {

        wingLocation = this.transform;
        //wingRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
    void FixedUpdate()
    {
        wingVelocity = wingRigidbody.GetPointVelocity(wingLocation.position);
        //This script acts on the 'up' axis of the attached rigidbody. I should probably add support for other axis.

        //thin airfoil theory:
        //center of pressure and aerodynamic center lie one quarter of the chord behind the leading edge
        //section lift coefficient = (lift at zero) + 2 * pi * angle of attack

        //lift force = 1/2 (air density) * vel^2 * wing area * lift coefficent at desired angle
        //lift is perpendicular to flow direction
        float AoA = Mathf.Deg2Rad * (Vector3.Angle(transform.up, wingVelocity) - 90.0f);
        
        float liftCoefficient = camberLift + (2 * Mathf.PI * AoA);

        //WHAT ARE YOU DOING

        if (liftCoefficient > stallThreshold)
        {
            liftCoefficient = stallThreshold;
        }
        else if (liftCoefficient < stallThreshold * -1)
        {
            liftCoefficient = stallThreshold * -1;
        }


        //Debug.Log(liftCoefficient);
        //float liftMag = Mathf.Clamp(0.5f * airDensity * wingVelocity.sqrMagnitude * wingArea * liftCoefficient, maxLiftForce * -1, maxLiftForce);
        float liftMag = 0.5f * airDensity * wingVelocity.sqrMagnitude * wingArea * liftCoefficient;

        //we have problems with 'boundry wiggle', caused by rapid shifts in force direction. 
        //This happens when the airfoils are flying 'straight' and fast, and is possibly a problem due to simulation tickrate
        //Or just, maybe, unrealisticly high acting surface area

        
        //wingRigidbody.AddForce(wingRigidbody.transform.up * liftMag, ForceMode.Acceleration);
        liftVisual = liftMag * 0.01f;

        //drag
        //wingRigidbody.AddForce(((Mathf.Abs(liftCoefficient) / 6.0f) + minDrag) * -0.5f * wingVelocity.sqrMagnitude * wingVelocity.normalized, ForceMode.Force);
        wingRigidbody.AddForceAtPosition(((Mathf.Abs(liftCoefficient) / 6.0f) + minDrag) * -0.5f * wingVelocity.sqrMagnitude * wingVelocity.normalized, wingLocation.position, ForceMode.Force);

        //lift
        wingRigidbody.AddForceAtPosition(wingLocation.rotation * planeAngle * liftMag, wingLocation.position + wingLocation.forward * liftOffset, ForceMode.Force);



    }
    void OnDrawGizmos()
    {
        if (wingRigidbody)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(wingLocation.position + wingRigidbody.transform.forward * liftOffset, wingLocation.rotation * planeAngle * liftVisual);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(wingLocation.position, wingVelocity);
        }

    }
}
