using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour {

    public Transform moveLocation;
    NavMeshAgent navAgent;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();

        navAgent.Warp(transform.position);

    }
	
	// Update is called once per frame
	void Update () {
        navAgent.SetDestination(moveLocation.position);
        
	}
}
