using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyDuckController : MonoBehaviour
{
    public Transform destination;
    NavMeshAgent agent;
    int distractionProbability = 0;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if(destination != null)
            agent.destination = destination.position;
    }

    public void Distract(Transform newDestination) {
        distractionProbability += 10;
        if(distractionProbability > Random.Range(0, 100)) {
            destination = newDestination;
        }
    }

    public void GoWithMom(Transform newDestination) {
        distractionProbability = 0;
        destination = newDestination;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Human")) {
            Vector3 dir = transform.position - other.transform.position;
            agent.destination = other.transform.position + dir * 3;
            destination = null;
        }
    }

    private void OnTriggerExit(Collider other) {
        //if (other.CompareTag("Human"))
            
    }
}
