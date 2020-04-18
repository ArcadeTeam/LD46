using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyDuckController : MonoBehaviour
{
    public Transform destination;
    NavMeshAgent agent;
    public int distractionProbability = 0;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
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
}
