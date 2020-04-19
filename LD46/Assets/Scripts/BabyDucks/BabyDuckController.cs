using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyDuckController : Duck
{
    public Transform destination;
    NavMeshAgent agent;
    int distractionProbability = 0;
    [SerializeField] Animator animator;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (dead) return;

        animator.SetBool("Running", agent.velocity.magnitude > 0f);           

        if (destination != null)
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

    public void HumanDetected(Vector3 humanPosition)
    {
        Vector3 dir = transform.position - humanPosition;
        agent.destination = humanPosition + dir * 3;
        destination = null;
    }
}
