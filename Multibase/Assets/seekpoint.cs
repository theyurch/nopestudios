using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seekpoint : MonoBehaviour {
    public Vector3 centerMass;
    public float height = 1.0f;
    public bool dynamic = true;
    public GameObject hip;

	// Use this for initialization
	void Start () {
	    	
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dynamic)
        {

            var bodies = this.transform.parent.GetComponentsInChildren<Rigidbody>();
            var cmnum = Vector3.zero;
            var cmden = 0.0f;

            foreach (Rigidbody body in bodies)
            {
                cmnum += body.mass * body.worldCenterOfMass;
                cmden += body.mass;
            }
            centerMass = cmnum / cmden;
            /*
            var layerMask = 1 << 8; //"walkables" layer
                                    // (~ inverts a bitmask)
            RaycastHit hit;
            //check downward to double height for a floor
            
            if (Physics.Raycast(centerMass, Vector3.down, out hit, height * 2, layerMask))
            {
                transform.position = hit.point + Vector3.up * height;
            }
            */
            var averageFoot = Vector3.zero;
            var legs = hip.GetComponent<HipLogic>().legs;
            foreach (GameObject leg in legs)
            {
                var thigh = leg.GetComponent<ThighScript>().GetComponent<Rigidbody>();
                var foot = leg.GetComponent<ThighScript>().foot;
                var legAnchor = leg.transform.TransformPoint(leg.GetComponent<ConfigurableJoint>().anchor);

                averageFoot += foot.GetComponent<Rigidbody>().position;

            }
            averageFoot /= legs.Length;
            transform.position = averageFoot + Vector3.up * height;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}
