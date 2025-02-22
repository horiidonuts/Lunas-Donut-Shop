using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;


public class debugnavmeshcube : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void OnDrawGismos()
    {
        
    }
}
