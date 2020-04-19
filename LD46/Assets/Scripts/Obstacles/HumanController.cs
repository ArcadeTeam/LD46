using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


public enum CharState{Idle, Walking, Talking, Scared, Running }

public class HumanController : MonoBehaviour
{
    public BoxCollider area;
    private float walkRadius;
    private float changePathTimeout = 5f;
    private float walkThreshold = 2f;

    private Vector3 lastPosition;

    private NavMeshAgent agent;

    private Animator animator;
    
    private CharState currentState;
    private float lastStateChange = 0f;
    private float lastPathChange = 0f;
    private HumanController other;
    private Vector3 lastWalkingOrientation;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = GetRandomPoint();
        animator = GetComponent<Animator>();
        setState(CharState.Walking);
        walkRadius = Random.Range(10f, 30f);
        lastPosition = transform.position;
        setState(CharState.Idle);
    }


    void Update()
    {

        var close = getCloseHumans();

        //talk with someone
        if (currentState == CharState.Idle)
        {
            setState(CharState.Walking);
        }


        //talk with someone
            if (currentState == CharState.Walking && timeInThisState() > 6f)
        {
            if (close.Count > 0)
            {
                var success = close.First().otherWantsToTalk(this);
                if (success)
                {
                    other = close.First();
                    setState(CharState.Talking);
                   
                }
            }
        }

        //while talking, look at the other character
        if (currentState == CharState.Talking)
        {
            transform.forward = Vector3.Lerp(lastWalkingOrientation,(other.transform.position - transform.position), timeInThisState() * 5f);
        }

        //stop talking
            if (currentState == CharState.Talking && timeInThisState() > 8f)
        {
            ChangePath();
            setState(CharState.Walking);
        }



        if (currentState == CharState.Walking)
        {
                lastWalkingOrientation = transform.forward;
        }
        //move to another location
        if (currentState == CharState.Walking && timeInThisPath() > 3f)
        {
            ChangePath();
        }

    }


    public Boolean otherWantsToTalk(HumanController other)
    {
        if (currentState == CharState.Walking)
        {
            this.other = other;
            setState(CharState.Talking);
            return true;
        }

        return false;
    }

    private List<HumanController> getCloseHumans()
    {
        List<HumanController> humans = Physics.OverlapSphere(transform.position, 4f)
            .Select(hit => hit.transform.gameObject.GetComponent<HumanController>())
            .Where(hc => hc != null && hc != this)
            .ToList();


        return humans;
    }

    private float timeInThisState()
    {
        return Time.realtimeSinceStartup - lastStateChange;
    }

    private float timeInThisPath()
    {
        return Time.realtimeSinceStartup - lastPathChange;
    }

    private void setState(CharState state)
    {
        currentState = state;
        lastStateChange = Time.realtimeSinceStartup;

        foreach (CharState value in Enum.GetValues(typeof(CharState)))
        {
            animator.SetBool(value.ToString(),currentState == value);
        }
        
        switch (state)
        {
            case CharState.Talking:
                agent.isStopped = true;
                animator.speed = 0.6f + Random.value * 0.4f;
                break;
            default:
                agent.isStopped = false;
                animator.speed = 1f;
                break;
        }
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

        lastPathChange = Time.realtimeSinceStartup;
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