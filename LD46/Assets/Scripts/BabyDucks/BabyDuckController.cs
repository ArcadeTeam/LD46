using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyDuckController : Duck
{
    public Transform destination;
    NavMeshAgent agent;
    int distractionProbability = 0;
    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (dead) return;

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

    private void OnTriggerEnter(Collider other) {
        if (dead) return;

        if (other.CompareTag("Human")) {
            Vector3 dir = transform.position - other.transform.position;
            agent.destination = other.transform.position + dir * 3;
            destination = null;
        }

        if (other.CompareTag("Player")) {
            other.gameObject.GetComponent<DuckController>().nearBabies.Add(this);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (dead) return;

        if (other.CompareTag("Player")) {
            other.gameObject.GetComponent<DuckController>().nearBabies.Remove(this);
        }
    }

    public void killDuck(Vector3 impactOrientation, float impactSpeed = 1.0f)
    {
        dead = true;
        agent.enabled = false;
        GetComponent<Rigidbody>().freezeRotation = false;
        _body.velocity = impactOrientation.normalized * impactSpeed;
        _body.AddTorque(new Vector3(0f, 10f, 10f));
    }
}
