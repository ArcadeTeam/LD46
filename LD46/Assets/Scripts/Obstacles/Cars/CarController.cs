﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarController : MonoBehaviour
{
    private float distanceThreshold = 10f;
    public Transform waypoints;
    private List<Transform> positions;

    public Transform currentTarget;
    private int currentPosition;

    private NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        positions = new List<Transform>();

        foreach (Transform child in waypoints)
        {
            //Debug.Log(child.name);
            positions.Add(child);
        }

        InitializeFirstPosition();

        agent = GetComponent<NavMeshAgent>();
        agent.destination = currentTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPatrolNextPosition();
    }

    private void InitializeFirstPosition()
    {
        if (currentTarget != null)
            for (int i = 0; i < waypoints.childCount; i++) 
            { 
                //Debug.Log(child.name);
                if (currentTarget == waypoints.GetChild(i))
                    currentPosition = i;
            }
        
        else
            currentPosition = 0;
        currentTarget = positions[currentPosition];
    }


    private void CheckPatrolNextPosition()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) < distanceThreshold)
        {
            UpdateCurrentPosition();
        }
    }

    private void UpdateCurrentPosition()
    {
        currentPosition = GetNextPosition();
        currentTarget = positions[currentPosition];
        /*Debug.Log(currentPosition);
        Debug.Log(currentTarget);*/
        agent.destination = currentTarget.position;
    }

    private int GetNextPosition()
    {
        int nextPosition = currentPosition + 1;

        if (nextPosition >= positions.Count)
            nextPosition = 0;

        return nextPosition;
    }

}
