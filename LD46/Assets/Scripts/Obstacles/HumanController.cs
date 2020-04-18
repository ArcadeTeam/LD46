using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanController : MonoBehaviour
{
    public BoxCollider area;
    private float walkRadius;
    private float changePathTimeout = 5f;
    private float walkThreshold = 2f;

    private Vector3 lastPosition;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = GetRandomPoint();

        walkRadius = Random.Range(10f, 30f);
        lastPosition = transform.position;
        InvokeRepeating("ChangePath", 0f, Random.Range(3f, changePathTimeout));
    }

    private void ChangePath()
    {
        if (Vector3.Distance(transform.position, lastPosition) < walkThreshold)
        {
            var nextPosition = GetRandomPoint();
            if (nextPosition.x != Mathf.Infinity)
            {
                //Debug.Log(nextPosition);
                agent.destination = nextPosition;
            }
        }

        lastPosition = transform.position;
    }


    private Vector3 GetRandomPoint()
    {
        if (area != null)
        {
            Vector3 randomPos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            randomPos = area.transform.TransformPoint(randomPos * .5f);

            return randomPos;
        }
        else
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

            return hit.position;
        }
    }
}