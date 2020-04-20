using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;


public enum CharState{Idle, Walking, Talking, Scared, Running }

public class HumanController : MonoBehaviour
{
    public bool debugMode = false;

    public BoxCollider area;
    private float walkRadius = 30;
    private float changePathTimeout = 5f;
    private float walkThreshold = 0.1f;

    private Vector3 lastPosition;

    private NavMeshAgent agent;

    private Animator animator;
    
    private CharState currentState;
    private float lastStateChange = 0f;
    private float lastPathChange = 0f;
    private HumanController other;
    private Vector3 lastWalkingOrientation;
    private Vector3 duckPosition;

    private float defaultSpeed;

    public GameObject bones;

    private float timeToStartWalking;
    private float timeTalking;

    private AudioSource audio;
    private AudioClip[] shouts;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        shouts = Resources.LoadAll<AudioClip>("sounds/human");
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        defaultSpeed = agent.speed;
        agent.destination = GetRandomPoint();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
        setState(CharState.Idle);
        bones.SetActive(false);
    }


    void Update()
    {
        var close = getCloseHumans();

        //talk with someone
        if (currentState == CharState.Idle)
        {
            if (timeInThisState() > timeToStartWalking)
                setState(CharState.Walking);
        }

        if (currentState == CharState.Walking)
        {
            lastWalkingOrientation = transform.forward;

            //talk with someone
            if (timeInThisState() > 6f)
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

            //exit condition
            if (agent.remainingDistance == Mathf.Infinity ||(!agent.pathPending && agent.remainingDistance < walkThreshold))
                setState(CharState.Idle);

        }

        //while talking, look at the other character
        if (currentState == CharState.Talking)
        {
            transform.forward = Vector3.Lerp(lastWalkingOrientation,(other.transform.position - transform.position), timeInThisState() * 5f);

            //stop talking
            if (timeInThisState() > timeTalking)
            {
                animator.SetBool("Talking", false);
                setState(CharState.Idle);
            }
                
        }

        //pass from scared to running
        if (currentState == CharState.Scared && timeInThisState() > 2f)
        {
            var nextPosition = transform.position + (transform.position - duckPosition).normalized * walkRadius;

            lastPathChange = Time.realtimeSinceStartup;
            lastPosition = transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(nextPosition, out hit, walkRadius, 1);

            if (debugMode) Debug.Log(hit.position);

            agent.SetDestination(hit.position);

            animator.SetBool("Scared", false);
            setState(CharState.Running);
        }

        //pass from running to idle
        if (currentState == CharState.Running)
        {
            if (agent.remainingDistance == Mathf.Infinity 
                || (!agent.pathPending && agent.remainingDistance < walkThreshold)
                || timeInThisState() > 15f)
            {
                animator.SetBool("Running", false);
                setState(CharState.Idle);
            }
        }
    }

    private void LateUpdate()
    {
        animator.SetBool("Walking", agent.velocity.normalized.magnitude > 0);
    }

    public void duckQuacked(Vector3 duckPosition)
    {
        if (currentState != CharState.Running && currentState != CharState.Scared)
        {
            setState(CharState.Scared);
            this.duckPosition = duckPosition;
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

        if (debugMode)  Debug.Log("STATE: " + state);
        
        switch (state)
        {
            case CharState.Idle:
                var rand = Random.Range(0, 100);
                if (rand < 100)
                    timeToStartWalking = Random.Range(1f, 5f);
                else
                    timeToStartWalking = Random.Range(1f, 30f);

                agent.speed = defaultSpeed;
                agent.isStopped = false;
                animator.speed = 1f;
                break;
            case CharState.Walking:
                var nextPosition = GetRandomPoint();
                agent.speed = defaultSpeed;
                agent.isStopped = false;
                animator.speed = 1f;
                agent.SetDestination(nextPosition);
                break;
            case CharState.Running:
                animator.SetBool("Running", true);
                animator.speed = 1f;
                agent.speed = defaultSpeed * 2;
                agent.isStopped = false;
                break;
            case CharState.Scared:
                audio.PlayOneShot(getRandomShout());
                animator.SetBool("Scared", true);
                animator.speed = 1f;
                agent.isStopped = true;
                return;
            case CharState.Talking:
                animator.SetBool("Talking", true);
                timeTalking = Random.Range(6f, 12f);
                agent.isStopped = true;
                animator.speed = 0.6f + Random.value * 0.4f;
                break;
            default:
                agent.speed = defaultSpeed;
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
            if (currentState == CharState.Scared || currentState == CharState.Running)
            {
                nextPosition = transform.position + (transform.position - duckPosition).normalized * 500f;
            }

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

    public void Die() {
        animator.enabled = false;
        bones.SetActive(true);
        agent.enabled = false;
        this.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    private AudioClip getRandomShout()
    {
        return shouts[Random.Range(0, shouts.Length)];
    }

}